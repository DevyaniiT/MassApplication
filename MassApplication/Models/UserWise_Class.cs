using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Models
{
    public class UserWise_Class
    {
        public int userwiseId { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_email_id { get; set; }
        public string user_password { get; set; }
        public string user_confirm_password { get; set; }
        public string user_type { get; set; }
        public string user_region { get; set; }
        public string[] userwiseregion { get; set; }
        public string user_network { get; set; }
        public int user_role_id { get; set; }
        public string user_role_name { get; set; }
        public List<SelectListItem> user_role { get; set; }
        public List<SelectListItem> user_db { get; set; }
        public bool is_deleted { get; set; }
        public int is_delete { get; set; }
        public bool is_active { get; set; }
        public string user_status { get; set; }
        public string user_dashboard { get; set; }
        public int user_dashboard_id { get; set; }

        /// <summary>
        /// Returns the User Wise data mapping with role and dashboard table 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the User id
        /// <returns></returns>
        public UserWise_Class getUserData(string schema, int id)
        {
            try
            {
                UserWise_Class mydata = new UserWise_Class();
                string sql_query = ClassFolder.Query_Class.sql_editUserQuery(schema, id);
                DataTable dt_qry = ClassFolder.Database_Class.OpenQuery(sql_query);
                foreach (DataRow dr in dt_qry.Rows)
                {
                    mydata.userwiseId = Convert.ToInt32(dr["id"]);
                    mydata.first_name = dr["first_name"].ToString();
                    mydata.last_name = dr["last_name"].ToString();
                    mydata.user_email_id = dr["email_id"].ToString();
                    mydata.user_password = dr["password"].ToString();
                    mydata.user_region = dr["region"].ToString();
                    //mydata.user_network = ClassFolder.GetValue.GetUserPipelinename(dr["region"].ToString());
                    mydata.userwiseregion = ClassFolder.GetValue.GetUserPipelineNames(dr["region"].ToString());
                    mydata.user_type = dr["user_type"].ToString();
                    mydata.user_role_id = Convert.ToInt32(dr["role_id"]);
                    mydata.user_role_name = dr["role_name"].ToString();
                    mydata.user_dashboard = dr["dashboard_name"].ToString();
                    mydata.user_dashboard_id = Convert.ToInt32(dr["dashboard_id"]);
                    mydata.user_role = getRoleName(schema, Convert.ToInt32(dr["role_id"]));
                    mydata.user_db = getDashboardName(schema, Convert.ToInt32(dr["dashboard_id"]));
                    //mydata.is_deleted = Convert.ToBoolean(dr["is_deleted"]);
                    mydata.is_delete = Convert.ToInt32(dr["is_deleted"]);
                    mydata.is_active = Convert.ToBoolean(dr["is_active"]);
                    //mydata.user_status = dr["user_status"].ToString();
                }
                dt_qry.Dispose();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the List of SelectListItem of role name with its id
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the role id 
        /// <returns></returns>
        public static List<SelectListItem> getRoleName(string schema, int id)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query = ClassFolder.Query_Class.sql_RoleName(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["role_id"].ToString(),
                        Value = sdr["role_name"].ToString()
                    });
                }

                sdr.Dispose();
                con.Close();
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Returns the List of SelectListItem of dashboard name with its id
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the dashboard id 
        /// <returns></returns>
        public static List<SelectListItem> getDashboardName(string schema, int id)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query = ClassFolder.Query_Class.sql_dashboardName(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                NpgsqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["dashboard_id"].ToString(),
                        Value = sdr["dashboard_name"].ToString()
                    });
                }

                sdr.Dispose();
                con.Close();
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public List<UserWise_Class> getAllUserItems(string schema)
        {
            try
            {
                List<UserWise_Class> mydata = new List<UserWise_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_UserWise_Qry(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserWise_Class user_class_data = new UserWise_Class();
                    user_class_data.userwiseId = Convert.ToInt32(rdr["id"]);
                    user_class_data.first_name = rdr["first_name"].ToString();
                    user_class_data.last_name = rdr["last_name"].ToString();
                    user_class_data.user_email_id = rdr["email_id"].ToString();
                    user_class_data.user_password = rdr["password"].ToString();
                    user_class_data.user_region = rdr["region"].ToString();
                    user_class_data.user_type = rdr["user_type"].ToString();
                    user_class_data.user_role_id = Convert.ToInt32(rdr["role_id"]);
                    //user_class_data.user_role = (SelectList)rdr["role_id"];
                    //user_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    user_class_data.is_delete = Convert.ToInt32(rdr["is_deleted"]);
                    user_class_data.is_active = Convert.ToBoolean(rdr["is_active"]);
                    user_class_data.user_status = rdr["user_status"].ToString();
                    mydata.Add(user_class_data);
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