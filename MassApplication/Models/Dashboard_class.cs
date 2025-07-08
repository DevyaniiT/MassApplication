using MassApplication.Models.Graph;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Dashboard_class
    {
        public int dashboard_id { get; set; }
        public string dashboard_name { get; set; }
        public string dashboard_description { get; set; }
        public string dashboard_position { get; set; }
        public string dashboard_chart_type { get; set; }
        public string dashboard_chart_parameters { get; set; }
        public bool is_deleted { get; set; }
        public string dashboard_status { get; set; }
        public bool defaultDashboard { get; set; }
        public int dashboard_user_id { get; set; }
        public List<Dashboard_kpi_class> kpiList { get; set; }

        /// <summary>
        /// Returns the list of dashboard class data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public List<Dashboard_class> getAllDashboardItems(string schema, int roleId)
        {
            try
            {
                List<Dashboard_class> mydata = new List<Dashboard_class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);
              
                con.Open();
                //string query_sql = ClassFolder.Query_Class.sql_dashboardDataOnly(schema);
                string query_sql = ClassFolder.Query_Class.sql_DashboardById(schema, roleId);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Dashboard_class db_class_data = new Dashboard_class();
                    db_class_data.dashboard_id = int.Parse(rdr["dashboard_id"].ToString());
                    db_class_data.dashboard_name = rdr["dashboard_name"].ToString();
                    db_class_data.dashboard_description = rdr["dashboard_description"].ToString();
                    db_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    //db_class_data.dashboard_status = rdr["dashboard_status"].ToString();
                    mydata.Add(db_class_data);
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
        /// Returns the dashboard class data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <param name="id"></param>
        /// Holds the id of dashboard
        /// <returns></returns>
        public Dashboard_class getDashboardItemsDemo(string schema, int id)
        {
            try
            {
                Dashboard_class mydata = new Dashboard_class();
                List<Dashboard_kpi_class> mykpidata = new List<Dashboard_kpi_class>();
                string query_sql = ClassFolder.Query_Class.sql_dashboardIdData(schema, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(query_sql);

                foreach (DataRow dr in dt.Rows)
                {
                    //DataTable resultdtt = GetScriptData.ConvertinJsonDatatblODBC(Convert.ToInt32(dr["query_id"]));
                    //string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    DataTable resultdtt = Dashboard_kpi_class.ChartQueryData(schema, Convert.ToInt32(dr["query_id"]));
                    string convertJson = JsonConvert.SerializeObject(resultdtt);

                    mykpidata.Add(new Dashboard_kpi_class
                    {
                        kpi_id = Convert.ToInt32(dr["kpi_id"]),
                        kpi_query_id = Convert.ToInt32(dr["query_id"]),
                        chart_type_id = Convert.ToInt32(dr["chart_type_id"]),
                        kpi_name = dr["kpi_name"].ToString(),
                        chart_type = dr["chart_type"].ToString(),
                        chart_params = dr["chart_params"].ToString(),
                        chart_bounds = dr["chart_bounds"].ToString(),
                        kpi_property = dr["kpi_property"].ToString(),
                        chart_position = dr["kpi_position"].ToString(),
                        chart_image = dr["chart_image"].ToString(),
                        json_tuple = convertJson
                    });

                    mydata.dashboard_id = Convert.ToInt32(dr["dashboard_id"]);
                    mydata.dashboard_name = dr["dashboard_name"].ToString();
                    mydata.dashboard_description = dr["dashboard_description"].ToString();
                    mydata.dashboard_position = dr["dashboard_position"].ToString();
                    mydata.dashboard_chart_type = dr["chart_type"].ToString();
                    mydata.dashboard_chart_parameters = dr["chart_params"].ToString();
                    mydata.dashboard_user_id = int.Parse(dr["dashboard_user_id"].ToString());
                    mydata.is_deleted = Convert.ToBoolean(dr["is_deleted"]);
                    mydata.kpiList = mykpidata;
                }
                dt.Dispose();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        /// <summary>
        /// Returns the list of dashboard class data 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public List<Dashboard_class> getAllRoleWiseDashboardItems(string schema, int id)
        {
            try
            {
                List<Dashboard_class> mydata = new List<Dashboard_class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_RoleWiseDashboardData(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Dashboard_class db_class_data = new Dashboard_class();
                    db_class_data.dashboard_id = int.Parse(rdr["dashboard_id"].ToString());
                    db_class_data.dashboard_name = rdr["dashboard_name"].ToString();
                    db_class_data.dashboard_description = rdr["dashboard_description"].ToString();
                    //db_class_data.dashboard_position = rdr["chart_position"].ToString();
                    //db_class_data.dashboard_chart_type = rdr["chart_type"].ToString();
                    //db_class_data.dashboard_chart_parameters = rdr["chart_params"].ToString();
                    db_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    db_class_data.dashboard_status = rdr["dashboard_status"].ToString();
                    //db_class_data.dashboard_user_id = int.Parse(rdr["dashboard_user_id"].ToString());
                    mydata.Add(db_class_data);
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