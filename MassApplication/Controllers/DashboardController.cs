using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using MassApplication.Models;
using MassApplication.Models.Graph;
using System.Data;
using Newtonsoft.Json;
using System.Collections;

namespace MassApplication.Controllers
{
    public class DashboardController : BaseController
    {
        NpgsqlConnection con = new NpgsqlConnection();
        public List<ResultJsonData> resultJsons;
        public List<KeyValuePair<string, string[]>> list;
        public static string Font_name = ConfigurationManager.AppSettings["FontName"];
        public static string Font_size = ConfigurationManager.AppSettings["FontSize"];
       
        /// <summary>
        /// This method is used for display the dashboard in home page
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeDashboard()
        {
            try
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

                    int id = 0;
                    if (Convert.ToInt32(Session["dashboard_id"]) == 0)
                    {
                         id = int.Parse(ClassFolder.Login_Class.active_dashboard);
                         //id = int.Parse(ClassFolder.Login_Class.dashboard_Id);
                    }
                    else
                    {
                        id = Convert.ToInt32(Session["dashboard_id"]);
                    }

                    //int id = Convert.ToInt32(Session["dashboard_id"]);
                    //int id = Convert.ToInt32(user_dash_id);
                    //List<GetQueriesData> getQueries = GetScriptData.Getscript().ToList();
                    Dashboard_class db = new Dashboard_class();
                    //Dashboard_class dashboardItems = db.getDashboardItems(schemapath, id);
                    Dashboard_class dashboardItems = db.getDashboardItemsDemo(schemapath, id);
                    //Session["dashboard_id"] = null;
                    ViewBag.LoginStatus = ClassFolder.Login_Class.login_status;
                    return View("HomeDashboard", dashboardItems);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
       
        /// <summary>
        /// This method is used to create new dashboard with using existing KPIs
        /// In this method, we load all the KPIs in view using ViewBag
        /// </summary>
        /// <returns></returns>
        public ActionResult StickyIndex()
        {
            try
            {
                Dashboard_kpi_class db = new Dashboard_kpi_class();
                List<Dashboard_kpi_class> kpiItems = db.getAllkpiItems(schemapath);
                ViewBag.kpiItems = kpiItems;
                ViewBag.FontName = Font_name;
                ViewBag.FontSize = Font_size;
                ViewBag.UserRoleType = GetUserTypeDD();
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
       
        /// <summary>
        /// This method is call when we drop the KPIs on the right side panel in StickyIndex View 
        /// This method return the chart parameters in json format 
        /// </summary>
        /// <param name="QueryId"></param>
        /// This parameter is used to fetch the KPI data according to given query Id
        /// <returns></returns>
        [HttpPost]
        public object NewChart(int QueryId)
        {
            string sql_chart_data = "Select chart_params,chart_type from mass2.dashboard_kpi_charts as kc left join mass2.dashboard_chart_mst as cm on kc.chart_type_id = cm.chart_type_id where kpi_id = " + QueryId + "";
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql_chart_data);

            string json = "";
            foreach (DataRow row in dt.Rows)
            {
                string name = row["chart_params"].ToString();
                string type = row["chart_type"].ToString();
                json = name;
            }

            var details = JsonConvert.DeserializeObject(json);

            Dashboard_kpi_class db = new Dashboard_kpi_class();
            Dashboard_kpi_class kpiItemsData = db.getAllkpiIDItems(schemapath, QueryId);
            ViewBag.ReconData = kpiItemsData;


            //var detailData = JsonConvert.DeserializeObject(json);

            //var detailType = chartType;
            // ViewData["ChartType"] = detailType;
            //var obj = new Object();
            //obj = { success: '' ,  }
            //var result = new { Success = "False", Message = "Error Message" };
            //var details = new { Success = json, Message = chartType };

            // var obj = { new Success : { detailData }, msg: { data: chartType } };
            //return Json(details, JsonRequestBehavior.AllowGet);


            return details;

        }
        
        /// <summary>
        /// This method return the chart parameters with its value in json format 
        /// </summary>
        /// <param name="QueryId"></param>
        /// This parameter is used to fetch the KPI data according to given query Id
        /// <returns></returns>
        [HttpPost]
        public JsonResult KpiData(int QueryId)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(QueryId);
            //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);


            DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, QueryId);
            //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
            string convertJson = JsonConvert.SerializeObject(resultdtt);

            return Json(convertJson, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// This method return the tabular parameters with its value in json format 
        /// </summary>
        /// <param name="QueryId"></param>
        /// This parameter is used to fetch the KPI data according to given query Id
        /// <param name="chartparams"></param>
        /// This parameter is used to fetch the KPI data according to parameter that it hold
        /// <returns></returns>
        [HttpPost]
        public JsonResult KpiTabularData(int QueryId, string chartparams)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            var details = JsonConvert.DeserializeObject(chartparams);
            var array = (details as IEnumerable).Cast<object>().Select(x => x.ToString()).ToArray();
            var arlist1 = new ArrayList();
            int pos = 0;
            string select = "", where = "", queryVal = "";

            foreach (var item in array)
            {
                pos = item.LastIndexOf(":");
                if (pos > 0)
                {
                    select = item.Substring(0, pos);
                    select = select.Replace(@"""", "");
                    where = item.Substring(pos);
                    queryVal = select + where.Replace(":", " as ");
                    arlist1.Add(queryVal);
                }
            }

            string[] asArr = new string[arlist1.Count];
            arlist1.CopyTo(asArr);
            string result = string.Join(",", asArr);

            //DataTable resultdtt = Dashboard_kpi_class.ChartQueryDataDemo(schemapath, QueryId, result);
            DataTable resultdtt = Dashboard_kpi_class.ChartQuery_tabularData(schemapath, QueryId, result);
            //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
            string convertJson = JsonConvert.SerializeObject(resultdtt);

            return Json(convertJson, JsonRequestBehavior.AllowGet);

        }
       
        /// <summary>
       /// Not in use
       /// </summary>
       /// <param name="QueryId"></param>
       /// <param name="chartparams"></param>
       /// <returns></returns>
        [HttpPost]
        public JsonResult KpiLabelData1(int QueryId, string chartparams)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            var details = JsonConvert.DeserializeObject(chartparams);
            Dictionary<string, string> dictData = JsonConvert.DeserializeObject<Dictionary<string, string>>(chartparams);
            var array = (details as IEnumerable).Cast<object>().Select(x => x.ToString()).ToArray();
            string labels = "", lbl_value = "", captions = "", select = "", cap_value = "", color = "";

            foreach (var item in dictData)
            {
                if (item.Key.ToLower().Contains("label"))
                {
                    labels = item.Key;
                    lbl_value = item.Value;
                }
                else if (item.Key.ToLower().Contains("caption"))
                {
                    captions = item.Value;
                    if (captions == "")
                    {
                        captions = labels;
                        cap_value = captions;
                    }
                    else
                    {
                        captions = item.Value;
                        cap_value = captions;
                    }

                }
                else if (item.Key.ToLower().Contains("select"))
                {
                    select = item.Value;
                    select = select + "(" + lbl_value + ")";
                    //vale = item.Value;
                }
                else if (item.Key.ToLower().Contains("color"))
                {
                    color = item.Value;
                }

                else { }

            }

            DataTable resultdtt = Dashboard_kpi_class.ChartQuery_labelData(schemapath, QueryId, select);
            string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

            var detailData = new { Success = convertJson, Message = color, DataVal = cap_value };
            return Json(detailData, JsonRequestBehavior.AllowGet);
            //return Json(convertJson, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// This method return the label parameters with its value in json format 
        /// </summary>
        /// <param name="QueryId"></param>
        /// This parameter is used to fetch the KPI data according to given query Id
        /// <param name="chartparams"></param>
        /// This parameter is used to fetch the KPI data according to parameter that it hold
        /// <returns></returns>
        [HttpPost]
        public JsonResult KpiLabelData(int QueryId, string chartparams)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            var details = JsonConvert.DeserializeObject(chartparams);
            Dictionary<string, string> dictData = JsonConvert.DeserializeObject<Dictionary<string, string>>(chartparams);
            var array = (details as IEnumerable).Cast<object>().Select(x => x.ToString()).ToArray();
            string labels = "", lbl_value = "", captions = "", select = "", select_value = "", cap_value = "", color = "", drop_position = "";
            List<string> chartParamsList = new List<string>();
            List<string> labelVal = new List<string>();
            List<string> captionVal = new List<string>();
            List<string> selectVal = new List<string>();
            List<string> positionVal = new List<string>();

            foreach (var item in dictData)
            {
                if (item.Key.ToLower().Equals("label"))
                {
                    labels = item.Key;
                    lbl_value = item.Value;
                    labelVal = lbl_value.Split(',').ToList();
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

                    captionVal = cap_value.Split(',').ToList();

                }
                else if (item.Key.ToLower().Equals("color"))
                {
                    if (item.Value != "")
                    {
                        color = item.Value;
                    }
                }
                else if (item.Key.ToLower().Equals("select"))
                {
                    select = item.Value.ToLower();
                    select_value = select + "(" + lbl_value + ")";
                    selectVal = select.Split(',').ToList();
                }
                else if (item.Key.ToLower().Equals("position"))
                {
                    drop_position = item.Value.ToLower();
                    positionVal = drop_position.Split(',').ToList();
                }

                else { }
            }

            for (int i = 0; i < labelVal.Count; i++)
            {
                string selectData = selectVal[i] + "(" + labelVal[i] + ")";
                //DataTable resultdtt = Dashboard_kpi_class.ChartQuery_labelData(schemapath, QueryId, selectData);
                //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schemapath, QueryId);
                object sumObject;
                sumObject = resultdtt.Compute(selectData, string.Empty);
                string convertJson = sumObject.ToString();
                chartParamsList.Add(convertJson);
            }

            var detailData = new { Success = chartParamsList, Message = color, DataVal = captionVal, DataVal1 = positionVal };
            return Json(detailData, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// This method is used for active and disable the dashboard using its dashboard id and is_deleted parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="is_deleted"></param>
        /// <returns></returns>
        public ActionResult DeleteDashboard(int id, bool is_deleted)
        {
            try
            {
                int delete_val = 0;
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                if (is_deleted.Equals(false) == true)
                {
                    delete_val = 1;
                }

                bool status;
                string sql = "UPDATE " + schemapath + "dashboard_mst Set is_deleted=" + delete_val + " WHERE dashboard_id = " + id + "";
                status = ClassFolder.Database_Class.ExecuteQuery(sql);
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Dashboard" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="dashboard_id"></param>
        /// <returns></returns>
        public ActionResult EditDashboardData(int dashboard_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                //List<GetQueriesData> getQueries = GetScriptData.Getscript().ToList();
                Dashboard_class db = new Dashboard_class();
                Dashboard_class dashboardItems = db.getDashboardItemsDemo(schemapath, dashboard_id);
                Dashboard_kpi_class db_kpi = new Dashboard_kpi_class();
                List<Dashboard_kpi_class> kpiItems = db_kpi.getAllkpiItems(schemapath);
                ViewBag.kpiChartItems = kpiItems;

                return View("EditDashboardData", dashboardItems);
            }
            catch
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = 'ReportGrid'; </script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = '/Dynamic/Index?menu_name=Dashboard'; </script>");
            }
        }
       
        /// <summary>
        /// This method is used for editing the dashboard using its dashboard id
        /// This method is called when we want to edit the dashboard table using specific dashboard id
        /// then that dashboard detail related to that id is show on popup modal
        /// </summary>
        /// <param name="dashboard_id"></param>
        /// This parameter hold the dashboard id
        /// <returns></returns>
        public ActionResult EditDashboardDemo(int dashboard_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                bool validateVal = ClassFolder.ValidationClass.ValidateDashboardUrl(dashboard_id);
                if (validateVal == true)
                {
                    Dashboard_class db = new Dashboard_class();
                    Dashboard_class dashboardItems = db.getDashboardItemsDemo(schemapath, dashboard_id);
                    Dashboard_kpi_class db_kpi = new Dashboard_kpi_class();
                    List<Dashboard_kpi_class> kpiItems = db_kpi.getAllkpiItems(schemapath);
                    ViewBag.kpiChartItems = kpiItems;
                    ViewBag.EditUserRoleType = GetUserTypeDD();
                    return View("EditDashboardDemo", dashboardItems);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = 'ReportGrid'; </script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = '/Dynamic/Index?menu_name=Dashboard'; </script>");
            }
        }

        /// <summary>
        /// This method is used for editing the dashboard using its dashboard id
        /// </summary>
        /// <param name="ds"></param>
        /// Basically this parameter is an instance of Dashboard_class class that hold the dashboard related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDbData(Dashboard_class ds)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                bool editStatus;
                int db_id = ds.dashboard_id;
                string name = ds.dashboard_name;
                string description = ds.dashboard_description;
                string position = ds.dashboard_position;
                string chart_type = ds.dashboard_chart_type;
                string chart_parameters = ds.dashboard_chart_parameters;
                int user_type_id = ds.dashboard_user_id;
                string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int created_by = 1;
                int kpiId = 0;
                int kpiQueryId = 0;
                Hashtable ht = new Hashtable();

                //connectionString();
                con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                con.Open();

                string update_db_sql = "UPDATE " + schemapath + "dashboard_mst Set dashboard_name='" + name + "', dashboard_description='" + description + "', dashboard_user_id=" + user_type_id + ", modified_on='" + created_on + "',modified_by=" + user_id + " WHERE dashboard_id = " + db_id + "";
                editStatus = ClassFolder.Database_Class.ExecuteQuery(update_db_sql);

                string kpimapDelete = "DELETE FROM " + schemapath + "dashboard_kpi_map WHERE dashboard_id IN(" + db_id + ")";
                editStatus = ClassFolder.Database_Class.ExecuteQuery(kpimapDelete);


                string chart_container, chart_top, chart_left, chart_height, chart_width, chart_bottom, chart_right, chart_position, kpiChartType, kpiProperty;
                string kpi_x, kpi_y, kpi_width, kpi_height;

                if (ds.kpiList != null)
                {
                    List<Dashboard_kpi_class> kpiclass = ds.kpiList.ToList();
                    var kpi_result = from s in kpiclass
                                     select s;
                    foreach (var data in kpi_result)
                    {
                        ht.Clear();
                        kpiId = data.kpi_id;
                        kpiQueryId = data.kpi_query_id;
                        kpiChartType = data.chart_type;
                        kpiProperty = data.kpi_property;
                        chart_container = data.chart_container.Trim();
                        chart_top = data.chart_top.Trim();
                        chart_left = data.chart_left.Trim();
                        chart_height = data.chart_height.Trim();
                        chart_width = data.chart_width.Trim();
                        chart_bottom = data.chart_bottom.Trim();
                        chart_right = data.chart_right.Trim();
                        kpi_x = data.kpi_x.Trim();
                        kpi_y = data.kpi_y.Trim();
                        kpi_width = data.kpi_width.Trim();
                        kpi_height = data.kpi_height.Trim();

                        ht.Add("chart_top", chart_top);
                        ht.Add("chart_left", chart_left);
                        ht.Add("chart_height", chart_height);
                        ht.Add("chart_width", chart_width);
                        ht.Add("chart_bottom", chart_bottom);
                        ht.Add("chart_right", chart_right);
                        ht.Add("kpi_x", kpi_x);
                        ht.Add("kpi_y", kpi_y);
                        ht.Add("kpi_width", kpi_width);
                        ht.Add("kpi_height", kpi_height);

                        chart_position = JsonConvert.SerializeObject(ht);

                        //string update_kpi_sql = "UPDATE " + schemapath + "dashboard_kpi_map Set kpi_position='" + chart_position + "'  WHERE dashboard_id = " + db_id + " AND kpi_id = " + kpiId + "";
                        //editStatus = ClassFolder.Database_Class.ExecuteQuery(update_kpi_sql);

                        string kpi_map_sql = "Insert into " + schemapath + "dashboard_kpi_map(dashboard_id, created_by, created_on, kpi_id, kpi_position, kpi_property) Values(" + db_id + "," + created_by + ",'" + created_on + "'," + kpiId + ",'" + chart_position + "','" + kpiProperty + "')"; ;
                        cmd = new NpgsqlCommand(kpi_map_sql, con);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                }

                //return RedirectToAction("DashboardList", "DashBoard");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Dashboard" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used for save the dashboard with its related data in database
        /// </summary>
        /// <param name="ds"></param>
        /// Basically this parameter is an instance of Dashboard_class class that hold the dashboard related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult DashboardSave(Dashboard_class ds)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                //bool editStatus;
                string name = ds.dashboard_name;
                string description = ds.dashboard_description;
                bool defaultDb = ds.defaultDashboard;
                //string dashboard_mode = ds.dashboard_mode;
                string dashboard_mode = ds.dashboard_position;
                string chart_type = ds.dashboard_chart_type;
                string chart_parameters = ds.dashboard_chart_parameters;
                int user_type_id = ds.dashboard_user_id;
                int is_deleted = 0;
                string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int created_by = 1;
                int kpiId = 0;
                int kpiQueryId = 0;
                Hashtable ht = new Hashtable();

                //connectionString();
                con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                con.Open();
                string db_sql = "Insert into " + schemapath + "dashboard_mst(dashboard_name, created_by, created_on, modified_by, modified_on, is_deleted, dashboard_position, dashboard_description, dashboard_user_id) Values('" + name + "'," + user_id + ",'" + created_on + "'," + user_id + ",'" + created_on + "'," + is_deleted + ",'" + dashboard_mode + "','" + description + "'," + user_type_id + ") RETURNING dashboard_id;";
                cmd = new NpgsqlCommand(db_sql, con);
                int db_id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                if (defaultDb == true)
                {
                    Session["dashboard_id"] = db_id;
                }
                else { }

                string chart_container, chart_top, chart_left, chart_height, chart_width, chart_bottom, chart_right, chart_position, kpiChartType, kpiProperty;
                string kpi_x, kpi_y, kpi_width, kpi_height;
                string map_sql;

                if (ds.kpiList != null)
                {
                    List<Dashboard_kpi_class> kpiclass = ds.kpiList.ToList();
                    var kpi_result = from s in kpiclass
                                     select s;
                    foreach (var data in kpi_result)
                    {
                        ht.Clear();
                        kpiId = data.kpi_id;
                        kpiQueryId = data.kpi_query_id;
                        kpiChartType = data.chart_type;
                        kpiProperty = data.kpi_property;
                        chart_container = data.chart_container.Trim();
                        chart_top = data.chart_top.Trim();
                        chart_left = data.chart_left.Trim();
                        chart_height = data.chart_height.Trim();
                        chart_width = data.chart_width.Trim();
                        chart_bottom = data.chart_bottom.Trim();
                        chart_right = data.chart_right.Trim();
                        kpi_x = data.kpi_x.Trim();
                        kpi_y = data.kpi_y.Trim();
                        kpi_width = data.kpi_width.Trim();
                        kpi_height = data.kpi_height.Trim();

                        ht.Add("chart_top", chart_top);
                        ht.Add("chart_left", chart_left);
                        ht.Add("chart_height", chart_height);
                        ht.Add("chart_width", chart_width);
                        ht.Add("chart_bottom", chart_bottom);
                        ht.Add("chart_right", chart_right);
                        ht.Add("kpi_x", kpi_x);
                        ht.Add("kpi_y", kpi_y);
                        ht.Add("kpi_width", kpi_width);
                        ht.Add("kpi_height", kpi_height);
                        chart_position = JsonConvert.SerializeObject(ht);

                        //map_sql = "Insert into " + schemapath + "dashboard_kpi_map(dashboard_id, created_by, created_on, kpi_id, kpi_position, kpi_property) Values(" + db_id + "," + user_id + ",'" + created_on + "'," + kpiId + ",'" + chart_position + "','" + kpiProperty + "')";
                        map_sql = "Insert into " + schemapath + "dashboard_kpi_map(dashboard_id, created_by, created_on, kpi_id, kpi_position, kpi_property) Values(" + db_id + "," + created_by + ",'" + created_on + "'," + kpiId + ",'" + chart_position + "','" + kpiProperty + "')";
                        cmd = new NpgsqlCommand(map_sql, con);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                }

                //return RedirectToAction("DashboardList", "DashBoard");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Dashboard" });

            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used for update the dashboard with its id and is_deleted parameter
        /// </summary>
        /// <param name="dashboard_id"></param>
        /// <param name="IsActiveState"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDashboardData(string dashboard_id, string IsActiveState)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string id = dashboard_id;
                string is_deleted = IsActiveState;
                string sql = ClassFolder.Query_Class.UpdateDashboard_mst(schemapath, is_deleted, id);
                bool status = ClassFolder.Database_Class.ExecuteQuery(sql);
                //return RedirectToAction("DashboardList", "DashBoard");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Dashboard" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is used for display the dashboard with its id parameter
        /// This method is used when we click the dashboard list item in View Dashboard menu list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowDashboard(int id)
        {
            try
            {
                bool validateVal = ClassFolder.ValidationClass.ValidateDashboardUrl(id);
                if (validateVal == true)
                {
                    Dashboard_class db = new Dashboard_class();
                    Dashboard_class menuItems = db.getDashboardItemsDemo(schemapath, id);
                    return View("ShowDashboard", menuItems);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used for display the dashboard with its id parameter
        /// This method is used when we click the preview button in dashboard grid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowGraphLayout(int id)
        {
            try
            {
                bool validateVal = ClassFolder.ValidationClass.ValidateDashboardUrl(id);
                if (validateVal == true)
                {
                    Dashboard_class db = new Dashboard_class();
                    Dashboard_class menuItems = db.getDashboardItemsDemo(schemapath, id);
                    ViewBag.FontName = Font_name;
                    ViewBag.FontSize = Font_size;
                    return View("ShowGraphLayout", menuItems);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called, when we want to create new query using popup modal
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewQuery()
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Connection_Class db = new Connection_Class();
                List<Connection_Class> ConnItems = db.getAllConnectionItems(schemapath);
                ViewBag.ConnectionItems = ConnItems;
                ViewBag.QueryType = getQueryType();
                ViewBag.UserRoleType = GetUserTypeDD();
                return PartialView("_partialNewQueryView");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is called, when we want to insert the query releted data in database
        /// </summary>
        /// <param name="querydata"></param>
        /// This parameter hold all the query releted data
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewQueryData(DBQueryClass querydata)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    using (NpgsqlConnection con = new NpgsqlConnection(constring))
                    {
                        int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                        int connectionId = querydata.con_id;
                        string name = querydata.query_name.Trim();
                        string type = querydata.query_type.ToLower().Trim();
                        string query = querydata.query_qrydata.ToLower().Trim();
                        int user_type_id = querydata.query_user_id;

                        int is_deleted = 0;
                        string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //int created_by = 1;
                        int q_id = 0;
                        string query_sql = "";
                        string rest = "";
                        string where = "";
                        string order_by = "";
                        string group_by = "";
                        string select = "";
                        int pos = 0;

                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = con;
                        con.Open();

                        //string from = "";
                        pos = query.LastIndexOf("where");
                        if (pos > 0)
                        {
                            select = query.Substring(0, pos);
                            where = query.Substring(pos);
                        }
                        else
                            select = query;
                        //Need to be think later how we will handle if order by or group by comes without where
                        if (where.Length > 0)
                        {
                            pos = where.LastIndexOf("order by");
                            if (pos > 0)
                            {
                                rest = where;
                                where = where.Substring(0, pos);
                                order_by = rest.Substring(pos);
                            }
                            pos = where.LastIndexOf("group by");
                            if (pos > 0)
                            {
                                rest = where;
                                where = where.Substring(0, pos);
                                group_by = rest.Substring(pos);
                            }
                        }

                        select = select.Replace("'", "''");
                        where = where.Replace("'", "''");
                        group_by = group_by.Replace("'", "''");
                        order_by = order_by.Replace("'", "''");

                        query_sql = "Insert into " + schemapath + "dashboard_query_mst(function_name, type, query_script, query_where_cond, query_group_by, query_order_by, created_on, created_by, modified_on, modified_by, is_deleted, dbconnection_id, query_user_id) Values('" + name + "','" + type + "','" + select + "','" + where + "','" + group_by + "','" + order_by + "','" + created_on + "'," + user_id + ",'" + created_on + "'," + user_id + "," + is_deleted + "," + connectionId + "," + user_type_id + ") RETURNING query_id;"; 
                         cmd = new NpgsqlCommand(query_sql, con);
                        q_id = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();
                    }

                    con.Close();
                    return RedirectToAction("Index", "Dynamic", new { @menu_name = "Query" });
                }

                catch (Exception ex)
                {
                    return Content("ERROR;" + ex.Message.ToString());
                }
            }

            return View();
        }

        /// <summary>
        /// This method is called when we want to edit the query table using specific query id
        /// then that query detail related to that id is show on popup modal
        /// </summary>
        /// <param name="queryId"></param>
        /// This parameter hold the query id
        /// <returns></returns>
        public ActionResult EditQueryData(int queryId)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                DBQueryClass qc = new DBQueryClass();
                DBQueryClass queryItems = qc.getQueryData(schemapath, queryId);
                Connection_Class db = new Connection_Class();
                List<Connection_Class> ConnItems = db.getAllConnectionItems(schemapath);
                ViewBag.EditQueryType = getQueryType();
                ViewBag.EditUserRoleType = GetUserTypeDD();
                ViewBag.ConnectionValues = ConnItems;

                return PartialView("_partialEditQueryView", queryItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is called when we update the query data using QueryData parameter
        /// This method is called after the submit button in query popup modal to update the query data values
        /// </summary>
        /// <param name="QueryData"></param>
        /// Basically this parameter is an instance of DBQueryClass class that hold the query related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQryData(DBQueryClass QueryData)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    bool editStatus;

                    int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                    int id = QueryData.queryId;
                    string name = QueryData.query_name;
                    string type = QueryData.query_type.ToLower().Trim();
                    string q_script = QueryData.query_qrydata.Trim().ToLower();
                    int connection_id = QueryData.con_id;
                    int query_user_id = QueryData.query_user_id;
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //connectionString();
                    con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    q_script = q_script.Replace("varunitfactor", "varUnitFactor");
                    q_script = q_script.Replace("varduration", "varDuration");
                    q_script = q_script.Replace("varstartcaldate", "varstartCalDate");
                    q_script = q_script.Replace("varendcaldate", "varendCalDate");
                    q_script = q_script.Replace("varpipelinemst_id", "varPipelinemst_id");
                    q_script = q_script.Replace("vardaycount", "varDayCount");
                    string rest = "";
                    string where = "";
                    string order_by = "";
                    string group_by = "";
                    string select = "";
                    int pos = 0;
                    //string from = "";
                    pos = q_script.LastIndexOf("where");
                    if (pos > 0)
                    {
                        select = q_script.Substring(0, pos);
                        where = q_script.Substring(pos);
                    }
                    else
                        select = q_script;
                    //Need to be think later how we will handle if order by or group by comes without where
                    if (where.Length > 0)
                    {
                        pos = where.LastIndexOf("order by");
                        if (pos > 0)
                        {
                            rest = where;
                            where = where.Substring(0, pos);
                            order_by = rest.Substring(pos);
                        }
                        pos = where.LastIndexOf("group by");
                        if (pos > 0)
                        {
                            rest = where;
                            where = where.Substring(0, pos);
                            group_by = rest.Substring(pos);
                        }
                    }

                    select = select.Replace("'", "''");
                    where = where.Replace("'", "''");
                    group_by = group_by.Replace("'", "''");
                    order_by = order_by.Replace("'", "''");

                    string query_sql = "UPDATE " + schemapath + "dashboard_query_mst SET function_name='" + name + "',type='" + type + "',query_script='" + select + "',query_where_cond='" + where + "',query_group_by='" + group_by + "',query_order_by='" + order_by + "', dbconnection_id=" + connection_id + ", query_user_id=" + query_user_id + "modified_by=" + user_id + ", modified_on='" + created_on + "' WHERE query_id = " + id + " ";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(query_sql);

                    return RedirectToAction("Index", "Dynamic", new { @menu_name = "Query" });
                }

                catch (Exception ex)
                {
                    return Content("ERROR;" + ex.Message);
                }

            }

            return View();
        }

        /// <summary>
        /// This method is called when we want to active or disable the query using id and is_deleted parameter
        /// </summary>
        /// <param name="id"></param>
        /// This parameter hold the query id
        /// <param name="is_deleted"></param>
        /// This parameter hold the is_deleted column value
        /// <returns></returns>
        public ActionResult DeleteQuery(int id, int is_deleted)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                if (is_deleted == 0)
                {
                    is_deleted = 1;
                }

                bool status;
                string sql = "UPDATE " + schemapath + "dashboard_query_mst Set is_deleted=" + is_deleted + " WHERE query_id = " + id + "";
                status = ClassFolder.Database_Class.ExecuteQuery(sql);
                //return RedirectToAction("Index", "Dashboard");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Query" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is called when we want to edit the connection using specific connection id
        /// then that connection detail related to that id is show on popup modal
        /// </summary>
        /// <param name="conn_id"></param>
        ///  This parameter hold the connection id
        /// <returns></returns>
        public ActionResult EditQueryConnData(int conn_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Connection_Class conclass = new Connection_Class();
                Connection_Class connItems = conclass.getConnectionData(schemapath, conn_id);

                return PartialView("_partialEditQueryConnectionEditView", connItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called after the submit button in connection popup modal to update the connection data values
        /// </summary>
        /// <param name="conData"></param>
        /// Basically this parameter is an instance of Connection_Class class that hold the connection related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQueryConnectionData(Connection_Class conData)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                bool connStatus;
                connStatus = getConnectionStatus(conData);
                return RedirectToAction("AddNewQuery", "Dashboard");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        
        /// <summary>
        /// Afetr updating all the values of connection data then we call this method
        /// </summary>
        /// <param name="cs"></param>
        /// This parameter hold all the updated values of Connection_Class class 
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConnectionSave(Connection_Class cs)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                //string connectionString = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
                using (NpgsqlConnection con = new NpgsqlConnection(constring))
                {
                    NpgsqlCommand cmd;
                    con.Open();
                    int db_type = 1;
                    int is_deleted = 0;
                    //string created_on = DateTime.Now.ToString();
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int connection_timeout = 5000;
                    int query_timeout = 120;

                    string result = ClassFolder.Database_Class.CheckODBC(cs.dsn_name, cs.user_id, cs.password, cs.db_schema_name);
                    if (result == "true")
                    {
                        //string conn_SelectSql = "SELECT * FROM " + schemapath + "dbconnection_mst WHERE dsn_name='" + cs.dsn_name + "' AND user_id='" + cs.user_id + "' AND password='" + cs.password + "' AND db_schemaname='" + cs.db_schema_name + "'";
                        string conn_SelectSql = ClassFolder.Query_Class.sql_ConnSelectQuery(schemapath, cs.dsn_name, cs.user_id, cs.password, cs.db_schema_name);
                        DataTable dt = ClassFolder.Database_Class.OpenQuery(conn_SelectSql);

                        if (dt.Rows.Count == 0)
                        {
                            //string conn_InsertSql = "Insert into " + schemapath + "dbconnection_mst(connection_name, db_type, driver_type, dsn_name, user_id, password, db_schemaname, connection_timeout, query_timeout, created_on, is_deleted) Values('" + cs.dsn_name + "'," + db_type + "," + db_type + ",'" + cs.dsn_name + "','" + cs.user_id + "','" + cs.password + "','" + cs.db_schema_name + "'," + connection_timeout + "," + query_timeout + ",'" + created_on + "'," + is_deleted + ") RETURNING id;";
                            string conn_InsertSql = ClassFolder.Query_Class.sql_ConnInsertQuery(schemapath, cs.dsn_name, db_type, cs.user_id, cs.password, cs.db_schema_name, connection_timeout, query_timeout, created_on, is_deleted);
                            cmd = new NpgsqlCommand(conn_InsertSql, con);
                            int connection_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                            con.Close();
                            return RedirectToAction("AddNewQuery", "DashBoard");
                        }
                        else
                        {
                            return Content("ERROR;Connection Entries already exists!!!");
                        }
                    }
                    else
                    {
                        con.Close();
                        return Content("ERROR;Please check connection entries!!!");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        ///  This method is used to edit connection table data
        /// </summary>
        /// <param name="conn_id"></param>
        /// holds connection id
        /// <param name="query_id"></param>
        /// holds query id
        /// <returns></returns>
        public ActionResult EditConnectionValue(int conn_id, int query_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Connection_Class conclass = new Connection_Class();
                Connection_Class connItems = conclass.getConnectionData(schemapath, conn_id);
                TempData["queryIdValue"] = query_id;
                return PartialView("_partialEditQueryConnectionView", connItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to edit connection table data
        /// </summary>
        /// <param name="conData"></param>
        /// Using instance of Connection_Class, it hold all updated values
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditConnectionEntry(Connection_Class conData)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                bool status;
                int connection_id;
                int id = (int)TempData["queryIdValue"];
                string dsn, user_id, password, database_name, conn_sql;

                connection_id = conData.connection_id;
                dsn = conData.dsn_name.Trim();
                user_id = conData.user_id.Trim();
                password = conData.password.Trim();
                database_name = conData.db_schema_name.Trim();

                conn_sql = "UPDATE " + schemapath + "dbconnection_mst Set connection_name='" + dsn + "', dsn_name='" + dsn + "',user_id='" + user_id + "',password='" + password + "',db_schemaname='" + database_name + "' WHERE id = " + connection_id + "";
                status = ClassFolder.Database_Class.ExecuteQuery(conn_sql);

                return RedirectToAction("EditQueryData", "Dashboard", new { queryId = id });

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to edit connection table data
        /// </summary>
        /// <param name="conData"></param>
        /// Using instance of Connection_Class, it hold all updated values
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditConnectionData(Connection_Class conData)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                bool connStatus;
                connStatus = getConnectionStatus(conData);
                return RedirectToAction("NewQueryData", "Dashboard");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Not in use
        /// </summary>
        /// <returns></returns>
        public ActionResult DynamicRow()
        {
            return View();
        }
        /// <summary>
        /// Not in use
        /// </summary>
        /// <returns></returns>
        public ActionResult StickyIndex1()
        {
            try
            {
                List<GetQueriesData> getQueries = GetScriptData.Getscript().ToList();
                Dashboard_kpi_class db = new Dashboard_kpi_class();
                List<Dashboard_kpi_class> kpiItems = db.getAllkpiItems(schemapath);
                Dashboard_kpi_class menuItems = db.getAllkpiItems1(schemapath);
                ViewBag.kpiItems = kpiItems;
                ViewBag.FontName = Font_name;
                ViewBag.FontSize = Font_size;
                return View("StickyIndex", menuItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}