using MassApplication.Models;
using MassApplication.Models.Graph;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Controllers
{
    public class GraphController : Controller
    {
        public List<ResultJsonData> resultJsons;
        public List<KeyValuePair<string, string[]>> list;
        NpgsqlConnection con = new NpgsqlConnection();
        public object ResultKPIName, ResultChartDetails;
        public int GetkpiId;
        public int global_flag = 0;
        string region = ClassFolder.Login_Class.region_id;
        string user_dash_id = ClassFolder.Login_Class.dashboard_Id;
        int roleId = Convert.ToInt32(ClassFolder.Login_Class.role_id);
        public static string schemapath = ConfigurationManager.AppSettings["SchemaPath"];
        ChartParameters getQueriesData = new ChartParameters();
        public static Diagnostic_Class diagnosticClass = new Diagnostic_Class();
        public static GC_Class gcClass = new GC_Class();
        public static string start_data_date = "";
        public static string end_data_date = "";
        public static int assetID = 0;
        public static int streamNO = 0;
        public static string defaultcolor = ConfigurationManager.AppSettings["DefaultColor"];

        /// <summary>
        /// This method is called when we want to create new KPI 
        /// </summary>
        /// <param name="chartpa"></param>
        /// In this parameter we defined the chart parameter
        /// <param name="chart_id"></param>
        /// In this paramter we defined the chart id
        /// <param name="chart_type_name"></param>
        /// This defined the chart type like chart(area, line, bubble, etc.), tabular and label
        /// <param name="report_id"></param>
        /// This defined the report id
        /// <param name="edit_flag"></param>
        /// This defined the edit flag which check is this on edit mode or not
        /// <param name="chartpara_edit"></param>
        /// This defined the chart parameter in edit mode
        /// <param name="charttype_id_edit"></param>
        /// This defined the chart type id in edit mode
        /// <param name="charttype_name_edit"></param>
        /// This defined the chart type name like chart(area, line, bubble, etc.), tabular and label in edit mode
        /// <param name="query_id_edit"></param>
        /// This defined the query id in edit mode
        /// <returns></returns>
        public ActionResult ChartView(string chartpa, string chart_id, string chart_type_name, string report_id, string edit_flag, string chartpara_edit, string charttype_id_edit, string charttype_name_edit, string query_id_edit)
        {
            try
            {
                Session["edit_select"] = null;
                TempData["temp_data_id"] = null;
                if (chart_id != null)
                {
                    getQueriesData.chart_selected_type = chart_id;
                    getQueriesData.chart_selected_params = chartpa;
                    Session["chart_params"] = chartpa;
                    Session["chart_type"] = chart_id;
                    Session["chart_type_name"] = chart_type_name;


                }
                if (report_id != null)
                {
                    @Session["edit_select"] = null;
                    Session["reportid_edit"] = null;
                }
                if (edit_flag != null)
                {
                    Session["edit_select"] = edit_flag;
                }
                List<ChartParameters> getQueries = GetScriptData.GetParameters().ToList();
                //ChartParameters getQueriesData = new ChartParameters();
                getQueriesData.chartlist = getQueries;
                //ViewBag.Data_id = null;
                List<GetQueriesData> getQueries1 = GetScriptData.Getscript().ToList();
                getQueriesData.Listquery = getQueries1;
                if (chartpara_edit != null)
                {
                    getQueriesData.chart_selected_params_edit = chartpara_edit;
                    getQueriesData.chart_type_edit = charttype_id_edit;
                    getQueriesData.chart_type_name_edit = charttype_name_edit;
                    // getQueriesData.json_tuple = TempData["Jsondata"];// Session["jsondata"];
                    DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, Convert.ToInt32(query_id_edit));
                    string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
                    getQueriesData.json_tuple = convertJson;
                }
                //Session["Data_id"] = null;
                ViewBag.DefineColor = defaultcolor;
                ViewBag.UserRoleType = BaseController.GetUserTypeDD();
                return View(getQueriesData);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is called when we submit/save the KPI
        /// </summary>
        /// <param name="ds"></param>
        /// This parameter hold all the kpi related data in it 
        /// <returns></returns>
        [HttpPost]
        public ActionResult kpiSave(kpiclass_chart ds)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                //bool editStatus;

                //int systemKPI_flag = 0;
                //if (ds.system_kpi)
                //{
                //    systemKPI_flag = 1;
                //}
                //else
                //{
                //    systemKPI_flag = 0;
                //}

                int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                int chart_id = ds.chart_id;
                int chart_bound = 50;
                string name = ds.kpi_name;
                string description = ds.kpi_description;
                string position = "1";
                string chart_type = ds.kpi_chart_type;
                string chart_parameters = ds.kpi_chart_parameters;
                int user_type_id = ds.kpi_user_id;
                int is_deleted = 0;
                string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int created_by = 1;
                int modified_by = 1;
                Hashtable ht = new Hashtable();
                //connectionString();
                con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                con.Open();

                string check_kpi = "select kpi_name from " + schemapath + " dashboard_kpi_charts where kpi_name ='" + name + "'";
                cmd = new NpgsqlCommand(check_kpi, con);
                cmd.ExecuteNonQuery();
                string kpi_name = Convert.ToString(cmd.ExecuteScalar());
                if (Session["reportid_edit"] != null)
                {
                    string db_sql = " Update " + schemapath + "dashboard_kpi_charts set chart_type_id='" + chart_id + "',kpi_name='" + name + "',chart_params='" + chart_parameters + "',created_by=" + created_by + ",created_on= '" + created_on + "',modified_by=" + modified_by + ",modified_on= '" + created_on + "',is_deleted= " + is_deleted + ",description='" + description + "',chart_bounds='" + chart_bound + "',chart_position= '" + position + "' where kpi_id='" + Session["reportid_edit"] + "'";

                    cmd = new NpgsqlCommand(db_sql, con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                else
                {

                    if (kpi_name == name)
                    {
                        return Content("Error;", "Entry already exits!!!");
                    }
                    else
                    {
                        //string db_sql = "Insert into " + schemapath + "dashboard_kpi_charts (chart_type_id,kpi_name,chart_params, created_by, created_on, modified_by, modified_on, is_deleted, description,chart_bounds,chart_position,system_kpi) Values('" + chart_id + "','" + name + "','" + chart_parameters + "'," + created_by + ",'" + created_on + "'," + modified_by + ",'" + modified_on + "'," + is_deleted + ",'" + description + "','" + chart_bound + "','" + position + "'," + systemKPI_flag + ") RETURNING kpi_id;";
                        string db_sql = "Insert into " + schemapath + "dashboard_kpi_charts (chart_type_id,kpi_name,chart_params, created_by, created_on, modified_by, modified_on, is_deleted, description,chart_bounds,chart_position,system_kpi) Values('" + chart_id + "','" + name + "','" + chart_parameters + "'," + user_id + ",'" + created_on + "'," + user_id + ",'" + created_on + "'," + is_deleted + ",'" + description + "','" + chart_bound + "','" + position + "'," + user_type_id + ") RETURNING kpi_id;";

                        cmd = new NpgsqlCommand(db_sql, con);
                        int db_id = Convert.ToInt32(cmd.ExecuteScalar());

                        ChartParameters getQueriesData = new ChartParameters();

                        int query_id = int.Parse(Session["ActiveQ"].ToString()); //getQueriesData.Active_queryID;

                        string db_sqlq = "Insert into " + schemapath + "dashboard_query_map(kpi_id,query_id) Values('" + db_id + "','" + query_id + "') RETURNING map_id;";
                        cmd = new NpgsqlCommand(db_sqlq, con);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                    }

                }

                //return RedirectToAction("kpiview", "Graph");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "KPI" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        public ActionResult kpiview()
        {
            if (region == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No network assign...!'); window.location.href = 'HomePage'; </script>");
            }
            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                return View();
            }
        }

        public ActionResult kpiviewdata()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            kpiview db = new kpiview();
            List<kpiview> menuItems = db.getAllkpiview(schemapath);
            ViewBag.ScheduleItems = menuItems;
            return PartialView("_partial_kpiviewdata", menuItems);
        }

        /// <summary>
        /// This method set the active or disable state of KPI
        /// </summary>
        /// <param name="report_id"></param>
        /// Set active or disable state of KPI using kpi_id parameter
        /// <param name="IsActiveState"></param>
        /// Set active or disable state of KPI using is_deleted parameter
        /// <returns></returns>
        public ActionResult UpdateRptData(string report_id, string IsActiveState)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string id = report_id;
                string is_deleted = IsActiveState;
                string sql = ClassFolder.Query_Class.UpdateKPI_mst(schemapath, is_deleted, id);
                bool status = ClassFolder.Database_Class.ExecuteQuery(sql);
                //return RedirectToAction("kpiview", "Graph");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "KPI" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method show the query data(table format) on the left side panel in ChartView
        /// </summary>
        /// <param name="getQuery_id"></param>
        /// This parameter hold the query id 
        /// <returns></returns>
        [HttpPost]
        public ActionResult getdataby_id(int getQuery_id) //public ActionResult PostQueryScript(int getQuery_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                //DataTable resultdtt = GetScriptData.ConvertinJsonDatatbl(getQueries);
                List<string> data_c = new List<string>();

                //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(getQuery_id);
                //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
                Session["Data_id"] = getQuery_id;
                string dataType = ClassFolder.GetValue.GetKpiParameterType(int.Parse(Session["Data_id"].ToString()));
                DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, Convert.ToInt32(Session["Data_id"]), dataType);//change data_id


                if (dataType.ToLower() == "diagnostic")
                {
                    DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt);
                    resultdtt = new DataTable();
                    resultdtt = dtNew.Copy();
                }

                string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                    dict.Add(resultdtt.Columns[i].ColumnName, resultdtt.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt.Columns[i]])).ToArray());

                resultJsons = new List<ResultJsonData>();
                ResultJsonData rsclass = new ResultJsonData();

                list = dict.ToList();

                foreach (KeyValuePair<string, string[]> pair in list)
                {
                    rsclass.keydata = pair.Key;
                    rsclass.valdata = pair.Value;
                }
                if (data_c.Count != 0)
                {
                    data_c.Clear();
                }
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                {
                    data_c.Add(resultdtt.Columns[i].ColumnName);
                }
                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.data_columns = data_c;
                getQueriesData.data_t = resultdtt;
                ViewBag.JsonData = resultdtt;
                //Session["Data_id"] = getQuery_id;
                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.chartlist = chartparamd;
                getQueriesData.json_tuple = convertJson;
                ViewBag.json_tuple = convertJson;
                getQueriesData.Active_queryID = getQuery_id;
                //TempData["temp_data_id"] = query_id;
                getQueriesData.keyvalue_tuple = list;
                return PartialView("_partialgetdataby_id", getQueriesData);
                //return RedirectToAction("ChartView", convertJson);
            }
            catch (Exception ex)
            {
                //ClassFolder.Error.GetErroronNotepad(ex.Message.ToString());
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called when we click on particular query then that query exceute
        /// and the result of that shown on the right side panel as table format in ChartView
        /// </summary>
        /// <param name="getQuery_id"></param>
        /// This parameter hold the query id 
        /// <returns></returns>
        public ActionResult _partialgetdataby_id_columns(int getQuery_id)//public ActionResult PostQueryScript(int getQuery_id)
        {
            //int aa = int.Parse(TempData["temp_data_id"].ToString());
            var aa = Session["Data_id"];
            if (Session["Data_id"] != null)//
            {
                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                //DataTable resultdtt = GetScriptData.ConvertinJsonDatatbl(getQueries);
                List<string> data_c = new List<string>();

                string dataType = ClassFolder.GetValue.GetKpiParameterType(int.Parse(Session["Data_id"].ToString()));
                DataTable resultdtt1 = Dashboard_kpi_class.ChartQueryData(schemapath, int.Parse(Session["Data_id"].ToString()), dataType);

                if (dataType.ToLower() == "diagnostic")
                {
                    DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt1);
                    resultdtt1 = new DataTable();
                    resultdtt1 = dtNew.Copy();
                }

                //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt1);
                string convertJson = JsonConvert.SerializeObject(resultdtt1);
                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                for (int ii = 0; ii < resultdtt1.Columns.Count; ii++)
                    dict.Add(resultdtt1.Columns[ii].ColumnName, resultdtt1.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt1.Columns[ii]])).ToArray());

                resultJsons = new List<ResultJsonData>();
                ResultJsonData rsclass = new ResultJsonData();
                list = dict.ToList();
                foreach (KeyValuePair<string, string[]> pair in list)
                {
                    rsclass.keydata = pair.Key;
                    rsclass.valdata = pair.Value;
                }

                if (data_c.Count != 0)
                {
                    data_c.Clear();
                }

                for (int i = 0; i < resultdtt1.Columns.Count; i++)
                {
                    data_c.Add(resultdtt1.Columns[i].ColumnName);
                }

                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.data_columns = data_c;
                getQueriesData.data_t = resultdtt1;
                // Session["Data_id"] = getQuery_id;
                TempData["temp_data_id"] = getQuery_id;
                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.chartlist = chartparamd;
                getQueriesData.json_tuple = convertJson;
                getQueriesData.Active_queryID = getQuery_id;
                Session["ActiveQ"] = Session["Data_id"];
                getQueriesData.keyvalue_tuple = list;
                ViewBag.DefineColor = defaultcolor;
            }
            return PartialView("_partialgetdataby_id_columns", getQueriesData);//
        }
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {


                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Creates partial view for viewing tabular format of data in KPI Designer
        /// </summary>
        /// <param name="columnname"></param>
        /// Shows the column name required for tabular data
        /// <returns></returns>
        public ActionResult tabular_plot(string columnname)
        {
            try 
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                List<string> data_c = new List<string>();

                string dataType = ClassFolder.GetValue.GetKpiParameterType(int.Parse(Session["Data_id"].ToString()));
                //columnname = "pipeline_name as pipeline,sales,purchases";
                DataTable resultdtt = Dashboard_kpi_class.ChartQuery_tabularData(schemapath, int.Parse(Session["Data_id"].ToString()), columnname);
                string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                //object sumObject;
                //sumObject = resultdtt.Compute(columnname, string.Empty);
                ////sumObject = resultdtt.Select(columnname);
                //string valueData = sumObject.ToString();


                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                    dict.Add(resultdtt.Columns[i].ColumnName, resultdtt.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt.Columns[i]])).ToArray());

                resultJsons = new List<ResultJsonData>();
                ResultJsonData rsclass = new ResultJsonData();

                list = dict.ToList();

                foreach (KeyValuePair<string, string[]> pair in list)
                {
                    rsclass.keydata = pair.Key;
                    rsclass.valdata = pair.Value;

                }
                if (data_c.Count != 0)
                {
                    data_c.Clear();
                }
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                {
                    data_c.Add(resultdtt.Columns[i].ColumnName);
                }
                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.data_columns = data_c;
                getQueriesData.data_t = resultdtt;

                getQueriesData.chartlist = chartparamd;
                getQueriesData.json_tuple = convertJson;
                getQueriesData.keyvalue_tuple = list;
                return PartialView("partial_tabular_plot", getQueriesData);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
      
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="columnname"></param>
        /// <returns></returns>
        public ActionResult tabular_plot_edit(string columnname)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            var result = new Hashtable();
            result = JsonConvert.DeserializeObject<Hashtable>(columnname);

            var chartpara = "";
            //var dta = "";
            //var dtv = "";
            foreach (DictionaryEntry entry in result)
            {
                if (entry.Value != null)
                {
                    chartpara += entry.Key + " as " + "\"" + entry.Value + "\"" + ",";
                }
                else
                {
                    chartpara += entry.Key + " as " + "\"" + entry.Value + "\"" + ",";
                }
            }

            string s = chartpara.Remove(chartpara.Length - 1, 1);


            List<ChartParameters> chartparamd = GetScriptData.GetParameters();
            List<string> data_c = new List<string>();
            //columnname = "pipeline_name as pipeline,sales,purchases";
            DataTable resultdtt = Dashboard_kpi_class.ChartQuery_tabularData(schemapath, int.Parse(Session["Data_id"].ToString()), s);
            string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            for (int i = 0; i < resultdtt.Columns.Count; i++)
                dict.Add(resultdtt.Columns[i].ColumnName, resultdtt.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt.Columns[i]])).ToArray());

            resultJsons = new List<ResultJsonData>();
            ResultJsonData rsclass = new ResultJsonData();

            list = dict.ToList();

            foreach (KeyValuePair<string, string[]> pair in list)
            {
                rsclass.keydata = pair.Key;
                rsclass.valdata = pair.Value;

            }
            if (data_c.Count != 0)
            {
                data_c.Clear();
            }
            for (int i = 0; i < resultdtt.Columns.Count; i++)
            {
                data_c.Add(resultdtt.Columns[i].ColumnName);
            }
            //ChartParameters tuplesData = new ChartParameters();
            getQueriesData.data_columns = data_c;
            getQueriesData.data_t = resultdtt;

            getQueriesData.chartlist = chartparamd;
            getQueriesData.json_tuple = convertJson;
            getQueriesData.keyvalue_tuple = list;
            return PartialView("partial_tabular_plot", getQueriesData);
        }

        /// <summary>
        /// This method is called when create tabular chart so this design tabular KPI Viewer on right side
        /// where we can drag and drop the column and also add the column 
        /// whatever the column we selected, their tabular format can be shown on ChartView
        /// </summary>
        /// <param name="getQuery_id"></param>
        /// All this done using query id parameter
        /// <returns></returns>
        public ActionResult _partialgetdataby_tabular(int getQuery_id)//public ActionResult PostQueryScript(int getQuery_id)
        {
            if (Session["Data_id"] != null)
            {
                var aa = Session["Data_id"];
                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                //DataTable resultdtt = GetScriptData.ConvertinJsonDatatbl(getQueries);
                List<string> data_c = new List<string>();

                string dataType = ClassFolder.GetValue.GetKpiParameterType(int.Parse(Session["Data_id"].ToString()));
                // DataTable resultdtt1 = Dashboard_kpi_class.ChartQueryData(schemapath, int.Parse(Session["Data_id"].ToString()));
                DataTable resultdtt1 = Dashboard_kpi_class.ChartQueryData(schemapath, int.Parse(Session["Data_id"].ToString()), dataType);

                if (dataType.ToLower() == "diagnostic")
                {
                    DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt1);
                    resultdtt1 = new DataTable();
                    resultdtt1 = dtNew.Copy();
                }

                string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt1);

                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                for (int ii = 0; ii < resultdtt1.Columns.Count; ii++)
                    dict.Add(resultdtt1.Columns[ii].ColumnName, resultdtt1.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt1.Columns[ii]])).ToArray());

                resultJsons = new List<ResultJsonData>();
                ResultJsonData rsclass = new ResultJsonData();
                list = dict.ToList();
                foreach (KeyValuePair<string, string[]> pair in list)
                {
                    rsclass.keydata = pair.Key;
                    rsclass.valdata = pair.Value;
                }

                if (data_c.Count != 0)
                {
                    data_c.Clear();
                }

                for (int i = 0; i < resultdtt1.Columns.Count; i++)
                {
                    data_c.Add(resultdtt1.Columns[i].ColumnName);
                }

                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.data_columns = data_c;
                getQueriesData.data_t = resultdtt1;
                //Session["Data_id"] = getQuery_id;
                TempData["temp_data_id"] = getQuery_id;
                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.chartlist = chartparamd;
                getQueriesData.json_tuple = convertJson;
                getQueriesData.Active_queryID = Convert.ToInt32(Session["Data_id"]);
                Session["ActiveQ"] = Session["Data_id"];
                getQueriesData.keyvalue_tuple = list;
            }

            return PartialView("_partialgetdataby_tabular", getQueriesData);//
        }
       
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="columnname"></param>
        /// <param name="label1"></param>
        /// <param name="caption1"></param>
        /// <param name="select1"></param>
        /// <param name="color1"></param>
        /// <returns></returns>
        public ActionResult _partialgetdataby_label(string columnname, string label1, string caption1, string select1, string color1)//string columnname)//public ActionResult PostQueryScript(int getQuery_id)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            if (label1 != "")
            {
                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                List<string> data_c = new List<string>();
                label1 = select1 + "(" + label1 + ")";


                DataTable resultdtt = Dashboard_kpi_class.ChartQuery_labelData(schemapath, int.Parse(Session["Data_id"].ToString()), label1);


                //ChartParameters tuplesData = new ChartParameters();
                getQueriesData.labelplotname = caption1;
                getQueriesData.colorcodevalues = color1;
                getQueriesData.labelplotvalues = resultdtt.Rows[0][select1].ToString();
                return PartialView("_partialgetdataby_label", getQueriesData);
            }
            else
            {
                return Content("Error: Label cannot be null!!!");
            }

        }

        /// <summary>
        /// This method is called when create label chart so this design label KPI Viewer on right side
        /// where we can create multiple label on 3*3 gid format
        /// whatever the column we selected, their label format can be shown on ChartView 
        /// </summary>
        /// <param name="jsonInput"></param>
        /// All this done using jsonInput parameter which hold the json values of label data
        /// <returns></returns>
        [HttpPost]
        public ActionResult _partialLabelChart(string jsonInput)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                if (jsonInput != "")
                {
                    List<ChartParameters> chartparams = new List<ChartParameters>();
                    var details = JsonConvert.DeserializeObject(jsonInput);
                    var array = (details as IEnumerable).Cast<object>().Select(x => x.ToString()).ToArray();
                    string labels = "", lbl_value = "", captions = "", select = "", select_value = "", cap_value = "", colors = "", drop_position = "";
                    foreach (var itemVal in array)
                    {
                        if (itemVal != "")
                        {
                            Dictionary<string, string> dictData = JsonConvert.DeserializeObject<Dictionary<string, string>>(itemVal);
                            foreach (var item in dictData)
                            {
                                if (item.Key.ToLower().Equals("label"))
                                {
                                    labels = item.Key;
                                    lbl_value = item.Value;
                                }
                                else if (item.Key.ToLower().Equals("caption"))
                                {
                                    captions = item.Value;
                                    if (captions == "")
                                    {
                                        captions = lbl_value;
                                        cap_value = captions;
                                    }
                                    else
                                    {
                                        captions = item.Value;
                                        cap_value = captions;
                                    }

                                }
                                else if (item.Key.ToLower().Equals("togglepaletteonly"))
                                {
                                    if (item.Value != "")
                                    {
                                        colors = item.Value;
                                    }

                                }
                                else if (item.Key.ToLower().Equals("drop_down_label"))
                                {
                                    select = item.Value.ToLower();
                                    select_value = select + "(" + lbl_value + ")";
                                    //vale = item.Value;
                                }
                                else if (item.Key.ToLower().Equals("drop_down_position"))
                                {
                                    drop_position = item.Value.ToLower();
                                }

                                else { }
                            }
                            //DataTable resultdtt = Dashboard_kpi_class.ChartQuery_labelData(schemapath, int.Parse(Session["Data_id"].ToString()), select_value);
                            DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, int.Parse(Session["Data_id"].ToString()));
                            ChartParameters cp = new ChartParameters();
                            cp.labelplotname = captions;
                            cp.colorcodevalues = colors;
                            cp.labelAlignment = drop_position;
                            //cp.labelplotvalues = resultdtt.Rows[0][select].ToString();
                            object sumObject;
                            sumObject = resultdtt.Compute(select_value, string.Empty);
                            cp.labelplotvalues = sumObject.ToString();
                            chartparams.Add(cp);
                        }

                    }
                    ViewBag.LabelColor = colors;
                    ViewBag.LabelList = chartparams;
                    return PartialView("_partialLabelChart", chartparams);
                }
                else
                {
                    return Content("Error: Label cannot be null!!!");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="columnname"></param>
        /// <param name="label1"></param>
        /// <param name="caption1"></param>
        /// <param name="select1"></param>
        /// <param name="color1"></param>
        /// <returns></returns>
        public ActionResult _partialgetdataby_label_edit(string columnname, string label1, string caption1, string select1, string color1)//string columnname)//public ActionResult PostQueryScript(int getQuery_id)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            var result = new Dictionary<string, string>();
            result = JsonConvert.DeserializeObject<Dictionary<string, string>>(columnname);
            List<ChartParameters> chartparamd = GetScriptData.GetParameters();


            caption1 = result["Caption"];
            label1 = result["Label"];
            select1 = result["Select"];
            color1 = result["Color"];


            List<string> data_c = new List<string>();
            label1 = select1 + "(" + label1 + ")";
            var query_id = Session["Data_id"].ToString();

            DataTable resultdtt = Dashboard_kpi_class.ChartQuery_labelData(schemapath, int.Parse(Session["Data_id"].ToString()), label1);

            //ChartParameters tuplesData = new ChartParameters();
            getQueriesData.labelplotname = caption1;
            getQueriesData.colorcodevalues = color1;
            getQueriesData.labelplotvalues = resultdtt.Rows[0][select1].ToString();
            return PartialView("_partialgetdataby_label", getQueriesData);//
        }

        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _partial_diagnostic_details1(Diagnostic_Class data)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                start_data_date = DateTime.Now.ToString("yyyy-MM-dd");
                end_data_date = DateTime.Now.ToString("yyyy-MM-dd");
                Diagnostic_Class db = new Diagnostic_Class();
                Diagnostic_Class menuItems = db.getDiagnosticDynamicItem(schemapath, data.abbrevation_name.ToUpper(), data, data.asset_id, data.stream_no, start_data_date, end_data_date);
                diagnosticClass = menuItems;
                return RedirectToAction("_partial_diagnostic_details", "Graph");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Not in used
        /// </summary>
        /// <returns></returns>
        public ActionResult _partial_diagnostic_details()
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                Diagnostic_Class menuItems = getDiagnosticValue(diagnosticClass.abbrevation_name.ToLower());
                return View("_partial_diagnostic_details", menuItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called when we double click on the digital twin table 
        /// then redirect to DiagnosticChartViewerData View
        /// Showing graph related to that parameter
        /// </summary>
        /// <param name="id"></param>
        /// Holds the asset id of digital twin
        /// <param name="consumer_name"></param>
        /// Holds the consumer name of digital twin
        /// <param name="meter_type"></param>
        /// Holds the meter type of digital twin
        /// <param name="stream_no"></param>
        /// Holds the stream number of digital twin
        /// <param name="maintenance_name"></param>
        /// Holds the maintenance name of digital twin
        /// <param name="pipeline_name"></param>
        /// Holds the pipeline name of digital twin
        /// <param name="abbrevation_name"></param>
        /// Holds the abbrevation name of digital twin
        /// <returns></returns>
        public ActionResult DiagnosticChartViewerData(int id, string consumer_name, string meter_type, int stream_no, string maintenance_name, string pipeline_name, string abbrevation_name)
        {
            start_data_date = DateTime.Now.ToString("yyyy-MM-dd");
            Diagnostic_Class db = new Diagnostic_Class();
            Diagnostic_Class menuItems = db.getDiagnosticDynamicItems(schemapath, abbrevation_name.ToUpper(), id, consumer_name, meter_type, stream_no, maintenance_name, pipeline_name, start_data_date);
            diagnosticClass = menuItems;
            return View("DiagnosticChartViewerData", menuItems);
        }

        /// <summary>
        /// This method is called when partial view rendered 
        /// </summary>
        /// <param name="menuName"></param>
        /// This hold the menu name in menuName parameter
        /// <returns></returns>
        public ActionResult DiagnosticViewerData(string menuName)
        {
            Diagnostic_Class menuItemData = getDiagnosticValue(menuName);
            return PartialView("_partial_diagnosticView", menuItemData);
        }

        /// <summary>
        /// This method fetch the Diagnostic_Class class data 
        /// </summary>
        /// <param name="menu_value"></param>
        /// using this paramter we hold the menu name
        /// <returns></returns>
        public Diagnostic_Class getDiagnosticValue(string menu_value)
        {
            Diagnostic_Class db = new Diagnostic_Class();
            Diagnostic_Class menuData = diagnosticClass;
            Diagnostic_Class menuItems = db.getDiagnosticDynamicItem(schemapath, menu_value.ToUpper(), menuData, menuData.asset_id, menuData.stream_no, start_data_date, end_data_date);
            return menuItems;
        }

        /// <summary>
        /// This method is called we filter graph data using date
        /// </summary>
        /// <param name="id"></param>
        /// holds the asset id of digital twin
        /// <param name="streamNum"></param>
        /// holds the stream number of digital twin
        /// <param name="start_date"></param>
        /// holds the start date of digital twin
        /// <param name="tab_name"></param>
        /// holds the tab name of digital twin
        /// <returns></returns>
        public ActionResult GetDynamicData(string id, string streamNum, string start_date, string tab_name)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                Diagnostic_Class db = new Diagnostic_Class();
                Diagnostic_Class menuDataItem = diagnosticClass;
                Diagnostic_Class menuDataItems = db.getDiagnosticDynamicItem(schemapath, tab_name.ToUpper(), menuDataItem, int.Parse(id), int.Parse(streamNum), start_date, start_date);
                diagnosticClass = menuDataItems;
                assetID = int.Parse(id);
                streamNO = int.Parse(streamNum);
                start_data_date = start_date;
                end_data_date = start_date;
                return PartialView("_partial_diagnosticView", menuDataItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// Not in used
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _partial_gc_details1(GC_Class data)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                start_data_date = DateTime.Now.ToString("yyyy-MM-dd");
                end_data_date = DateTime.Now.ToString("yyyy-MM-dd");
                GC_Class db = new GC_Class();
                GC_Class menuItems = db.getGCDynamicItem(schemapath, data.abbrevation_name.ToUpper(), data, data.gc_id, start_data_date, end_data_date);
                gcClass = menuItems;
                //return RedirectToAction("_partial_gc_details", "Graph", menuItems);
                return View("_partial_gc_details", menuItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Not in used
        /// </summary>
        /// <returns></returns>
        public ActionResult _partial_gc_details()
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                GC_Class menuItems = getGCValue(gcClass.abbrevation_name.ToLower());
                return View("_partial_gc_details", menuItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called when we double click on the GC table 
        /// then redirect to GCChartViewerData View
        /// Showing graph related to that parameter 
        /// </summary>
        /// <param name="id"></param>
        /// Holds the gc id of digital twin
        /// <param name="gc_name"></param>
        /// Holds the gc id of digital twin
        /// <param name="gc_network"></param>
        /// Holds the gc network of digital twin
        /// <param name="gc_station"></param>
        /// Holds the gc station of digital twin
        /// <param name="abbrevation_name"></param>
        /// Holds the abbrevation name of digital twin
        /// <param name="data_date"></param>
        /// Holds the start date of digital twin
        /// <returns></returns>
        public ActionResult GCChartViewerData(int id, string gc_name, string gc_network, string gc_station, string abbrevation_name, string data_date)
        {
            //start_data_date = DateTime.Now.ToString("yyyy-MM-dd");
            start_data_date = data_date;
            GC_Class db = new GC_Class();
            GC_Class menuItems = db.getGCDynamicItems(schemapath, abbrevation_name.ToUpper(), id, gc_name, gc_network, gc_station, start_data_date);
            gcClass = menuItems;
            return View("GCChartViewerData", menuItems);
        }

        /// <summary>
        /// This method is called when partial view rendered 
        /// </summary>
        /// <param name="menuName"></param>
        /// This hold the menu name in menuName parameter
        /// <returns></returns>
        public ActionResult GCViewerData(string menuName)
        {
            GC_Class menuItemData = getGCValue(menuName);
            return PartialView("_partialGCView", menuItemData);
        }

        /// <summary>
        ///  This method fetch the GC_Class class data 
        /// </summary>
        /// <param name="menu_value"></param>
        /// using this paramter we hold the menu name
        /// <returns></returns>
        public GC_Class getGCValue(string menu_value)
        {
            GC_Class db = new GC_Class();
            GC_Class menuData = gcClass;
            GC_Class menuItems = db.getGCDynamicItem(schemapath, menu_value.ToUpper(), menuData, menuData.gc_id, start_data_date, end_data_date);
            return menuItems;
        }

        /// <summary>
        /// This method is called we filter graph data using date
        /// </summary>
        /// <param name="id"></param>
        /// holds the gc id of GC
        /// <param name="start_date"></param>
        /// holds the start date of GC
        /// <param name="tab_name"></param>
        /// holds the tab name of GC
        /// <returns></returns>
        public ActionResult GetGCDynamicData(string id, string start_date, string tab_name)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                GC_Class db = new GC_Class();
                GC_Class menuDataItem = gcClass;
                GC_Class menuDataItems = db.getGCDynamicItem(schemapath, tab_name.ToUpper(), menuDataItem, int.Parse(id), start_date, start_date);
                gcClass = menuDataItems;
                int gcID = int.Parse(id);
                start_data_date = start_date;
                end_data_date = start_date;
                return PartialView("_partialGCView", menuDataItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        
        /// <summary>
        /// This method is called when we want to edit the KPI table using specific KPI id
        /// then that KPI detail related to that id is show on popup modal
        /// </summary>
        /// <param name="report_id"></param>
        /// This parameter hold the KPI id
        /// <returns></returns>
        public ActionResult EditChartView(int kpi_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                bool validateVal = ClassFolder.ValidationClass.ValidateKPIUrl(kpi_id);
                if (validateVal == true)
                {
                    con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    con.Open();
                    string dataid = "select query_id from " + schemapath + " dashboard_query_map  where kpi_id ='" + kpi_id + "'";
                    cmd = new NpgsqlCommand(dataid, con);
                    string query_id = Convert.ToString(cmd.ExecuteScalar());
                    Session["Data_id"] = query_id;

                    string edit_kpi_name = "";
                    string edit_kpi_description = "";
                    string chart_para_edit = "";
                    string chart_type_id_edit = "";
                    int system_kpi_edit = 0;
                    string chart_parameters_edit = "";
                    string chart_type_name_edit = "";
                    string chart_image_edit = "";

                    string chart_parmas_edit = "select kpi_name, description, chart_type_id, chart_params, system_kpi from " + schemapath + " dashboard_kpi_charts  where kpi_id ='" + kpi_id + "'";
                    cmd = new NpgsqlCommand(chart_parmas_edit, con);
                    //cmd.ExecuteNonQuery();
                    NpgsqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        edit_kpi_name = rd["kpi_name"].ToString();
                        edit_kpi_description = rd["description"].ToString();
                        chart_para_edit = rd["chart_params"].ToString();
                        chart_type_id_edit = rd["chart_type_id"].ToString();
                        system_kpi_edit = int.Parse(rd["system_kpi"].ToString());
                    }
                    cmd.Dispose();
                    con.Close();

                    //bool isCheckedValue = false;
                    //if (system_kpi_edit.ToLower() == "1")
                    //{
                    //    isCheckedValue = true;
                    //}

                    NpgsqlCommand cmd1 = new NpgsqlCommand();
                    cmd1.Connection = con;
                    con.Open();

                    string chart_type_e = "select chart_type,chart_parameters,chart_image  from " + schemapath + " dashboard_chart_mst  where chart_type_id ='" + chart_type_id_edit + "'";
                    cmd1 = new NpgsqlCommand(chart_type_e, con);
                    NpgsqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        chart_parameters_edit = rdr["chart_parameters"].ToString();
                        chart_type_name_edit = rdr["chart_type"].ToString();
                        chart_image_edit = rdr["chart_image"].ToString();
                    }
                    cmd1.Dispose();
                    con.Close();

                    List<ChartParameters> getChartParameters = GetScriptData.GetParameters().ToList();
                    getQueriesData.chartlist = getChartParameters;
                    List<GetQueriesData> getQueries = GetScriptData.Getscript().ToList();
                    getQueriesData.Listquery = getQueries;
                    getQueriesData.chart_selected_params_edit = chart_para_edit;
                    getQueriesData.ChartParams = chart_parameters_edit;
                    getQueriesData.chart_type_edit = chart_type_id_edit;
                    getQueriesData.chart_type_name_edit = chart_type_name_edit;
                    getQueriesData.image_path = chart_image_edit.Trim();
                    getQueriesData.kpi_data_id = kpi_id;
                    getQueriesData.kpi_data_name = edit_kpi_name;
                    getQueriesData.description = edit_kpi_description;
                    getQueriesData.kpi_user_type_id = system_kpi_edit;
                    //getQueriesData.IsChecked = isCheckedValue;
                    getQueriesData.Active_queryID = int.Parse(query_id);
                    Session["Data_id"] = query_id;
                    string dataType = ClassFolder.GetValue.GetKpiParameterType(int.Parse(query_id));
                    DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, Convert.ToInt32(query_id));
                    if (dataType.ToLower() == "diagnostic")
                    {
                        DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt);
                        resultdtt = new DataTable();
                        resultdtt = dtNew.Copy();
                    }
                    string convertJson = JsonConvert.SerializeObject(resultdtt);

                    Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                    for (int i = 0; i < resultdtt.Columns.Count; i++)
                        dict.Add(resultdtt.Columns[i].ColumnName, resultdtt.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt.Columns[i]])).ToArray());

                    List<KeyValuePair<string, string[]>> tupleList = dict.ToList();
                    getQueriesData.keyvalue_tuple = tupleList;
                    getQueriesData.json_tuple = convertJson;
                    ViewBag.JsonData = resultdtt;
                    ViewBag.DefineColor = defaultcolor;
                    ViewBag.EditUserRoleType = BaseController.GetUserTypeDD();
                    return View("EditChartView", getQueriesData);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = 'Dynamic/Index?menu_name=KPI'; </script>");
            }

        }

        /// <summary>
        /// This method is called when we click on particular query then that query exceute
        /// and the result of that shown on the right side panel as table format in Edit ChartView
        /// </summary>
        /// <param name="query_id"></param>
        /// This parameter hold the query id 
        /// <returns></returns>
        public ActionResult _partialgetEditdataby_id_columns(int query_id)
        {
            Session["Data_id"] = query_id;
            List<ChartParameters> chartparamd = GetScriptData.GetParameters();
            List<string> data_c = new List<string>();

            string dataType = ClassFolder.GetValue.GetKpiParameterType(query_id);
            DataTable resultdtt1 = Dashboard_kpi_class.ChartQueryData(schemapath, query_id, dataType);

            if (dataType.ToLower() == "diagnostic")
            {
                DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt1);
                resultdtt1 = new DataTable();
                resultdtt1 = dtNew.Copy();
            }

            string convertJson = JsonConvert.SerializeObject(resultdtt1);
            Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
            for (int ii = 0; ii < resultdtt1.Columns.Count; ii++)
                dict.Add(resultdtt1.Columns[ii].ColumnName, resultdtt1.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt1.Columns[ii]])).ToArray());

            resultJsons = new List<ResultJsonData>();
            ResultJsonData rsclass = new ResultJsonData();
            list = dict.ToList();
            foreach (KeyValuePair<string, string[]> pair in list)
            {
                rsclass.keydata = pair.Key;
                rsclass.valdata = pair.Value;
            }

            if (data_c.Count != 0)
            {
                data_c.Clear();
            }

            for (int i = 0; i < resultdtt1.Columns.Count; i++)
            {
                data_c.Add(resultdtt1.Columns[i].ColumnName);
            }
            //chart_selected_params_edit
            getQueriesData.data_columns = data_c;
            getQueriesData.data_t = resultdtt1;
            ViewBag.JsonData = resultdtt1;
            getQueriesData.chartlist = chartparamd;
            getQueriesData.json_tuple = convertJson;
            getQueriesData.Active_queryID = query_id;
            getQueriesData.keyvalue_tuple = list;
            string keyString = JsonConvert.SerializeObject(list);
            getQueriesData.chart_selected_keyvalue_params = keyString;
            ViewBag.DefineColor = defaultcolor;
            return PartialView("_partialgetEditdataby_id_columns", getQueriesData);
        }

        /// <summary>
        ///  This method show the query data(table format) on the left side panel in Edit ChartView
        /// </summary>
        /// <param name="getQuery_id"></param>
        /// This parameter hold the query id 
        /// <returns></returns>
        public ActionResult GetEditDataById(int getQuery_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                Session["Data_id"] = getQuery_id;
                List<ChartParameters> chartparamd = GetScriptData.GetParameters();
                List<string> data_c = new List<string>();

                string dataType = ClassFolder.GetValue.GetKpiParameterType(getQuery_id);
                DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, getQuery_id, dataType);//change data_id

                if (dataType.ToLower() == "diagnostic")
                {
                    DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt);
                    resultdtt = new DataTable();
                    resultdtt = dtNew.Copy();
                }

                string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                    dict.Add(resultdtt.Columns[i].ColumnName, resultdtt.Rows.Cast<DataRow>().Select(k => Convert.ToString(k[resultdtt.Columns[i]])).ToArray());

                resultJsons = new List<ResultJsonData>();
                ResultJsonData rsclass = new ResultJsonData();

                list = dict.ToList();

                foreach (KeyValuePair<string, string[]> pair in list)
                {
                    rsclass.keydata = pair.Key;
                    rsclass.valdata = pair.Value;
                }
                if (data_c.Count != 0)
                {
                    data_c.Clear();
                }
                for (int i = 0; i < resultdtt.Columns.Count; i++)
                {
                    data_c.Add(resultdtt.Columns[i].ColumnName);
                }
                getQueriesData.data_columns = data_c;
                getQueriesData.data_t = resultdtt;
                ViewBag.JsonData = resultdtt;
                getQueriesData.chartlist = chartparamd;
                getQueriesData.json_tuple = convertJson;
                ViewBag.json_tuple = convertJson;
                getQueriesData.Active_queryID = getQuery_id;
                getQueriesData.keyvalue_tuple = list;
                return PartialView("_partialgetdataby_id", getQueriesData);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        ///  This method is called when we UPDATE the KPI in Edit mode
        /// </summary>
        /// <param name="ds"></param>
        /// This parameter hold all the kpi related data in it 
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateKPI(kpiclass_chart ds)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int kpiId = ds.kpi_ID;
                string kpi_name = ds.kpi_name;
                string kpi_description = ds.kpi_description;
                int chart_id = ds.chart_id;
                int query_id = ds.kpi_query_ID;
                string chart_parameters = ds.kpi_chart_parameters;
                int user_type_id = ds.kpi_user_id;

                //int systemKPI_flag = 0;
                //if (ds.system_kpi)
                //{
                //    systemKPI_flag = 1;
                //}

                con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                con.Open();

                string edit_kpi_sql = "UPDATE " + schemapath + "dashboard_kpi_charts SET chart_type_id=" + chart_id + ",kpi_name='" + kpi_name + "',chart_params='" + chart_parameters + "',description='" + kpi_description + "',system_kpi=" + user_type_id + ",modified_on='" + created_on + "',modified_by=" + user_id + " WHERE kpi_id=" + kpiId + "";
                cmd = new NpgsqlCommand(edit_kpi_sql, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                string edit_query_sql = "UPDATE " + schemapath + "dashboard_query_map SET query_id=" + query_id + " where kpi_id=" + kpiId + "";
                cmd = new NpgsqlCommand(edit_query_sql, con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                con.Close();

                return Content("Data updated successfully!!!");
                //return RedirectToAction("Index", "Dynamic", new { @menu_name = "KPI" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }
    }
}