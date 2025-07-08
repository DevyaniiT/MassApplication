using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class Query_Class
    {
        public static string pSql;

        /// <summary>
        /// Returns sql query by using report id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string sql_reportData(string s, int id)
        {
            pSql = "SELECT qm.query_id,function_name,type,rm.report_name,parameters,rm.report_id,report_path,report_file, "
                    + "CONCAT(query_script,' ',query_where_cond,' ',query_group_by,' ',query_order_by) Query_script "
                    + "FROM " + s + "report_query_mst qm "
                    + "LEFT JOIN " + s + "report_query_map rqm ON rqm.query_id = qm.query_id "
                    + "LEFT JOIN " + s + "report_mst rm ON rm.report_id = rqm.report_id "
                    + "WHERE qm.is_deleted = 0 AND rm.report_id = " + id + " "
                    + "ORDER BY qm.query_id DESC";
            return pSql;
        }
        
        /// <summary>
        /// Returns sql query by using query id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the query id
        /// <returns></returns>
        public static string sql_qryData(string s, int id) 
        {
            pSql = "select dbconnection_id,query_id,function_name,type,"
                  + " concat(query_script, ' ', query_where_cond, ' ', query_group_by, ' ', query_order_by) Query_scr,"
                  + " connection_name, dsn_name, user_id, password, db_schemaname"
                  + " from " + s + "dashboard_query_mst dqm"
                  + " left join " + s + "dbconnection_mst dbm on dbm.id = dqm.dbconnection_id"
                  + " where dqm.is_deleted = 0 and dbm.is_deleted = 0 and dqm.query_id = " + id + "";
            
             return pSql;
        }

        /// <summary>
        /// Returns sql query by using pipeline id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string sql_qry(string s, string id)
        {
            pSql = "SELECT DISTINCT on (pipelinemst_id) pipelinemst_id,pipeline_name FROM " + s + "pipeline_mst WHERE is_deleted = 0 AND pipelinemst_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query by using connnection id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the connection id
        /// <returns></returns>
        public static string sql_getConnId(string s, int id)
        {
            pSql = "Select * from " + s + "dbconnection_mst where id = " + id + "";

            return pSql;
        }

        /// <summary>
        /// Returns sql query by using section id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the section id
        /// <returns></returns>
        public static string sql_Sectionqry(string s, string id)
        {
            pSql = "SELECT DISTINCT on (section_id) section_id,section_name FROM " + s + "pipeline_section WHERE section_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline_checkmeter table by using check meter id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the check meter id 
        /// <returns></returns>
        public static string sql_CheckMeterqry(string s, string id)
        {
            pSql = "SELECT DISTINCT(check_meter_name), check_meter_id FROM " + s + "pipeline_checkmeter WHERE is_deleted=0 AND check_meter_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report query map table by using report id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema
        /// <param name="id"></param>
        /// Defines the report id 
        /// <returns></returns>
        public static string sql_getNetworkId(string s, int id)
        {
            pSql = "SELECT query_id FROM " + s + "report_query_map WHERE report_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline section table by using pipeline id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema aof database
        /// <param name="network"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string sql_SectionId(string s, string network)
        {
            pSql = "SELECT DISTINCT on (section_id) section_id, section_name FROM " + s + "pipeline_section WHERE is_deleted = 0 AND pipelinemst_id IN (" + network + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline station table by using station id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the station id
        /// <returns></returns>
        public static string sql_StationId(string s, string id)
        {
            pSql = "SELECT DISTINCT(station_name),station_id FROM " + s + "pipe_station WHERE station_id IN (" + id + ") ORDER BY station_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for gc table by using its id 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the gc id
        /// <returns></returns>
        public static string sql_GCId(string s, string id)
        {
            pSql = "SELECT gc_name FROM " + s + "gc_mst WHERE gc_id IN (" + id + ") ORDER BY gc_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report query param table by using report id
        /// </summary>
        /// <param name="s"></param>
        ///  Defines the schema of database
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string sql_ComboReportParamQryUnOrder(string s, string id)
        {
            pSql = "SELECT * FROM " + s + "report_query_param WHERE is_deleted=0 AND report_id=" + id + " ORDER BY param_query,param_label ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report query param table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ComboNewReportParamQry(string s)
        {
            pSql = "SELECT * FROM " + s + "report_query_param WHERE is_deleted=0 ORDER BY param_query ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for connection table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ComboConnectionQry(string s)
        {
            pSql = "SELECT * FROM " + s + "dbconnection_mst Where is_deleted=0 ORDER BY connection_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for emailerjobs_scheduled table (older table)
        /// Currently we are using pipeline_job_master table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ComboScheduleQry(string s)
        {
            pSql = "SELECT *, CASE WHEN es.id_deleted=0 THEN 'Active' ELSE 'Disable' END AS Status FROM " + s + "emailerjobs_scheduled as es LEFT JOIN " + s + "emailerjobs_emails as ee ON es.schdr_jobid = ee.schdr_jobid Where es.id_deleted=0 ORDER BY es.job_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ComboReportQryData(string s)
        {
            pSql = "SELECT rm.report_id,rm.report_name,rm.report_description,rm.report_path,rm.report_file,rm.is_deleted,rm.report_user_id,CASE WHEN rm.is_deleted = 0 THEN 'Active' ELSE 'Disable' END AS Status FROM " + s + "report_mst as rm ORDER BY rm.report_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for connection table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the connection id
        /// <returns></returns>
        public static string sql_editConnectionQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dbconnection_mst WHERE id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for connection table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string sql_editParamReportQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "report_query_param WHERE report_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        ///  Returns sql query for report table by using its id
        ///  and mapping with report_query_map, report_query_mst, and dbconnection_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string sql_editQryReportQuery(string s, int id)
        {
            //pSql = "SELECT rm.report_id,rm.report_name,rm.report_description,rm.report_path,rm.report_file,rm.is_deleted,qm.type,qm.query_id,CONCAT(qm.query_script,' ',qm.query_where_cond,' ',qm.query_group_by,' ',qm.query_order_by) report_query FROM " + s + "report_mst as rm LEFT JOIN " + s + "report_query_map as rqm ON rqm.report_id = rm.report_id LEFT JOIN " + s + "report_query_mst as qm ON qm.query_id = rqm.query_id WHERE rm.report_id IN (" + id + ") and qm.is_deleted=0";

            pSql = "SELECT rm.report_id,rm.report_name,rm.report_path,rm.report_file,"
                 + "rm.is_deleted,rm.report_description,rm.report_user_id,qm.type,qm.query_id,qm.parameters,"
                 + "CONCAT(qm.query_script, ' ', qm.query_where_cond, ' ', qm.query_group_by, ' ', qm.query_order_by) report_query,"
                 + "cm.id,cm.dsn_name,cm.user_id,cm.password,cm.db_schemaname "
                 + "FROM " + s + "report_mst as rm "
                 + "LEFT JOIN " + s + "report_query_map as rqm ON rqm.report_id = rm.report_id "
                 + "LEFT JOIN " + s + "report_query_mst as qm ON qm.query_id = rqm.query_id "
                 + "LEFT JOIN " + s + "dbconnection_mst as cm on qm.dbconnection_id = cm.id "
                 + "WHERE rm.report_id IN(" + id + ") and qm.is_deleted = 0";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for connection table using these parameters
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="dsnName"></param>
        /// Defines the dsn name
        /// <param name="userID"></param>
        /// Defines the user id
        /// <param name="pwd"></param>
        /// Defines the password
        /// <param name="schemaName"></param>
        /// Defines the database name
        /// <returns></returns>
        public static string sql_ConnSelectQuery(string s, string dsnName, string userID, string pwd, string schemaName)
        {
            pSql = "SELECT * FROM " + s + "dbconnection_mst WHERE dsn_name='" + dsnName + "' "
                 + "AND user_id='" + userID + "' AND password='" + pwd + "' AND db_schemaname='" + schemaName + "'";
            return pSql;
        }

        /// <summary>
        /// Used for insertion in connection table
        /// Returns sql query for connection table using these parameters
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="dsnName"></param>
        /// Defines the dsn name
        /// <param name="dbType"></param>
        /// Defines the database type
        /// <param name="userId"></param>
        /// Defines the user id
        /// <param name="pwd"></param>
        /// Defines the password
        /// <param name="schemaName"></param>
        /// Defines the schema name (database name)
        /// <param name="connTimeOut"></param>
        /// Defines the Connection time out variable
        /// <param name="QryTimeOut"></param>
        /// Defines the query time out variable
        /// <param name="created_on"></param>
        ///  Defines the created on variable
        /// <param name="is_deleted"></param>
        /// Defines the is_deleted variable
        /// <returns></returns>
        public static string sql_ConnInsertQuery(string s, string dsnName, int dbType, string userId, string pwd, string schemaName, int connTimeOut, int QryTimeOut, string created_on, int is_deleted)
        {
            pSql = "Insert into " + s + "dbconnection_mst(connection_name, db_type, driver_type, dsn_name, user_id, password,"
                 + "db_schemaname, connection_timeout, query_timeout, created_on, is_deleted) "
                 + "Values('" + dsnName + "'," + dbType + "," + dbType + ",'" + dsnName + "','" + userId + "',"
                 + "'" + pwd + "','" + schemaName + "'," + connTimeOut + "," + QryTimeOut + ","
                 + "'" + created_on + "'," + is_deleted + ") RETURNING id";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="roleID"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string sql_ComboReportQry(string s, int roleID)
        {
            pSql = "SELECT * FROM " + s + "report_mst WHERE is_deleted=0 ORDER BY report_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard_query_mst table by using its id
        /// and mapping with dbconnection_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the query id
        /// <returns></returns>
        public static string sql_editQryQuery(string s, int id)
        {
            pSql = "SELECT dqm.query_id,dqm.function_name,dqm.type,dqm.is_deleted,dqm.query_user_id,"
                 + "CONCAT(dqm.query_script, ' ', dqm.query_where_cond, ' ', dqm.query_group_by, ' ', dqm.query_order_by) qry_script,"
                 + "cm.id,cm.dsn_name,cm.user_id,cm.password,cm.db_schemaname "
                 + "FROM " + s + "dashboard_query_mst as dqm "
                 + "LEFT JOIN " + s + "dbconnection_mst as cm on dqm.dbconnection_id = cm.id "
                 + "WHERE dqm.query_id IN(" + id + ") and dqm.is_deleted = 0";
            return pSql;
        }

        /// <summary>
        ///  Returns sql query for report table by using its id
        ///  Used for updating the is_deleted column of report table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="is_deleted"></param>
        /// Holds the is_deleted value
        /// <param name="id"></param>
        /// Holds the report id
        /// <returns></returns>
        public static string UpdateReport_mst(string s, string is_deleted, string id)
        {
            pSql = "UPDATE " + s + "report_mst Set is_deleted=" + is_deleted + " WHERE report_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        ///  Returns sql query for kpi table mapping with chart table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_kpiview_Qry(string s)
        {
            pSql = "SELECT es.kpi_id,es.chart_type_id ,es.kpi_name,es.description,es.is_deleted , dcs.chart_type , CASE WHEN es.is_deleted=0 THEN 'Active' ELSE 'Disable' END AS Status FROM " + s + "dashboard_kpi_charts as es " +
                "left join " + s + "dashboard_chart_mst as dcs on dcs.chart_type_id=es.chart_type_id order by es.kpi_id desc";

            return pSql;
        }

        /// <summary>
        ///  Returns sql query for kpi table mapping with dashboard_query_mst and dashboard_chart_mst tables using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the kpi_id
        /// <returns></returns>
        public static string sql_editQryKPIQuery(string s, int id)
        {
            pSql = "SELECT dcm.chart_type, es.kpi_id,es.chart_type_id ,es.kpi_name,es.description,es.is_deleted,dqm.query_script ,CASE WHEN es.is_deleted=0 THEN 'Active' ELSE 'Disable' END AS Status FROM " + s + "dashboard_kpi_charts as es  " +
                "left join " + s + "dashboard_query_mst as dqm on dqm.function_name=es.kpi_name" +
                " left join  " + s + "dashboard_chart_mst as dcm on dcm.chart_type_id=es.chart_type_id " +
                "  where es.kpi_id IN(" + id + ")";
            return pSql;
        }

        /// <summary>
        ///  Returns sql query for dashboard_query_mst table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the query id
        /// <returns></returns>
        public static string sql_qry_mst(string s, int id)
        {
            pSql = "SELECT type FROM " + s + "dashboard_query_mst WHERE query_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table by using its id
        /// Used for updating the is_deleted column in kpi table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="is_deleted"></param>
        /// Holds the is_deleted column value
        /// <param name="id"></param>
        /// Holds the kpi id
        /// <returns></returns>
        public static string UpdateKPI_mst(string s, string is_deleted, string id)
        {
            pSql = "UPDATE " + s + "dashboard_kpi_charts Set is_deleted=" + is_deleted + " WHERE kpi_id = " + id + "";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using its id
        /// Used for updating the is_deleted column in dashboard table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="is_deleted"></param>
        /// Holds the is_deleted column value
        /// <param name="id"></param>
        /// Holds the dashboard id
        /// <returns></returns>
        public static string UpdateDashboard_mst(string s, string is_deleted, string id)
        {
            pSql = "UPDATE " + s + "dashboard_mst Set is_deleted=" + is_deleted + " WHERE dashboard_id = " + id + "";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table by using its id
        /// and mapping with report_query_map and report_query_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the report id
        /// <returns></returns>
        public static string sql_getConnectionId(string s, int id)
        {
            pSql = "select dbconnection_id from " + s + "report_mst as rm " +
                "left join " + s + "report_query_map as rqm on rm.report_id = rqm.report_id " +
                "left join " + s + "report_query_mst as rqmm on rqm.query_id = rqmm.query_id " +
                "where rm.report_id = " + id + "";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_dashboardName(string s)
        {
            pSql = "SELECT dashboard_id,dashboard_name FROM " + s + "dashboard_mst WHERE is_deleted=0 ORDER BY dashboard_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_RoleName(string s)
        {
            pSql = "SELECT role_id,role_name FROM " + s + "user_role_mst WHERE is_deleted=0 ORDER BY role_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_UserTypeName(string s)
        {
            pSql = "SELECT * FROM " + s + "user_role";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the pipeline id
        /// <returns></returns>
        public static string sql_qry_user(string s, string id)
        {
            pSql = "SELECT DISTINCT on (pipelinemst_id) pipelinemst_id,pipeline_name FROM " + s + "pipeline_mst WHERE pipelinemst_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_editRoleMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "user_role_mst WHERE is_deleted=0 AND role_id = (" + id + ")";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table by using its id
        /// and mapping with menu_role_map and menu_mst tables 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleAccessQry(string s, string id)
        {
            pSql = "SELECT ur.role_id,ur.role_name,ur.role_description,mm.menu_id,mm.menu_name,mm.menu_url,mm.menu_parent_id," +
                "mm.menu_route,mm.menu_icon,mm.group,mrm.allow_create,mrm.allow_read,mrm.allow_update,mrm.allow_delete " +
                "FROM " + s + "user_role_mst as ur " +
                "LEFT JOIN " + s + "menu_role_map as mrm on mrm.role_id = ur.role_id " +
                "LEFT JOIN " + s + "menu_mst as mm on mm.menu_id = mrm.menu_id " +
                "WHERE ur.is_deleted=0 AND mm.is_deleted=0 AND ur.role_id IN (" + id + ") " +
                "ORDER BY role_id,menu_id ASC";

            return pSql;
        }

        /// <summary>
        /// Return dashboard id and dashboard name for dashboard drop down 
        /// After the selection of role type in user popup
        /// Mapping of dashboard_mst with dashboard_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_DashboardDDById(string s, int id)
        {
            pSql = "SELECT dm.dashboard_id, dm.dashboard_name FROM " + s + "dashboard_mst as dm " 
                 + "LEFT JOIN " + s + "dashboard_role_map drm on dm.dashboard_id = drm.dashboard_id "
                 + "WHERE dm.is_deleted=0 AND drm.allow_read=true AND drm.role_id=" + id + " " 
                 + "ORDER BY dm.dashboard_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for menu table by using menu_parent_id 
        /// and mapping with menu_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the menu_parent_id
        /// <param name="roleId"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_menuByID(string s, int id, int roleId)
        {
            //pSql = "SELECT * FROM " + s + "menu_mst WHERE is_deleted=0 AND menu_parent_id = " + id + " ORDER BY menu_order ASC";
            pSql = "SELECT * FROM " + s + "menu_mst as mm LEFT JOIN " + s + "menu_role_map mrm on mm.menu_id = mrm.menu_id "
                 + "WHERE mm.is_deleted=0 AND mrm.role_id=" + roleId + " AND mm.menu_parent_id=" + id + " ORDER BY mm.menu_order ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for menu table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_AllMenuItems(string s)
        {
            pSql = "SELECT menu_id,menu_name FROM " + s + "menu_mst WHERE is_deleted=0 ORDER BY menu_id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_RoleWise_Qry(string s)
        {
            pSql = "SELECT role_id,role_name,role_description,is_deleted from " + s + "user_role_mst WHERE is_deleted=0 ORDER BY role_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using role id
        /// and mapping with dashboard_role_map and user_role_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_editRoleDashboardMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_mst as dm "
              + "LEFT JOIN " + s + "dashboard_role_map AS drm ON drm.dashboard_id=dm.dashboard_id "
              + "LEFT JOIN " + s + "user_role_mst as urm on urm.role_id = drm.role_id "
              //+ "LEFT JOIN " + s + "user_login AS ul on ul.role_id = urm.role_id "
              + "LEFT JOIN " + s + "user_login as ul ON dm.created_by=ul.id "
              + "WHERE dm.is_deleted=0 AND urm.is_deleted=0 AND drm.role_id IN (" + id + ") or drm.role_id isnull";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table by using role id
        /// and mapping with kpi_role_map and user_role_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_editRoleKPIMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_kpi_charts as dkc "
                + "LEFT JOIN " + s + "kpi_role_map AS krm ON dkc.kpi_id = krm.kpi_id "
                + "LEFT JOIN " + s + "user_role_mst as urm on urm.role_id = krm.role_id "
                //+ "LEFT JOIN " + s + "user_login AS ul on ul.role_id = urm.role_id "
                + "LEFT JOIN " + s + "user_login as ul ON dkc.created_by=ul.id "
                + "WHERE dkc.is_deleted=0 AND urm.is_deleted=0 AND krm.role_id IN (" + id + ") or krm.role_id isnull";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table by using role id
        /// and mapping with query_role_map and user_role_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// <returns></returns>
        public static string sql_editRoleQueryMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_query_mst as dqm "
                 + "LEFT JOIN " + s + "query_role_map AS qrm ON dqm.query_id = qrm.dashboard_query_id "
                 + "LEFT JOIN " + s + "user_role_mst as urm on urm.role_id = qrm.role_id "
                 //+ "LEFT JOIN " + s + "user_login AS ul on ul.role_id = urm.role_id "
                 + "LEFT JOIN " + s + "user_login as ul ON dqm.created_by=ul.id " 
                 + "WHERE dqm.is_deleted=0 AND urm.is_deleted=0 AND qrm.role_id IN (" + id + ") or qrm.role_id isnull"; 
            
            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table by using role id
        /// and mapping with report_role_map and user_role_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_editRoleReportMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "report_mst as rm " +
               "LEFT JOIN " + s + "report_role_map AS rrm ON rrm.report_id=rm.report_id " +
               "LEFT JOIN " + s + "user_role_mst as urm on urm.role_id = rrm.role_id " +
               //"LEFT JOIN " + s + "user_login AS ul on ul.role_id = urm.role_id " +
               "LEFT JOIN " + s + "user_login as ul ON rm.created_by=ul.id " +
               "WHERE rm.is_deleted=0 AND urm.is_deleted=0 AND rrm.role_id IN (" + id + ") or rrm.role_id isnull";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for menu table by using role id
        /// and mapping with menu_role_map and user_role_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_editRoleMenuMapQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "menu_mst as mm "
                 + "LEFT JOIN " + s + "menu_role_map AS mrm ON mm.menu_id = mrm.menu_id "
                 + "LEFT JOIN " + s + "user_role_mst as urm on mrm.role_id = urm.role_id "
                 + "LEFT JOIN " + s + "user_login as ul ON mm.created_by=ul.id "
                 + "WHERE mm.is_deleted=0 AND urm.is_deleted=0 AND mrm.role_id IN (" + id + ") or mrm.role_id isnull"; 

            return pSql;
        }

        /// <summary>
        /// Returns sql query for menu table
        /// Mapping with user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public static string sql_newRoleMenuMapQuery(string s)
        {
            pSql = "SELECT * FROM " + s + "menu_mst as mm "
                 + "LEFT JOIN " + s + "user_login as ul ON mm.created_by=ul.id "
                 + "WHERE mm.is_deleted=0 ORDER BY mm.menu_id DESC";

            return pSql;
        }
        
        /// <summary>
        /// Returns sql query for report table
        /// Mapping with user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public static string sql_newRoleReportMapQuery(string s)
        {
            pSql = "SELECT * FROM " + s + "report_mst as rm "
                 + "LEFT JOIN " + s + "user_login as ul ON rm.created_by=ul.id "
                 + "WHERE rm.is_deleted=0 ORDER BY rm.report_id DESC";

            return pSql;
        }
        
        /// <summary>
        /// Returns sql query for dashboard table
        /// Mapping with user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public static string sql_newRoleDashboardMapQuery(string s)
        {
            pSql = "SELECT * FROM " + s + "dashboard_mst as dm "
                 + "LEFT JOIN " + s + "user_login as ul ON dm.created_by=ul.id "
                 + "WHERE dm.is_deleted=0 ORDER BY dm.dashboard_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for KPI table
        /// Mapping with user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public static string sql_newRoleKPIMapQuery(string s)
        {
            pSql = "SELECT * FROM " + s + "dashboard_kpi_charts as dkc "
                 + "LEFT JOIN " + s + "user_login as ul ON dkc.created_by=ul.id "
                 + "WHERE dkc.is_deleted=0 ORDER BY dkc.kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table
        /// Mapping with user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public static string sql_newRoleQueryMapQuery(string s)
        {
            pSql = "SELECT * FROM " + s + "dashboard_query_mst as dqm "
                 + "LEFT JOIN " + s + "user_login as ul ON dqm.created_by=ul.id "
                 + "WHERE dqm.is_deleted=0 ORDER BY dqm.query_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline table by using its name
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the pipeline name
        /// <returns></returns>
        public static string sql_UserPipelineId(string s, string id)
        {
            pSql = "SELECT pipelinemst_id FROM " + s + "pipeline_mst WHERE pipeline_name IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using role id
        /// and mapping with dashboard_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleDashboardMapQuery(string s, int id)
        {
            pSql = "SELECT dm.dashboard_id FROM " + s + "dashboard_mst as dm " +
                "LEFT JOIN " + s + "dashboard_role_map as drm on dm.dashboard_id = drm.dashboard_id " +
                "WHERE dm.is_deleted=0 AND drm.role_id = " + id + " ORDER BY dm.dashboard_id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table by using role id
        /// and mapping with kpi_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleKPIMapQuery(string s, int id)
        {
            pSql = "SELECT dkc.kpi_id FROM " + s + "dashboard_kpi_charts as dkc " +
                "LEFT JOIN " + s + "kpi_role_map as krm on dkc.kpi_id = krm.kpi_id " +
                "WHERE dkc.is_deleted=0 AND krm.role_id = " + id + " ORDER BY dkc.kpi_id ASC";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table by using role id
        /// and mapping with query_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleQueryMapQuery(string s, int id)
        {
            pSql = "SELECT dqm.query_id FROM " + s + "dashboard_query_mst as dqm " +
                "LEFT JOIN " + s + "query_role_map as qrm on dqm.query_id = qrm.dashboard_query_id " +
                "WHERE dqm.is_deleted=0 AND qrm.role_id = " + id + " ORDER BY dqm.query_id ASC";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table by using role id
        /// and mapping with report_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleReportMapQuery(string s, int id)
        {
            pSql = "SELECT rm.report_id FROM " + s + "report_mst as rm " +
                "LEFT JOIN " + s + "report_role_map as urm on rm.report_id = urm.report_id " +
                "WHERE rm.is_deleted=0 AND urm.role_id = " + id + " ORDER BY rm.report_id ASC";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for menu table by using role id
        /// and mapping with menu_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleMenuMapQuery(string s, int id)
        {
            pSql = "SELECT mm.menu_id FROM " + s + "menu_mst as mm " +
                "LEFT JOIN " + s + "menu_role_map as mrm on mm.menu_id = mrm.menu_id " +
                "WHERE mm.is_deleted=0 AND mrm.role_id = " + id + " ORDER BY mm.menu_id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using role id
        /// and mapping with dashboard_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleDashboardQuery(string s, int id)
        {
            pSql = "SELECT dm.dashboard_id FROM " + s + "dashboard_mst as dm " +
                "LEFT JOIN " + s + "dashboard_role_map as drm on dm.dashboard_id = drm.dashboard_id " +
                "WHERE dm.is_deleted=0 AND drm.allow_read = true AND drm.role_id = " + id + " ORDER BY dm.dashboard_id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table by using role id
        /// and mapping with kpi_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleKPIQuery(string s, int id)
        {
            pSql = "SELECT dkc.kpi_id FROM " + s + "dashboard_kpi_charts as dkc " +
                "LEFT JOIN " + s + "kpi_role_map as krm on dkc.kpi_id = krm.kpi_id " +
                "WHERE dkc.is_deleted=0 AND krm.allow_read = true AND krm.role_id = " + id + " ORDER BY dkc.kpi_id ASC";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for user table by using its id
        /// Used for updating the is_active column in table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema aof database
        /// <param name="is_active"></param>
        /// Holds the is_active column value
        /// <param name="id"></param>
        /// Holds the user id
        /// <returns></returns>
        public static string UpdateUser_mst(string s, string is_active, string id)
        {
            pSql = "UPDATE " + s + "user_login Set is_active=" + is_active + " WHERE id = " + id + "";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for user table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_UserWise_Qry(string s)
        {
            pSql = "SELECT id,first_name,last_name,email_id,password,user_type,region,role_id,is_active,is_deleted," +
                "CASE WHEN is_active=1 THEN 'Active' ELSE 'Disable' END AS user_status from " + s + "user_login " +
                "WHERE is_deleted=0 ORDER BY id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for user table by using its id
        /// and mapping with user_role_mst and dashboard_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the user id
        /// <returns></returns>
        public static string sql_editUserQuery(string s, int id)
        {
            //pSql = "SELECT * FROM " + s + "user_login WHERE is_deleted=0 AND id IN (" + id + ")";
            //pSql = "SELECT ul.id,ul.first_name,ul.last_name,ul.email_id,ul.password,ul.user_type,ul.region,ul.role_id,ul.is_active,ul.is_deleted,ul.dashboard,urm.role_id,urm.role_name,urm.role_description,dm.dashboard_id,dm.dashboard_name,CASE WHEN ul.is_active=1 THEN 'Active' ELSE 'Disable' END AS user_status from " + s + "user_login ul left join " + s + "user_role_mst as urm on ul.role_id = urm.role_id left join " + s + "dashboard_mst as dm on ul.dashboard = dm.dashboard_id WHERE ul.is_deleted=0 AND urm.is_deleted=0 AND dm.is_deleted=0 AND ul.id IN (" + id + ")";
            pSql = "SELECT ul.id,ul.first_name,ul.last_name,ul.email_id,ul.password,ul.user_type,ul.region,ul.role_id," +
                "ul.is_active,ul.is_deleted,ul.dashboard,urm.role_id,urm.role_name,urm.role_description,dm.dashboard_id," +
                "dm.dashboard_name,CASE WHEN ul.is_active=1 THEN 'Active' ELSE 'Disable' END AS user_status " +
                "from " + s + "user_login ul left join " + s + "user_role_mst as urm on ul.role_id = urm.role_id " +
                "left join " + s + "dashboard_mst as dm on ul.dashboard = dm.dashboard_id " +
                "WHERE ul.is_deleted=0 AND dm.is_deleted=0 AND ul.id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleName(string s, int id)
        {
            pSql = "SELECT role_id,role_name FROM " + s + "user_role_mst WHERE is_deleted=0 AND role_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using its id
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the dashboard id
        /// <returns></returns>
        public static string sql_dashboardName(string s, int id)
        {
            pSql = "SELECT dashboard_id,dashboard_name FROM " + s + "dashboard_mst WHERE is_deleted=0 AND dashboard_id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table
        /// Mapping with dashboard_query_map, dashboard_query_mst and dashboard_chart_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_DashboardKpiQuery(string s)
        {
            pSql = "SELECT * from " + s + "dashboard_kpi_charts as kc "
                + "left join " + s + "dashboard_query_map as qm on kc.kpi_id = qm.kpi_id "
                + "left join " + s + "dashboard_query_mst as dqm on qm.query_id = dqm.query_id "
                + "left join " + s + "dashboard_chart_mst as cm on kc.chart_type_id = cm.chart_type_id "
                + "WHERE kc.is_deleted = 0 AND kc.system_kpi = 0 AND cm.is_deleted = 0 ORDER BY kc.kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table
        /// Mapping with dashboard_query_map, dashboard_query_mst, dashboard_chart_mst and kpi_role_map tables 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string sql_RoleWiseDashboardKpi(string s, int id)
        {
            pSql = "SELECT * from " + s + "dashboard_kpi_charts as dkc "
                + "left join " + s + "dashboard_query_map as qm on dkc.kpi_id = qm.kpi_id "
                + "left join " + s + "dashboard_query_mst as dqm on qm.query_id = dqm.query_id "
                + "left join " + s + "dashboard_chart_mst as cm on dkc.chart_type_id = cm.chart_type_id "
                + "left join " + s + "kpi_role_map as krm on dkc.kpi_id = krm.kpi_id "
                + "WHERE dkc.is_deleted = 0 AND dkc.system_kpi = 0 AND cm.is_deleted = 0 AND krm.allow_read = true "
                + "AND krm.role_id = " + id + " ORDER BY dkc.kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table by using its id
        /// Mapping with dashboard_query_map and dashboard_chart_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// <returns></returns>
        public static string sql_DashboardKpiByQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_kpi_charts as kc "
                 + "left join " + s + "dashboard_query_map as qm on kc.kpi_id = qm.kpi_id "
                 + "left join " + s + "dashboard_chart_mst as cm on kc.chart_type_id = cm.chart_type_id "
                 + "WHERE kc.is_deleted=0 AND cm.is_deleted=0 AND kc.kpi_id = " + id + "";

            return pSql;
        }

        /// <summary>
        ///  Returns sql query for report table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_AllReportData(string s)
        {
            pSql = "SELECT report_id,report_name FROM " + s + "report_mst WHERE is_deleted=0";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_dashboardDataOnly(string s)
        {
            //pSql = "SELECT dashboard_id,dashboard_name,is_deleted,dashboard_description,CASE WHEN is_deleted = 0 THEN 'Active' ELSE 'Disable' END AS dashboard_status FROM " + s + "dashboard_mst ORDER BY dashboard_id DESC";
            pSql = "SELECT dashboard_id,dashboard_name,is_deleted,dashboard_description " +
                "FROM " + s + "dashboard_mst WHERE is_deleted = 0 ORDER BY dashboard_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_DashboardById(string s, int id)
        {
            pSql = "SELECT dm.dashboard_id, dm.is_deleted, dm.dashboard_name, dm.dashboard_description "
                 + "FROM " + s + "dashboard_mst as dm "
                 + "LEFT JOIN " + s + "dashboard_role_map drm on dm.dashboard_id = drm.dashboard_id "
                 + "WHERE dm.is_deleted=0 AND drm.allow_read=true AND drm.role_id=" + id + " "
                 + "ORDER BY dm.dashboard_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for KPI table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_KPIById(string s, int id)
        {
            pSql = "SELECT dkc.kpi_id, dkc.is_deleted, dkc.kpi_name,dcs.chart_type, dkc.description "
                 + "FROM " + s + "dashboard_kpi_charts as dkc " 
                 + "left join " + s + "dashboard_chart_mst as dcs on dkc.chart_type_id=dcs.chart_type_id "
                 + "LEFT JOIN " + s + "kpi_role_map krm on dkc.kpi_id=krm.kpi_id "
                 + "WHERE dkc.is_deleted=0 AND krm.allow_read=true AND krm.role_id=" + id + " "
                 + "ORDER BY dkc.kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table
        /// </summary>
        /// <param name="s"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        ///  Holds the role id
        /// <returns></returns>
        public static string sql_QueryById(string s, int id)
        {
            pSql = "SELECT dqm.query_id, dqm.is_deleted, dqm.function_name, dqm.type,"
                + "concat(dqm.query_script,' ',dqm.query_where_cond,' ',dqm.query_group_by,' ',dqm.query_order_by) Query_script "
                + "FROM " + s + "dashboard_query_mst as dqm "
                + "LEFT JOIN " + s + "query_role_map as qrm on dqm.query_id = qrm.dashboard_query_id "
                + "WHERE dqm.is_deleted=0 AND qrm.allow_read=true AND qrm.role_id=" + id + " "
                + "ORDER BY dqm.query_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table
        /// </summary>
        /// <param name="s"></param>
        ///  Holds the schema of database
        /// <param name="id"></param>
        ///  Holds the role id
        /// <returns></returns>
        public static string sql_ReportById(string s, int id)
        {
            pSql = "SELECT rm.report_id, rm.is_deleted, rm.report_name, rm.report_description "
                + "FROM " + s + "report_mst as rm "
                + "LEFT JOIN " + s + "report_role_map as rrm on rm.report_id = rrm.report_id "
                + "WHERE rm.is_deleted=0 AND rrm.allow_read=true AND rrm.role_id=" + id + " "
                + "ORDER BY rm.report_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_kpiDataOnly(string s)
        {
            pSql = "SELECT kpi_id, kpi_name, is_deleted, description FROM " + s + "dashboard_kpi_charts " +
                "WHERE is_deleted = 0 ORDER BY kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_queryDataOnly(string s)
        {
            pSql = "SELECT query_id, function_name, is_deleted FROM " + s + "dashboard_query_mst " +
                "WHERE is_deleted = 0 ORDER BY query_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using role id
        /// and mapping with dashboard_role_map table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the role id
        /// <returns></returns>
        public static string sql_RoleWiseDashboardData(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_mst as dm "
                + "LEFT JOIN " + s + "dashboard_role_map as drm ON dm.dashboard_id = drm.dashboard_id "
                + "WHERE dm.is_deleted = 0 AND drm.role_id IN (" + id + ")"
                + "ORDER BY dm.dashboard_id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table by using its id
        /// and mapping with dashboard_kpi_map, dashboard_kpi_charts, dashboard_query_map, dashboard_query_mst and dashboard_chart_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the dashboard id
        /// <returns></returns>
        public static string sql_dashboardIdData(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "dashboard_mst as dm " 
                + "left join " + s + "dashboard_kpi_map as dkm on dm.dashboard_id = dkm.dashboard_id "
                + "left join " + s + "dashboard_kpi_charts as dkc on dkm.kpi_id = dkc.kpi_id "
                + "left join " + s + "dashboard_query_map as dqm on dkc.kpi_id = dqm.kpi_id " 
                + "left join " + s + "dashboard_query_mst as dq on dqm.query_id = dq.query_id " 
                + "left join " + s + "dashboard_chart_mst as dcm on dkc.chart_type_id = dcm.chart_type_id " 
                + "WHERE dm.is_deleted=0 AND dkc.is_deleted=0 AND dcm.is_deleted=0 AND " 
                + "dm.dashboard_id = " + id + " ORDER BY dkm.id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for diagnostic_kpi_map table by using abbrevation name
        /// And mapping with dashboard_kpi_charts, dashboard_query_map, dashboard_query_mst and dashboard_chart_mst tables
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="name"></param>
        /// Holds the name of abbrevation
        /// <returns></returns>
        public static string GetDiagnosticKpiMapQuery(string s, string name)
        {
            pSql = "SELECT * FROM " + s + "diagnostic_kpi_map as dkm " +
                "left join " + s + "dashboard_kpi_charts as dkc on dkm.kpi_id = dkc.kpi_id " +
                "left join " + s + "dashboard_query_map as dqm on dkc.kpi_id = dqm.kpi_id " +
                "left join " + s + "dashboard_query_mst as dq on dqm.query_id = dq.query_id " +
                "left join " + s + "dashboard_chart_mst as dcm on dkc.chart_type_id = dcm.chart_type_id " +
                "WHERE dkc.is_deleted=0 AND dcm.is_deleted=0 AND dkm.abbrevation_name = '" + name + "' ORDER BY dkm.id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ReportQuery(string s)
        {
            pSql = "SELECT report_id,is_deleted,report_name,report_description FROM " + s + "report_mst WHERE is_deleted=0 ORDER BY report_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline_job_master table 
        /// Mapping with emailerjobs_emails table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ScheduleQuery(string s)
        {
            pSql = "SELECT pjm.id, pjm.is_deleted as is_deleted, pjm.job_name, upper(pjm.job_status) as duration,"
               + "pjm.job_timeinterval as recurrence, pjm.frequency_days as frequency,"
               + "pjm.job_path_filename as report_name,"
               + "ee.email_to, ee.email_cc,"
               + "to_char(pjm.job_lastexetime,'YYYY-MM-DD hh24:mi:ss') as last_execution,"
               + "to_char(pjm.job_nextexetime,'YYYY-MM-DD hh24:mi:ss') as next_execution,"
               + "CASE WHEN pjm.status = 0 THEN 'Active' ELSE 'Inactive' END AS Status "
               + "FROM " + s + "pipeline_job_master as pjm "
               + "LEFT JOIN " + s + "emailerjobs_emails as ee ON pjm.job_id = ee.schdr_jobid "
               + "WHERE pjm.is_deleted=0 "
               + "ORDER BY pjm.job_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline_job_history table 
        /// Mapping with pipeline_job_master and user_login table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the job id
        /// <returns></returns>
        public static string sql_JobHistory(string s, int id)
        {
            pSql = "SELECT pjh.id, pjh.job_title, pjh.job_filename, pjh.job_script, pjm.job_name,"
               + "ul.user_type, pjh.status, pjh.created_on "
               + "FROM " + s + "pipeline_job_history as pjh "
               + "LEFT JOIN " + s + "pipeline_job_master as pjm on pjh.jobmst_id = pjm.id "
               + "LEFT JOIN " + s + "user_login as ul on pjh.created_by = ul.id "
               + "WHERE pjh.jobmst_id IN(" + id + ")"
               + "ORDER BY pjh.id ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline_job_master table
        /// Used for updating the status column in table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="status_val"></param>
        /// Holds the status column value
        /// <param name="id"></param>
        /// <returns></returns>
        public static string UpdateScheduler_mst(string s, string status_val, string id)
        {
            pSql = "UPDATE " + s + "pipeline_job_master Set status=" + status_val + " WHERE id IN (" + id + ")";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for report table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_ReportQueryData(string s)
        {
            pSql = "SELECT rm.report_id,rm.is_deleted,rm.report_name,rm.report_description FROM " + s + "report_mst as rm ORDER BY rm.report_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for user table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_UserWiseQuery(string s)
        {
            pSql = "SELECT id,is_deleted,first_name,last_name,email_id,user_type from " + s + "user_login WHERE is_deleted=0 ORDER BY id DESC";
            return pSql;
        }

        /// <summary>
        /// Returns sql query for role table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_RoleWiseQuery(string s)
        {
            pSql = "SELECT role_id,is_deleted,role_name,role_description from " + s + "user_role_mst WHERE is_deleted=0 ORDER BY role_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_DashboardQuery(string s)
        {
            pSql = "SELECT dashboard_id,is_deleted,dashboard_name,dashboard_description FROM " + s + "dashboard_mst ORDER BY dashboard_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for kpi table 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string sql_KPIQuery(string s)
        {
            pSql = "SELECT es.kpi_id,es.is_deleted,es.kpi_name,dcs.chart_type,es.description FROM " + s + "dashboard_kpi_charts as es left join " + s + "dashboard_chart_mst as dcs on dcs.chart_type_id=es.chart_type_id ORDER BY es.kpi_id DESC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for dashboard query table 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_DBQuery(string s)
        {
            pSql = "SELECT query_id,is_deleted,function_name,type,"
                  + "concat(query_script,' ',query_where_cond,' ',query_group_by,' ',query_order_by) Query_script "
                  + "FROM " + s + "dashboard_query_mst WHERE is_deleted=0 ORDER BY function_name ASC";

            return pSql;
        }

        /// <summary>
        /// Returns sql query for pipeline_job_master table by using its id
        /// Mapping with emailerjobs_emails table
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="id"></param>
        /// Holds the job id
        /// <returns></returns>
        public static string sql_editSchedularQuery(string s, int id)
        {
            pSql = "SELECT * FROM " + s + "pipeline_job_master as pjm LEFT JOIN " + s + "emailerjobs_emails as jm "
                 + "ON pjm.job_id = jm.schdr_jobid "
                 + "WHERE pjm.id= " + id + "";
            return pSql;
        }

        /// <summary>
        /// Returns the Diagnostic Summary 
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <returns></returns>
        public static string sql_DiagnosticSummaryQuery(string s)
        {
            pSql = "SELECT mm.mmst_id,mm.is_deleted, mm.consumer_name as consumer, mt.value as meter_type,"
                    + "mmsm.stream_no, mb.maintenance_base_name as maintenance, pm.pipeline_name as region,"
                    + "mmsm.status, mmsm.calibration, mmsm.health, mmsm.ghg, mmpmd.make, mmpmd.model,"
                    + "mmsm.transducer_failure, mmsm.detection_problem, mmsm.ultrasonic_noise, mmsm.process_condition_pressure as process_pressure,"
                    + "mmsm.process_condition_temprature as process_temprature, mmsm.fouling, mmsm.change_in_flow_profile as flow_profile, mmsm.high_velocity "
                    + "FROM " + s + "mmst as mm "
                    + "LEFT JOIN " + s + "pipeline_mst as pm on mm.pipelinemst_id = pm.pipelinemst_id "
                    + "LEFT JOIN " + s + "pipe_station as ps on mm.station_id = ps.station_id "
                    + "LEFT JOIN " + s + "maintenance_base as mb on mm.maintenance_base_id = mb.maintenance_base_id "
                    + "LEFT JOIN " + s + "mmst_summary_map as mmsm on mm.mmst_id = mmsm.mmst_id "
                    + "LEFT JOIN " + s + "mmst_primary_meter_data as mmpmd on mmsm.primary_id = mmpmd.primary_id and mmsm.stream_id = mmpmd.stream_id "
                    + "LEFT JOIN " + s + "metering_type as mt on mmpmd.meter_type = mt.key::int "
                    + "LEFT JOIN " + s + "asset_mst am on am.asset_id = mmpmd.fc_id "
                    + "WHERE mm.is_deleted = 0 AND mm.consumer_name != '' "
                    + "ORDER BY mm.consumer_name ASC";

            return pSql;

        }

        /// <summary>
        /// Returns the GC Query
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="date"></param>
        /// Defines the date
        /// <returns></returns>
        public static string sql_GCQuery(string s, string date)
        {
            pSql = "SELECT gcm.gc_id, gcm.is_deleted, gcm.gc_name, pm.pipeline_name, ps.station_name, gct.gc_status as status,"
                    + "gcv.r2c1_c6, gcv.r2c126, gcv.r2c345 "
                    + "FROM " + s + "gc_mst as gcm "
                    + "LEFT JOIN " + s + "gc_values as gcv on gcm.gc_id = gcv.gc_id "
                    + "LEFT JOIN " + s + "gc_thresholds as gct on gcm.gc_id = gct.gc_id "
                    + "LEFT JOIN " + s + "pipeline_mst as pm on gcm.gc_region = pm.pipelinemst_id "
                    + "LEFT JOIN " + s + "pipe_station as ps on gcm.gc_station = ps.station_id "
                    + "WHERE gcm.is_deleted = 0 AND date(gcv.gc_to_date) = '" + date + "' "
                    + "ORDER BY gcm.gc_name ASC"; 

            return pSql;

        }

        /// <summary>
        /// Returns the Survey Query
        /// </summary>
        /// <param name="s"></param>
        /// Defines the schema of database
        /// <param name="date"></param>
        /// Defines the date
        /// <returns></returns>
        public static string sql_SurveyQuery(string s, string date)
        {
            pSql = "Select row_number() OVER () as id,sm.survey_name,to_char(sm.date,'YYYY-MM-DD') as date,um.user_name, "
                    + "cm.client_name,cm.client_department,sm.survey_status "
                    + "FROM " + s + "survey_mst sm "
                    + "LEFT JOIN " + s + "client_mst cm on sm.client_id=cm.client_id "
                    + "LEFT JOIN " + s + "user_mst um on sm.created_by=um.user_id "
                    //+ "WHERE date(sm.date) = '" + date + "' "
                    + "ORDER BY sm.survey_name ASC";
            return pSql;

        }
    }
}