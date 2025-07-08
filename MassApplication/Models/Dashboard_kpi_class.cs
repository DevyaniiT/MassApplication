using MassApplication.Models.Graph;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MassApplication.Models
{
    public class Dashboard_kpi_class
    {
        public int kpi_id { get; set; }
        public int kpi_query_id { get; set; }
        public int chart_type_id { get; set; }
        public string kpi_name { get; set; }
        public string kpi_description { get; set; }
        public string chart_type { get; set; }
        public string chart_params { get; set; }
        public string chart_bounds { get; set; }
        public string chart_position { get; set; }
        public bool is_deleted { get; set; }
        public string json_tuple { get; set; }
        public string chart_image { get; set; }
        public string chart_container { get; set; }
        public string chart_top { get; set; }
        public string chart_left { get; set; }
        public string chart_bottom { get; set; }
        public string chart_right { get; set; }
        public string chart_width { get; set; }
        public string chart_height { get; set; }
        public string chart_caption { get; set; }
        public string chart_color { get; set; }
        public string json_property { get; set; }
        public string kpi_property { get; set; }
        public string query_type { get; set; }
        public string kpi_width { get; set; }
        public string kpi_height { get; set; }
        public string kpi_x { get; set; }
        public string kpi_y { get; set; }

        /// <summary>
        /// Returns the list of all kpi's along with dashboard_query_mst and dashboard_chart_mst table data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public List<Dashboard_kpi_class> getAllkpiItems(string schema)
        {
            try
            {
                List<Dashboard_kpi_class> mydata = new List<Dashboard_kpi_class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                //string query_sql = ClassFolder.Query_Class.sql_DashboardKpiQuery(schema);
                string query_sql = ClassFolder.Query_Class.sql_RoleWiseDashboardKpi(schema, int.Parse(ClassFolder.Login_Class.role_id));
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                int ncount = 0;
                while (rdr.Read())
                {
                    ncount = ncount + 1;

                    Dashboard_kpi_class kpi_class_data = new Dashboard_kpi_class();
                    kpi_class_data.kpi_id = Convert.ToInt32(rdr["kpi_id"]);
                    kpi_class_data.kpi_query_id = Convert.ToInt32(rdr["query_id"]);
                    kpi_class_data.chart_type_id = Convert.ToInt32(rdr["chart_type_id"]);
                    kpi_class_data.kpi_name = rdr["kpi_name"].ToString();
                    kpi_class_data.chart_type = rdr["chart_type"].ToString();
                    kpi_class_data.chart_params = rdr["chart_params"].ToString();
                    kpi_class_data.chart_bounds = rdr["chart_bounds"].ToString();
                    kpi_class_data.chart_position = rdr["chart_position"].ToString();
                    kpi_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    kpi_class_data.chart_image = rdr["icon_path"].ToString();
                    kpi_class_data.json_property = rdr["chart_property"].ToString();
                    kpi_class_data.query_type = rdr["type"].ToString();

                    //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(kpi_class_data.kpi_query_id);
                    //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    DataTable resultdtt = ChartQueryData(schema, Convert.ToInt32(kpi_class_data.kpi_query_id), kpi_class_data.query_type);
                    string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
                    kpi_class_data.json_tuple = convertJson;
                    mydata.Add(kpi_class_data);
                }
                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Returns the list of all kpi class data 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public List<Dashboard_kpi_class> getAllRoleWiseKPIItems(string schema)
        {
            try
            {
                List<Dashboard_kpi_class> mydata = new List<Dashboard_kpi_class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_kpiDataOnly(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Dashboard_kpi_class kpi_class_data = new Dashboard_kpi_class();
                    kpi_class_data.kpi_id = int.Parse(rdr["kpi_id"].ToString());
                    kpi_class_data.kpi_name = rdr["kpi_name"].ToString();
                    mydata.Add(kpi_class_data);
                }

                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Returns the kpi data along with dashboard_query_mst and dashboard_chart_mst table data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of kpi
        /// <returns></returns>
        public Dashboard_kpi_class getAllkpiIDItems(string schema, int id)
        {
            try
            {
                Dashboard_kpi_class mydata = new Dashboard_kpi_class();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_DashboardKpiByQuery(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Dashboard_kpi_class kpi_class_data = new Dashboard_kpi_class();
                    mydata.kpi_id = Convert.ToInt32(rdr["kpi_id"]);
                    mydata.kpi_query_id = Convert.ToInt32(rdr["query_id"]);
                    mydata.chart_type_id = Convert.ToInt32(rdr["chart_type_id"]);
                    mydata.kpi_name = rdr["kpi_name"].ToString();
                    mydata.chart_type = rdr["chart_type"].ToString();
                    mydata.chart_params = rdr["chart_params"].ToString();
                    mydata.chart_bounds = rdr["chart_bounds"].ToString();
                    mydata.chart_position = rdr["chart_position"].ToString();
                    mydata.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    mydata.chart_image = rdr["chart_image"].ToString();

                    //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(mydata.kpi_query_id);
                    //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    DataTable resultdtt = ChartQueryData(schema, Convert.ToInt32(mydata.kpi_query_id));
                    //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
                    string convertJson = JsonConvert.SerializeObject(resultdtt);

                    mydata.json_tuple = convertJson;

                    //mydata.Add(kpi_class_data);
                }
                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable ChartQueryData(string schema, int getquery_Id, string query_type = "")
        {
            try
            {
                DataTable dtt = new DataTable();
                int asset_id = int.Parse(ConfigurationManager.AppSettings["DefaultAssetId"]);
                int stream_no = int.Parse(ConfigurationManager.AppSettings["DefaultStreamNo"]);
                int gcID = int.Parse(ConfigurationManager.AppSettings["DefaultGCID"]);
                string start_date = DateTime.Now.ToString("yyyy-MM-dd");
                string end_date = start_date;
                string cname = null;
                dtt = GetDataTableData(schema, getquery_Id, asset_id, stream_no, gcID, start_date, end_date, cname);
                return dtt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static DataTable ChartQuery_tabularData(string schema, int getquery_Id, string cname)
        {
            try
            {
                DataTable dtt = new DataTable();
                int asset_id = int.Parse(ConfigurationManager.AppSettings["DefaultAssetId"]);
                int stream_no = int.Parse(ConfigurationManager.AppSettings["DefaultStreamNo"]);
                int gcID = int.Parse(ConfigurationManager.AppSettings["DefaultGCID"]);
                string start_date = DateTime.Now.ToString("yyyy-MM-dd");
                string end_date = start_date;
                dtt = GetDataTableData(schema, getquery_Id, asset_id, stream_no, gcID, start_date, end_date, cname);
                return dtt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static DataTable ChartQuery_labelData(string schema, int getquery_Id, string cname)
        {
            try
            {
                DataTable dtt = new DataTable();
                int asset_id = int.Parse(ConfigurationManager.AppSettings["DefaultAssetId"]);
                int stream_no = int.Parse(ConfigurationManager.AppSettings["DefaultStreamNo"]);
                int gcID = int.Parse(ConfigurationManager.AppSettings["DefaultGCID"]);
                string start_date = DateTime.Now.ToString("yyyy-MM-dd");
                string end_date = start_date;
                dtt = GetDataTableData(schema, getquery_Id, asset_id, stream_no, gcID, start_date, end_date, cname);
                return dtt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        
        public static DataTable GetDataTableData(string schema, int getquery_Id, int asset_id, int stream_no, int gc_id, string start_date, string end_date, string tabString)
        {
            try
            {
                DataTable dtt = new DataTable();
                string sql = "";
                string databaseName = "";
                string password = "";
                string userName = "";
                string odbcName = "";

                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_qryData(schema, getquery_Id);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    Dashboard_kpi_class gd = new Dashboard_kpi_class();
                    //gd.json_tuple = rd["function_name"].ToString();
                    gd.json_tuple = rd["Query_scr"].ToString();
                    odbcName = rd["dsn_name"].ToString();
                    userName = rd["user_id"].ToString();
                    password = rd["password"].ToString();
                    databaseName = rd["db_schemaname"].ToString();
                    sql = gd.json_tuple;
                }

                rd.Dispose();
                con.Close();

                string current_date = DateTime.Now.ToString("yyyy-MM-dd");
                string last_month_date = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");

                sql = sql.Replace("[varDate]", start_date);
                sql = sql.Replace("[varAsset_id]", asset_id.ToString());
                sql = sql.Replace("[varStream_No]", stream_no.ToString());
                sql = sql.Replace("[varLastMonth]", last_month_date);
                sql = sql.Replace("[varGc_id]", gc_id.ToString());

                if (tabString != null)
                {
                    int pos = 0;
                    string firstString = sql.ToString();
                    string select = "";
                    string remainingString = "";
                    List<string> paramList = new List<string>();
                    List<string> paramList2 = new List<string>();
                    pos = firstString.LastIndexOf("from");
                    if (pos > 0)
                    {
                        select = firstString.Substring(7, pos - 7);
                        remainingString = firstString.Substring(pos);
                    }
                    string[] selectVal = new string[] { };
                    if (select.Trim() != "*")
                    {
                        MatchCollection matchCollection = Regex.Matches(select, @"([^,\(\\]+(\(.*?\))*)+");
                        //MatchCollection matchCollection = Regex.Matches(select, @"([^,\(\\)]+(\(.*?\))*)+");
                        foreach (Match match in matchCollection)
                        {
                            paramList.Add(match.Value.ToString());
                        }
                        paramList2 = tabString.Split(',').ToList<string>();
                        List<string> newList = new List<string>();
                        for (int i = 0; i < paramList2.Count; i++)
                        {
                            string[] paramName = paramList2[i].Split(new string[] { " as " }, StringSplitOptions.RemoveEmptyEntries);
                            string param_name = paramName[0].Trim();
                            string param_label_name = paramName[1];

                            for (int j = 0; j < paramList.Count; j++)
                            {
                                string newPram = paramList[j].ToString();
                                if (newPram.Contains(param_name.ToString()) == true)
                                {
                                    string[] newparamName = newPram.Split(new string[] { " as ", " AS " }, StringSplitOptions.RemoveEmptyEntries);
                                    string newparam_name = newparamName[0].Trim();
                                    string result = newparam_name + " as " + param_label_name;
                                    newList.Add(result);
                                }
                            }
                        }
                        string newSql = string.Join(",", newList);
                        sql = "SELECT" + " " + newSql + " " + remainingString;
                    }
                    else
                    {
                        sql = sql.Replace("*", tabString);
                    }
                }

                dtt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbcName, userName, password, databaseName);
                return dtt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public static DataTable ChartDynamicQueryData(string schema, int getquery_Id, int asset_id, int stream_no, string start_date, string end_date, string query_type = "")
        //{
        //    try
        //    {
        //        DataTable dtt = new DataTable();
        //        string cname = null;
        //        int gcID = 0;
        //        dtt = GetDataTableData(schema, getquery_Id, asset_id, stream_no, gcID, start_date, end_date, cname);
        //        return dtt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}
        //public static DataTable ChartDynamicGCQueryData(string schema, int getquery_Id, int gc_id, string start_date, string end_date, string query_type = "")
        //{
        //    try
        //    {
        //        DataTable dtt = new DataTable();
        //        string cname = null;
        //        int stream_no = 0;
        //        dtt = GetDataTableData(schema, getquery_Id, gc_id, stream_no, gc_id, start_date, end_date, cname);
        //        return dtt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public Dashboard_kpi_class getAllkpiItems1(string schema)
        {
            try
            {
                Dashboard_kpi_class mydata = new Dashboard_kpi_class();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_DashboardKpiQuery(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Dashboard_kpi_class kpi_class_data = new Dashboard_kpi_class();
                    mydata.kpi_id = Convert.ToInt32(rdr["kpi_id"]);
                    mydata.kpi_query_id = Convert.ToInt32(rdr["query_id"]);
                    mydata.chart_type_id = Convert.ToInt32(rdr["chart_type_id"]);
                    mydata.kpi_name = rdr["kpi_name"].ToString();
                    mydata.chart_type = rdr["chart_type"].ToString();
                    mydata.chart_params = rdr["chart_params"].ToString();
                    mydata.chart_bounds = rdr["chart_bounds"].ToString();
                    mydata.chart_position = rdr["chart_position"].ToString();
                    mydata.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    mydata.chart_image = rdr["icon_path"].ToString();
                    mydata.json_property = rdr["chart_property"].ToString();
                    mydata.query_type = rdr["type"].ToString();

                    //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(kpi_class_data.kpi_query_id);
                    //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    DataTable resultdtt = ChartQueryData(schema, Convert.ToInt32(mydata.kpi_query_id), mydata.query_type);
                    string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);
                    mydata.json_tuple = convertJson;
                }
                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}