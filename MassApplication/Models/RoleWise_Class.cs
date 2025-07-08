using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class RoleWise_Class
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string role_description { get; set; }
        public int is_delete { get; set; }
        public bool is_deleted { get; set; }
        public List<ReportMap_Class> report_map_data { get; set; }
        public List<MenuMap_Class> menu_map_data { get; set; }
        public List<DashboardMap_Class> dashboard_map_data { get; set; }
        public List<KPIMap_Class> kpi_map_data { get; set; }
        public List<QueryMap_Class> query_map_data { get; set; }

        /// <summary>
        /// Returns the role wise data with its list of menu items, kpi items, dashboard items and query items 
        /// Used for displaying and editing data 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of role
        /// <returns></returns>
        public RoleWise_Class getRoleData(string schema, int id)
        {
            try
            {
                RoleWise_Class mydata = new RoleWise_Class();
                List<DashboardMap_Class> mydbmapdata = new List<DashboardMap_Class>();
                List<KPIMap_Class> mykpimapdata = new List<KPIMap_Class>();
                List<QueryMap_Class> myquerymapdata = new List<QueryMap_Class>();
                List<ReportMap_Class> myrptmapdata = new List<ReportMap_Class>();
                List<MenuMap_Class> mymenumapdata = new List<MenuMap_Class>();

                string sql_menu = ClassFolder.Query_Class.sql_editRoleMenuMapQuery(schema, id);
                string sql_report = ClassFolder.Query_Class.sql_editRoleReportMapQuery(schema, id);
                string sql_dashboard = ClassFolder.Query_Class.sql_editRoleDashboardMapQuery(schema, id);
                string sql_kpi = ClassFolder.Query_Class.sql_editRoleKPIMapQuery(schema, id); 
                string sql_query = ClassFolder.Query_Class.sql_editRoleQueryMapQuery(schema, id);

                
                DataTable menu_dt = ClassFolder.Database_Class.OpenQuery(sql_menu);
                foreach (DataRow menu_dr in menu_dt.Rows)
                {
                    if ((menu_dr["allow_read"]).ToString() == "" || (menu_dr["allow_create"]).ToString() == "" || (menu_dr["allow_update"]).ToString() == "" || (menu_dr["allow_delete"]).ToString() == "")
                    {
                        menu_dr["allow_read"] = "false";
                        menu_dr["allow_create"] = "false";
                        menu_dr["allow_delete"] = "false";
                        menu_dr["allow_update"] = "false";
                    }
                    mymenumapdata.Add(new MenuMap_Class
                    {
                        menu_id = Convert.ToInt32(menu_dr["menu_id"]),
                        menu_name = menu_dr["menu_name"].ToString(),
                        menu_parent_id = menu_dr["menu_parent_id"].ToString(),
                        role_menu_read = Convert.ToBoolean(menu_dr["allow_read"]),
                        role_menu_create = Convert.ToBoolean(menu_dr["allow_create"]),
                        role_menu_delete = Convert.ToBoolean(menu_dr["allow_delete"]),
                        role_menu_update = Convert.ToBoolean(menu_dr["allow_update"])
                    });
                }
                DataTable rpt_dt = ClassFolder.Database_Class.OpenQuery(sql_report);
                foreach (DataRow rpt_dr in rpt_dt.Rows)
                {
                    if ((rpt_dr["allow_read"]).ToString() == "")
                    {
                        rpt_dr["allow_read"] = "false";
                    }
                    myrptmapdata.Add(new ReportMap_Class
                    {
                        report_id = Convert.ToInt32(rpt_dr["report_id"]),
                        report_name = rpt_dr["report_name"].ToString(),
                        report_user_type = GetUserType(int.Parse(rpt_dr["report_user_id"].ToString())),
                        report_created_by = rpt_dr["user_type"].ToString(),
                        role_report_read = Convert.ToBoolean(rpt_dr["allow_read"])
                    });
                }
                DataTable db_dt = ClassFolder.Database_Class.OpenQuery(sql_dashboard);
                foreach (DataRow db_dr in db_dt.Rows)
                {
                    if ((db_dr["allow_read"]).ToString() == "")
                    {
                        db_dr["allow_read"] = "false";
                    }
                    mydbmapdata.Add(new DashboardMap_Class
                    {
                        dashboard_id = Convert.ToInt32(db_dr["dashboard_id"]),
                        dashboard_name = db_dr["dashboard_name"].ToString(),
                        dashboard_user_type = GetUserType(int.Parse(db_dr["dashboard_user_id"].ToString())),
                        dashboard_created_by = db_dr["user_type"].ToString(),
                        role_dashboard_read = Convert.ToBoolean(db_dr["allow_read"])
                    });
                }
                DataTable kpi_dt = ClassFolder.Database_Class.OpenQuery(sql_kpi);
                foreach (DataRow kpi_dr in kpi_dt.Rows)
                {
                    if ((kpi_dr["allow_read"]).ToString() == "")
                    {
                        kpi_dr["allow_read"] = "false";
                    }
                    mykpimapdata.Add(new KPIMap_Class
                    {
                        kpi_id = Convert.ToInt32(kpi_dr["kpi_id"]),
                        kpi_name = kpi_dr["kpi_name"].ToString(),
                        kpi_user_type = GetUserType(int.Parse(kpi_dr["system_kpi"].ToString())),
                        kpi_created_by = kpi_dr["user_type"].ToString(),
                        role_kpi_read = Convert.ToBoolean(kpi_dr["allow_read"])
                    });
                }
                DataTable query_dt = ClassFolder.Database_Class.OpenQuery(sql_query);
                foreach (DataRow query_dr in query_dt.Rows)
                {
                    if ((query_dr["allow_read"]).ToString() == "")
                    {
                        query_dr["allow_read"] = "false";
                    }
                    myquerymapdata.Add(new QueryMap_Class
                    {
                        query_id = Convert.ToInt32(query_dr["query_id"]),
                        query_name = query_dr["function_name"].ToString(),
                        query_user_type = GetUserType(int.Parse(query_dr["query_user_id"].ToString())),
                        query_created_by = query_dr["user_type"].ToString(),
                        role_query_read = Convert.ToBoolean(query_dr["allow_read"])
                    });
                }

                string sql_qry = ClassFolder.Query_Class.sql_editRoleMapQuery(schema, id);
                DataTable dt_qry = ClassFolder.Database_Class.OpenQuery(sql_qry);
                foreach (DataRow dr in dt_qry.Rows)
                {
                    mydata.role_id = Convert.ToInt32(dr["role_id"]);
                    mydata.role_name = dr["role_name"].ToString();
                    mydata.role_description = dr["role_description"].ToString();
                    mydata.is_deleted = Convert.ToBoolean(dr["is_deleted"]);
                    mydata.is_delete = Convert.ToInt32(dr["is_deleted"]);
                    mydata.dashboard_map_data = mydbmapdata;
                    mydata.report_map_data = myrptmapdata;
                    mydata.menu_map_data = mymenumapdata;
                    mydata.kpi_map_data = mykpimapdata;
                    mydata.query_map_data = myquerymapdata;
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
        /// Returns the list of menu items, kpi items, dashboard items and query items 
        /// Used for adding new role in popup modal
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public RoleWise_Class getNewRoleData(string schema)
        {
            try
            {
                RoleWise_Class mydata = new RoleWise_Class();
                List<DashboardMap_Class> mydbmapdata = new List<DashboardMap_Class>();
                List<KPIMap_Class> mykpimapdata = new List<KPIMap_Class>();
                List<QueryMap_Class> myquerymapdata = new List<QueryMap_Class>();
                List<ReportMap_Class> myrptmapdata = new List<ReportMap_Class>();
                List<MenuMap_Class> mymenumapdata = new List<MenuMap_Class>();

                //string sql_menu = ClassFolder.Query_Class.sql_AllMenuItems(schema);
                //string sql_report = ClassFolder.Query_Class.sql_AllReportData(schema);
                //string sql_dashboard = ClassFolder.Query_Class.sql_dashboardDataOnly(schema);
                //string sql_kpi = ClassFolder.Query_Class.sql_kpiDataOnly(schema);
                //string sql_query = ClassFolder.Query_Class.sql_queryDataOnly(schema);

                string sql_menu = ClassFolder.Query_Class.sql_newRoleMenuMapQuery(schema);
                string sql_report = ClassFolder.Query_Class.sql_newRoleReportMapQuery(schema);
                string sql_dashboard = ClassFolder.Query_Class.sql_newRoleDashboardMapQuery(schema);
                string sql_kpi = ClassFolder.Query_Class.sql_newRoleKPIMapQuery(schema);
                string sql_query = ClassFolder.Query_Class.sql_newRoleQueryMapQuery(schema);

                DataTable menu_dt = ClassFolder.Database_Class.OpenQuery(sql_menu);
                foreach (DataRow menu_dr in menu_dt.Rows)
                {
                    mymenumapdata.Add(new MenuMap_Class
                    {
                        menu_id = Convert.ToInt32(menu_dr["menu_id"]),
                        menu_name = menu_dr["menu_name"].ToString(),
                        menu_parent_id = menu_dr["menu_parent_id"].ToString()
                    });
                }
                DataTable rpt_dt = ClassFolder.Database_Class.OpenQuery(sql_report);
                foreach (DataRow rpt_dr in rpt_dt.Rows)
                {
                    myrptmapdata.Add(new ReportMap_Class
                    {
                        report_id = Convert.ToInt32(rpt_dr["report_id"]),
                        report_name = rpt_dr["report_name"].ToString(),
                        report_user_type = GetUserType(int.Parse(rpt_dr["report_user_id"].ToString())),
                        report_created_by = rpt_dr["user_type"].ToString()
                    });
                }
                DataTable db_dt = ClassFolder.Database_Class.OpenQuery(sql_dashboard);
                foreach (DataRow db_dr in db_dt.Rows)
                {
                    mydbmapdata.Add(new DashboardMap_Class
                    {
                        dashboard_id = Convert.ToInt32(db_dr["dashboard_id"]),
                        dashboard_name = db_dr["dashboard_name"].ToString(),
                        dashboard_user_type = GetUserType(int.Parse(db_dr["dashboard_user_id"].ToString())),
                        dashboard_created_by = db_dr["user_type"].ToString()
                    });
                }
                DataTable kpi_dt = ClassFolder.Database_Class.OpenQuery(sql_kpi);
                foreach (DataRow kpi_dr in kpi_dt.Rows)
                {
                    mykpimapdata.Add(new KPIMap_Class
                    {
                        kpi_id = Convert.ToInt32(kpi_dr["kpi_id"]),
                        kpi_name = kpi_dr["kpi_name"].ToString(),
                        kpi_user_type = GetUserType(int.Parse(kpi_dr["system_kpi"].ToString())),
                        kpi_created_by = kpi_dr["user_type"].ToString()
                    });
                }
                DataTable query_dt = ClassFolder.Database_Class.OpenQuery(sql_query);
                foreach (DataRow query_dr in query_dt.Rows)
                {
                    myquerymapdata.Add(new QueryMap_Class
                    {
                        query_id = Convert.ToInt32(query_dr["query_id"]),
                        query_name = query_dr["function_name"].ToString(),
                        query_user_type = GetUserType(int.Parse(query_dr["query_user_id"].ToString())),
                        query_created_by = query_dr["user_type"].ToString()
                    });
                }

                mydata.dashboard_map_data = mydbmapdata;
                mydata.report_map_data = myrptmapdata;
                mydata.menu_map_data = mymenumapdata;
                mydata.kpi_map_data = mykpimapdata;
                mydata.query_map_data = myquerymapdata;

                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<RoleWise_Class> getAllRoleItems(string schema)
        {
            try
            {
                List<RoleWise_Class> mydata = new List<RoleWise_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_RoleWise_Qry(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    RoleWise_Class role_class_data = new RoleWise_Class();
                    role_class_data.role_id = Convert.ToInt32(rdr["role_id"]);
                    role_class_data.role_name = rdr["role_name"].ToString();
                    role_class_data.role_description = rdr["role_description"].ToString();
                    role_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    role_class_data.is_delete = Convert.ToInt32(rdr["is_deleted"]);
                    mydata.Add(role_class_data);
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
        public string GetUserType(int id) 
        {
            string userType = "";
            if (id == 1)
            {
                userType = "system kpi";
            }
            else if (id == 2)
            {
                userType = "private";
            }
            else
            {
                userType = "public";
            }
            return userType.ToUpper();
        }
    }
}