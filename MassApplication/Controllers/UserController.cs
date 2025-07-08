using MassApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Data;
using Newtonsoft.Json;

namespace MassApplication.Controllers
{
    public class UserController : BaseController
    {
        int UserRoleId = Convert.ToInt32(ClassFolder.Login_Class.role_id);
        
        /// <summary>
        /// This method is used to adding new user in the user_mst table
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewUser()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            // ViewBag.NetworksData = PopulateNetworks(region);
            string strdata = ClassFolder.GetValue.GetUserPipelinename(region);
            string[] pipeline_name = strdata.Split(',');
            ViewBag.NetworksData = pipeline_name;
            //ViewBag.DashboardData = getAllDashboardName(schemapath);
            ViewBag.RoleData = getAllRoleName(schemapath);
            ViewBag.UserTypeData = getAllUserTypeName(schemapath);

            return PartialView("_partialNewUserView");
        }

        /// <summary>
        /// Returns the json data for dashboard table
        /// After the selection of role type
        /// </summary>
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public JsonResult GetDashboardDropdown(int id)
        {
            string sql = ClassFolder.Query_Class.sql_DashboardDDById(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            var dashboardDDData = JsonConvert.SerializeObject(dt);
            return Json(dashboardDDData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  This method is used to insert the details of user related data 
        /// </summary>
        /// <param name="userdata"></param>
        /// holds the UserWise_Class class data
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewUserData(UserWise_Class userdata)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                using (NpgsqlConnection con = new NpgsqlConnection(constring))
                {
                    int role_id = userdata.user_role_id;
                    string firstname = userdata.first_name;
                    string lastname = userdata.last_name;
                    string emailId = userdata.user_email_id;
                    string password = userdata.user_password;
                    string cpassword = userdata.user_confirm_password;
                    string userType = userdata.user_type.ToUpper();
                    int userDashboard = Convert.ToInt32(userdata.user_dashboard);
                    string[] user_region = userdata.userwiseregion;
                    string region_result = string.Join(",", user_region);
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int created_by = 1;
                    int is_deleted = 0;
                    //string userDashboard = userdata.user_dashboard;
                    //string user_region = userdata.user_region;
                    //bool userStatus = userdata.is_deleted;
                    //int userStatus = userdata.is_delete;
                    bool userStatus = userdata.is_active;
                    int statusdata = 0;
                    if (userStatus == false)
                    {
                        statusdata = 0;
                    }
                    else
                    {
                        statusdata = 1;
                    }

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    //string role_sql = "Select role_id from " + schemapath + "user_role_mst WHERE role_name = '" + userType + "'";
                    //cmd = new NpgsqlCommand(role_sql, con);
                    //int role_id = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.Dispose();

                    string pipeline_id = ClassFolder.GetValue.GetUserPipelineId(region_result);
                    string user_sql = "Insert into " + schemapath + "user_login(login_id,first_name,last_name,email_id,password,user_type,create_on,create_by,is_active,is_deleted,region,role_id,dashboard) Values('" + emailId + "','" + firstname + "','" + lastname + "','" + emailId + "','" + password + "','" + userType + "','" + created_on + "'," + created_by + "," + statusdata + "," + is_deleted + ",'" + pipeline_id + "'," + role_id + "," + userDashboard + ") RETURNING id;";
                    cmd = new NpgsqlCommand(user_sql, con);
                    int user_id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    con.Close();
                }

                return RedirectToAction("Index", "Dynamic", new { @menu_name = "User" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.Message.ToString());
            }

        }

        /// <summary>
        /// This method is used to active and disable the user status
        /// </summary>
        /// <param name="userwiseId"></param>
        /// holds the user id column value
        /// <param name="IsActiveState"></param>
        /// holds the is_deleted column value
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUserData(string userwiseId, string IsActiveState)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string id = userwiseId;
                string is_deleted = IsActiveState;
                string sql = ClassFolder.Query_Class.UpdateUser_mst(schemapath, is_deleted, id);
                bool status = ClassFolder.Database_Class.ExecuteQuery(sql);
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "User" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is called when we want to edit the user table using specific user id
        /// then that user detail related to that id is show on popup modal
        /// </summary>
        /// <param name="userwiseId"></param>
        /// This parameter hold the user id 
        /// <returns></returns>
        public ActionResult EditUserData(int userwiseId)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                UserWise_Class rc = new UserWise_Class();
                UserWise_Class userwiseItems = rc.getUserData(schemapath, userwiseId);
                ViewBag.EditUserTypeData = getAllUserTypeName(schemapath);
                ViewBag.EditRoleData = getAllRoleName(schemapath);
                //ViewBag.EditDashboardData = getAllDashboardName(schemapath);
                ViewBag.EditDashboardData = getAllDashboardName(schemapath, userwiseItems.user_role_id);
                return PartialView("_partialEditUserView", userwiseItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is used for editing/updating the user details after submit button using its user id
        /// </summary>
        /// <param name="userdata"></param>
        /// Basically this parameter is an instance of UserWise_Class class that hold the user related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserFormData(UserWise_Class userdata)
        {
            try
            {

                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                using (NpgsqlConnection con = new NpgsqlConnection(constring))
                {
                    bool editStatus;
                    int edit_user_id = userdata.userwiseId;
                    int edit_role_id = userdata.user_role_id;
                    string edit_firstname = userdata.first_name;
                    string edit_lastname = userdata.last_name;
                    string edit_emailId = userdata.user_email_id;
                    string edit_password = userdata.user_password;
                    string edit_cpassword = userdata.user_confirm_password;
                    string edit_userType = userdata.user_type.ToUpper();
                    int edit_userDashboard = Convert.ToInt32(userdata.user_dashboard);
                    string edit_user_region1 = userdata.user_region;
                    string[] edit_user_region = userdata.userwiseregion;
                    string edit_region_result = string.Join(",", edit_user_region);
                    bool edit_userStatus = userdata.is_active;
                    int edit_statusdata = 0;

                    if (edit_userStatus == false)
                    {
                        edit_statusdata = 0;
                    }
                    else
                    {
                        edit_statusdata = 1;
                    }

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    string edit_region = ClassFolder.GetValue.GetUserPipelineId(edit_region_result);

                    //string role_sql = "Select role_id from " + schemapath + "user_role_mst WHERE role_name = '" + edit_userType + "'";
                    //cmd = new NpgsqlCommand(role_sql, con);
                    //int edit_role_id = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.Dispose();

                    string edit_user_sql = "UPDATE " + schemapath + "user_login Set login_id='" + edit_emailId + "',first_name='" + edit_firstname + "',last_name='" + edit_lastname + "',email_id='" + edit_emailId + "',password='" + edit_password + "',user_type='" + edit_userType + "',is_active=" + edit_statusdata + ",region='" + edit_region + "',role_id=" + edit_role_id + ",dashboard=" + edit_userDashboard + " WHERE id= " + edit_user_id + "";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_user_sql);

                    con.Close();
                }

                return RedirectToAction("Index", "Dynamic", new { @menu_name = "User" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is called when we click on delete button in the user grid
        /// This method is used to deleted the user by set it its is_deleted column value i.e, 0 or 1
        /// </summary>
        /// <param name="id"></param>
        /// Holds the user id column value
        /// <param name="is_deleted"></param>
        /// Holds the is_deleted column value
        /// <returns></returns>
        public ActionResult DeleteUser(int id, int is_deleted)
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
            string sql = "UPDATE " + schemapath + "user_login Set is_deleted=" + is_deleted + " WHERE id = " + id + "";
            status = ClassFolder.Database_Class.ExecuteQuery(sql);
            return RedirectToAction("Index", "Dynamic", new { @menu_name = "User" });
        }

        /// <summary>
        /// This method is called when we click on delete button in the role grid
        /// This method is used to deleted the role by set it its is_deleted column value i.e, 0 or 1
        /// </summary>
        /// <param name="id"></param>
        /// Holds the user id column value
        /// <param name="is_deleted"></param>
        /// Holds the is_deleted column value
        /// <returns></returns>
        public ActionResult DeleteRole(int id, int is_deleted)
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
            string sql = "UPDATE " + schemapath + "user_role_mst Set is_deleted=" + is_deleted + " WHERE role_id = " + id + "";
            status = ClassFolder.Database_Class.ExecuteQuery(sql);
            return RedirectToAction("Index", "Dynamic", new { @menu_name = "Role" });
        }

        /// <summary>
        /// This method is used to adding new role in the role_mst table
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewRole()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            RoleWise_Class rc = new RoleWise_Class();
            RoleWise_Class rolewiseItems = rc.getNewRoleData(schemapath);
            return PartialView("_partialNewRoleView", rolewiseItems);
        }

        /// <summary>
        /// This method is called when we want to edit the role table using specific role id
        /// then that role detail related to that id is show on popup modal
        /// </summary>
        /// <param name="rolewiseId"></param>
        /// This parameter hold the role id 
        /// <returns></returns>
        public ActionResult EditRoleData(int rolewiseId)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                RoleWise_Class rc = new RoleWise_Class();
                RoleWise_Class rolewiseItems = rc.getRoleData(schemapath, rolewiseId);

                return PartialView("_partialEditRoleView", rolewiseItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        ///  This method is used to insert the details of role related data 
        /// </summary>
        /// <param name="roledata"></param>
        /// holds the RoleWise_Class class data
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewRoleData(RoleWise_Class roledata)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                using (NpgsqlConnection con = new NpgsqlConnection(constring))
                {
                    //int role_id = roledata.role_id;
                    int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                    string role_name = roledata.role_name;
                    string role_description = roledata.role_description;
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //int created_by = 1;
                    int is_deleted = 0;
                    bool rpt_view, db_view, kpi_view, query_view, menu_view, menu_create, menu_update, menu_delete;
                    string db_sql, rpt_sql, menu_sql, kpi_sql, query_sql;
                    int dashID = 0;
                    int kpiID = 0;
                    int queryID = 0;
                    int rptID = 0;
                    int menuID = 0;

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    con.Open();

                    string role_sql = "INSERT INTO " + schemapath + "user_role_mst(role_name,role_description,created_on,created_by,modified_on,modified_by,is_deleted) Values('" + role_name + "','" + role_description + "','" + created_on + "'," + user_id + ",'" + created_on + "'," + user_id + "," + is_deleted + ") RETURNING role_id;";
                    cmd = new NpgsqlCommand(role_sql, con);
                    int roleid = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    if (roledata.menu_map_data != null)
                    {
                        List<MenuMap_Class> menuclassdata = roledata.menu_map_data.ToList();
                        var rpt_result = from s in menuclassdata
                                         select s;
                        foreach (var data in rpt_result)
                        {
                            menuID = data.menu_id;
                            menu_view = data.role_menu_read;
                            menu_create = data.role_menu_create;
                            menu_update = data.role_menu_update;
                            menu_delete = data.role_menu_delete;

                            menu_sql = "INSERT INTO " + schemapath + "menu_role_map(role_id,menu_id,allow_create,allow_read,allow_update,allow_delete) Values(" + roleid + "," + menuID + "," + menu_create + "," + menu_view + "," + menu_update + "," + menu_delete + ") Returning map_id";
                            cmd = new NpgsqlCommand(menu_sql, con);
                            int rpt_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }
                    }

                    if (roledata.dashboard_map_data != null)
                    {
                        List<DashboardMap_Class> dbclassdata = roledata.dashboard_map_data.ToList();
                        var db_result = from s in dbclassdata
                                        select s;
                        foreach (var data in db_result)
                        {
                            dashID = data.dashboard_id;
                            db_view = data.role_dashboard_read;

                            db_sql = "INSERT into " + schemapath + "dashboard_role_map(role_id,dashboard_id,allow_read) Values(" + roleid + "," + dashID + "," + db_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(db_sql, con);
                            int db_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    if (roledata.report_map_data != null)
                    {
                        List<ReportMap_Class> rptclassdata = roledata.report_map_data.ToList();
                        var rpt_result = from s in rptclassdata
                                         select s;
                        foreach (var data in rpt_result)
                        {
                            rptID = data.report_id;
                            rpt_view = data.role_report_read;

                            rpt_sql = "INSERT INTO " + schemapath + "report_role_map(role_id,report_id,allow_read) Values(" + roleid + "," + rptID + "," + rpt_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(rpt_sql, con);
                            int rpt_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_rpt_sql = "UPDATE " + schemapath + "report_role_map SET allow_create = " + rpt_create + ", allow_read=" + rpt_view + ",allow_update = " + rpt_update + ", allow_delete=" + rpt_delete + " WHERE role_id IN (" + rptID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_rpt_sql);

                        }
                    }

                    if (roledata.kpi_map_data != null)
                    {
                        List<KPIMap_Class> kpiclassdata = roledata.kpi_map_data.ToList();
                        var kpi_result = from s in kpiclassdata
                                        select s;
                        foreach (var data in kpi_result)
                        {
                            kpiID = data.kpi_id;
                            kpi_view = data.role_kpi_read;

                            kpi_sql = "INSERT into " + schemapath + "kpi_role_map(role_id,kpi_id,allow_read) Values(" + roleid + "," + kpiID + "," + kpi_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(kpi_sql, con);
                            int kpi_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    if (roledata.query_map_data != null)
                    {
                        List<QueryMap_Class> queryclassdata = roledata.query_map_data.ToList();
                        var query_result = from s in queryclassdata
                                         select s;
                        foreach (var data in query_result)
                        {
                            queryID = data.query_id;
                            query_view = data.role_query_read;

                            query_sql = "INSERT into " + schemapath + "query_role_map(role_id,dashboard_query_id,allow_read) Values(" + roleid + "," + queryID + "," + query_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(query_sql, con);
                            int query_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    con.Close();
                }

                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Role" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.Message.ToString());
            }

        }

        /// <summary>
        /// This method is used for editing/updating the role details after submit button using its role id 
        /// </summary>
        /// <param name="roledata"></param>
        ///  Basically this parameter is an instance of RoleWise_Class class that hold the role related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRoleFormData(RoleWise_Class roledata)
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
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    bool editStatus;
                    int edit_role_id = roledata.role_id;
                    string edit_role_name = roledata.role_name;
                    string edit_role_description = roledata.role_description;

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    con.Open();
                    bool rpt_view, db_view, kpi_view, query_view, menu_view, menu_create, menu_update, menu_delete;
                    string edit_db_sql, edit_rpt_sql, edit_menu_sql, edit_kpi_sql, edit_query_sql;
                    int dashID = 0;
                    int rptID = 0;
                    int menuID = 0;
                    int kpiID = 0;
                    int queryID = 0;

                    string menumapDelete = "DELETE FROM " + schemapath + "menu_role_map WHERE role_id IN(" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(menumapDelete);
                    
                    string dbmapDelete = "DELETE FROM " + schemapath + "dashboard_role_map WHERE role_id IN(" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(dbmapDelete);

                    string rptmapDelete = "DELETE FROM " + schemapath + "report_role_map WHERE role_id IN(" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(rptmapDelete);

                    string kpimapDelete = "DELETE FROM " + schemapath + "kpi_role_map WHERE role_id IN(" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(kpimapDelete);

                    string querymapDelete = "DELETE FROM " + schemapath + "query_role_map WHERE role_id IN(" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(querymapDelete);

                    if (roledata.dashboard_map_data != null)
                    {
                        List<DashboardMap_Class> dbclassdata = roledata.dashboard_map_data.ToList();
                        var db_result = from s in dbclassdata
                                        select s;
                        foreach (var data in db_result)
                        {
                            dashID = data.dashboard_id;
                            db_view = data.role_dashboard_read;

                            edit_db_sql = "INSERT into " + schemapath + "dashboard_role_map(role_id,dashboard_id,allow_read) Values(" + edit_role_id + "," + dashID + "," + db_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(edit_db_sql, con);
                            int edit_db_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    if (roledata.report_map_data != null)
                    {
                        List<ReportMap_Class> rptclassdata = roledata.report_map_data.ToList();
                        var rpt_result = from s in rptclassdata
                                         select s;
                        foreach (var data in rpt_result)
                        {
                            rptID = data.report_id;
                            rpt_view = data.role_report_read;

                            edit_rpt_sql = "INSERT INTO " + schemapath + "report_role_map(role_id,report_id,allow_read) Values(" + edit_role_id + "," + rptID + "," + rpt_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(edit_rpt_sql, con);
                            int edit_rpt_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_rpt_sql = "UPDATE " + schemapath + "report_role_map SET allow_create = " + rpt_create + ", allow_read=" + rpt_view + ",allow_update = " + rpt_update + ", allow_delete=" + rpt_delete + " WHERE role_id IN (" + rptID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_rpt_sql);

                        }
                    }

                    if (roledata.menu_map_data != null)
                    {
                        List<MenuMap_Class> menuclassdata = roledata.menu_map_data.ToList();
                        var menu_result = from s in menuclassdata
                                         select s;
                        foreach (var data in menu_result)
                        {
                            menuID = data.menu_id;
                            menu_view = data.role_menu_read;
                            menu_create = data.role_menu_create;
                            menu_update = data.role_menu_update;
                            menu_delete = data.role_menu_delete;

                            edit_menu_sql = "INSERT INTO " + schemapath + "menu_role_map(role_id,menu_id,allow_create,allow_read,allow_update,allow_delete) Values(" + edit_role_id + "," + menuID + "," + menu_create + "," + menu_view + "," + menu_update + "," + menu_delete + ") Returning map_id";
                            cmd = new NpgsqlCommand(edit_menu_sql, con);
                            int edit_menu_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_rpt_sql = "UPDATE " + schemapath + "report_role_map SET allow_create = " + rpt_create + ", allow_read=" + rpt_view + ",allow_update = " + rpt_update + ", allow_delete=" + rpt_delete + " WHERE role_id IN (" + rptID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_rpt_sql);

                        }
                    }

                    if (roledata.kpi_map_data != null)
                    {
                        List<KPIMap_Class> kpiclassdata = roledata.kpi_map_data.ToList();
                        var kpi_result = from s in kpiclassdata
                                         select s;
                        foreach (var data in kpi_result)
                        {
                            kpiID = data.kpi_id;
                            kpi_view = data.role_kpi_read;

                            edit_kpi_sql = "INSERT into " + schemapath + "kpi_role_map(role_id,kpi_id,allow_read) Values(" + edit_role_id + "," + kpiID + "," + kpi_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(edit_kpi_sql, con);
                            int edit_kpi_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    if (roledata.query_map_data != null)
                    {
                        List<QueryMap_Class> queryclassdata = roledata.query_map_data.ToList();
                        var query_result = from s in queryclassdata
                                           select s;
                        foreach (var data in query_result)
                        {
                            queryID = data.query_id;
                            query_view = data.role_query_read;

                            edit_query_sql = "INSERT into " + schemapath + "query_role_map(role_id,dashboard_query_id,allow_read) Values(" + edit_role_id + "," + queryID + "," + query_view + ") Returning map_id";
                            cmd = new NpgsqlCommand(edit_query_sql, con);
                            int edit_query_map_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            //string edit_db_sql = "UPDATE " + schemapath + "dashboard_role_map SET allow_create = " + db_create + ", allow_read=" + db_view + ",allow_update = " + db_update + ", allow_delete=" + db_delete + " WHERE role_id IN (" + dashID + ")";
                            //editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_db_sql);

                        }
                    }

                    string edit_role_sql = "UPDATE " + schemapath + "user_role_mst SET role_name = '" + edit_role_name + "', role_description='" + edit_role_description + "', modified_by=" + user_id + ", modified_on='" + created_on + "' WHERE role_id IN (" + edit_role_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(edit_role_sql);

                    con.Close();
                }

                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Role" });
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }
    }
}