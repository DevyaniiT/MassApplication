using CrystalDecisions.CrystalReports.Engine;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class GetValue
    {
        //public static string constring = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
        public static string value, sql, qry, query, path_name, file_name, report_name;
        public static ReportDocument rpt = new ReportDocument();
        public static string schemapath = ConfigurationManager.AppSettings["SchemaPath"];
        public static string odbc_name = ConfigurationManager.AppSettings["ODBCName"];
        public static string server_name = ConfigurationManager.AppSettings["server"];
        public static string database_name = ConfigurationManager.AppSettings["database"];
        public static string user_name = ConfigurationManager.AppSettings["user"];
        public static string pwd = ConfigurationManager.AppSettings["password"];

        public static int numValue = 0;

        /// <summary>
        /// Returns the Hashtable data for reconcile data using some parameters
        /// </summary>
        /// <param name="from_date"></param>
        /// Defines the start date
        /// <param name="to_date"></param>
        ///  Defines the end date
        /// <param name="duration"></param>
        ///  Defines the duration
        /// <param name="UnitFactor"></param>
        /// Defines the unit factor i.e, MMSCM, KSCM, SCM etc
        /// <param name="pipeline_id"></param>
        /// Defines the pipeline id
        /// <param name="region"></param>
        /// Defines the region assign to particular user
        /// <returns></returns>
        public static Hashtable GetHashReconcileValue(string from_date, string to_date, string duration, string UnitFactor, string pipeline_id, string region)
        {
            Hashtable hashdata = new Hashtable();
            hashdata.Add("startCalDate", from_date);
            hashdata.Add("endCalDate", to_date);
            hashdata.Add("Duration", duration);
            if (UnitFactor.ToLower() == "mmscm" || UnitFactor.ToLower() == "mmbtu")
            {
                hashdata.Add("UnitFactor", "1000000");
            }

            else if (UnitFactor.ToLower() == "kscm" || UnitFactor.ToLower() == "kbtu")
            {
                hashdata.Add("UnitFactor", "1000");
            }

            else
            {
                hashdata.Add("UnitFactor", "1");
            }

            if (pipeline_id == null || pipeline_id.ToLower() == "all")
            {
                hashdata.Add("Pipelinemst_id", region);
            }

            else
            {
                hashdata.Add("Pipelinemst_id", pipeline_id);
            }
            return hashdata;
        }
        
        /// <summary>
        /// This method is used for generating the crystal report 
        /// </summary>
        /// <param name="report_id"></param>
        /// Defines the report id
        /// <param name="name"></param>
        /// Defines the name
        /// <param name="hashdata"></param>
        /// Defines the hashtable data
        /// <returns></returns>
        public static ReportDocument GenerateReport(int report_id, string name, Hashtable hashdata)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            ReportDocument cry = new ReportDocument();
            sql = ClassFolder.Query_Class.sql_reportData(schemapath, report_id);
            DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                qry = dt.Rows[i]["parameters"].ToString();
                query = dt.Rows[i]["query_script"].ToString();
                path_name = dt.Rows[i]["report_path"].ToString();
                file_name = dt.Rows[i]["report_file"].ToString();
                report_name = dt.Rows[i]["report_name"].ToString();

                string val = ClassFolder.Hash_Class.ReplaceHashdata(query, hashdata);

                string path = path_name + file_name;
                string reportpath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                cry.Load(reportpath);

                cry.DataSourceConnections[0].SetConnection(server_name, database_name, user_name, pwd);

                //cry.SetParameterValue("pipename", ClassFolder.GetValue.GetPipelinename(name));
                cry.SetParameterValue("report_title", report_name);
                cry.SetParameterValue(qry, val);
                rpt = cry;
            }

            return rpt;
        }

        /// <summary>
        /// Returns the pipeline name with using its appropriate id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string GetPipelinename(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_qry(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "pipeline_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["pipeline_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the section name with using its appropriate id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the section id
        /// <returns></returns>
        public static string GetSectionname(string id)
        {
            string section_name = "";

            if (id.ToLower() == "all")
            {
                section_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_Sectionqry(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "section_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    section_name = section_name + dr["section_name"].ToString() + ",";
                }

                section_name = section_name.TrimEnd(',');
            }

            value = section_name;

            return value;
        }

        /// <summary>
        /// Returns the check meter name with using its appropriate id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the check meter id
        /// <returns></returns>
        public static string GetCheckMetername(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_CheckMeterqry(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "check_meter_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["check_meter_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the station name with using its appropriate id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the station id
        /// <returns></returns>
        public static string GetStationName(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_StationId(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "station_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["station_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the GC name with using its appropriate id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the GC id
        /// <returns></returns>
        public static string GetGCName(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_GCId(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "gc_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["gc_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the query ids with using report id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string GetNetworkId(int id)
        {
            string query_id = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_getNetworkId(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "query_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                query_id = query_id + dr["query_id"].ToString() + ",";
            }

            query_id = query_id.TrimEnd(',');
            value = query_id;
            return value;

        }

        /// <summary>
        /// Returns the section id with using pipeline id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string GetSectionId(string id)
        {
            string section_name = "";

            if (id.ToLower() == "all")
            {
                section_name = ClassFolder.Login_Class.region_id;
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_SectionId(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
                
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "section_id");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    section_name = section_name + dr["section_id"].ToString() + ",";
                }

                section_name = section_name.TrimEnd(',');

            }

            value = section_name;
            return value;
        }

        /// <summary>
        /// Returns the connection id with using report id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static int GetConnectionId(int id)
        {
            int rpt_id;
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_getConnectionId(schemapath, id);
            rpt_id = ClassFolder.Database_Class.ExecuteScalar(sql);
            numValue = rpt_id;
            return numValue;
        }

        /// <summary>
        /// Returns the dashboard id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleWiseDashboardId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleDashboardQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "dashboard_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["dashboard_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the KPI id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleWiseKPIId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleKPIQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "kpi_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["kpi_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the dashboard id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleDashboardId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleDashboardMapQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "dashboard_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["dashboard_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the kpi id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleKPIId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleKPIMapQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "kpi_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["kpi_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }
        
        /// <summary>
        /// Returns the query_id id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleQueryId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleQueryMapQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "query_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["query_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the report id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleReportId(int id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleReportMapQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "report_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["report_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the menu id with using role id
        /// </summary>
        /// <param name="id"></param>
        /// Defines the role id
        /// <returns></returns>
        public static string GetRoleMenuId(int id)
        {
            string menu_id = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_RoleMenuMapQuery(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "menu_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                menu_id = menu_id + dr["menu_id"].ToString() + ",";
            }

            menu_id = menu_id.TrimEnd(',');
            value = menu_id;

            return value;
        }

        /// <summary>
        /// Returns the pipeline name with using its appropriate id 
        /// Used in User menu item
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string GetUserPipelinename(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_qry_user(schemapath, id);
                //DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
                DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "pipeline_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["pipeline_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the pipeline name with using its appropriate id
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string GetUserPipelineId(string id)
        {
            string pipe_name = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }

            string[] names = id.Split(',');

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = "'" + names[i].Trim() + "'";
            }

            string result = string.Join(",", names);


            sql = ClassFolder.Query_Class.sql_UserPipelineId(schemapath, result);
            //DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "pipelinemst_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["pipelinemst_id"].ToString() + ",";
            }

            pipe_name = pipe_name.TrimEnd(',');
            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the pipeline name in array of string format with using its appropriate id 
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string[] GetUserPipelineNames(string id)
        {
            string pipe_name = "";
            //string[] pipeline_name = { };

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_qry_user(schemapath, id);
            //DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "pipeline_name");
            foreach (DataRow dr in distinctValues.Rows)
            {
                pipe_name = pipe_name + dr["pipeline_name"].ToString() + ",";
            }
            pipe_name = pipe_name.TrimEnd(',');

            string[] pipeline_name = pipe_name.Split(',');

            //for (int i = 0; i < pipe_name.Length; i++)
            //{
            //    pipeline_name[i] = pipeline_name[i].Trim();
            //}


            return pipeline_name;
        }

        /// <summary>
        /// Returns the type i.e, static or dynamic from the dashboard_query_mst table
        /// </summary>
        /// <param name="id"></param>
        /// Defines the query id
        /// <returns></returns>
        public static string GetKpiParameterType(int id)
        {
            string type = "";
            if (schemapath == null)
            {
                schemapath = "public.";
            }

            sql = ClassFolder.Query_Class.sql_qry_mst(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "type");
            foreach (DataRow dr in distinctValues.Rows)
            {
                type = type + dr["type"].ToString() + ",";
            }

            type = type.TrimEnd(',');
            value = type;

            return value;
        }

        /// <summary>
        /// Returns the Hashtable data 
        /// But not in use
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetKeyWordHash() 
        {
            Hashtable hashdata = new Hashtable();
            string current_date = DateTime.Now.ToString("yyyy-MM-dd");
            string last_month_date = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            hashdata.Add("Date", current_date);
            hashdata.Add("LastMonth", last_month_date);
            hashdata.Add("Asset_id", "13424");
            hashdata.Add("Stream_No", "1");
            return hashdata;
        }

        /// <summary>
        /// Returns the pipeline name with using its appropriate id 
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the pipeline id
        /// <returns></returns>
        public static string GetODBCPipelinename(string id)
        {
            string pipe_name = "";

            if (id == null || id.ToLower() == "all")
            {
                pipe_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_qry(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "pipeline_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    pipe_name = pipe_name + dr["pipeline_name"].ToString() + ",";
                }

                pipe_name = pipe_name.TrimEnd(',');
            }

            value = pipe_name;

            return value;
        }

        /// <summary>
        /// Returns the section name with using its appropriate id 
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the section id
        /// <returns></returns>
        public static string GetODBCSectionname(string id)
        {
            string section_name = "";

            if (id.ToLower() == "all")
            {
                section_name = "All";
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_Sectionqry(schemapath, id);
                DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "section_name");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    section_name = section_name + dr["section_name"].ToString() + ",";
                }

                section_name = section_name.TrimEnd(',');
            }

            value = section_name;

            return value;
        }

        /// <summary>
        /// Returns the query ids with using report id
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        /// Defines the report id
        /// <returns></returns>
        public static string GetODBCNetworkId(int id)
        {
            string query_id = "";

            if (schemapath == null)
            {
                schemapath = "public.";
            }
            sql = ClassFolder.Query_Class.sql_getNetworkId(schemapath, id);
            //DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "query_id");
            foreach (DataRow dr in distinctValues.Rows)
            {
                query_id = query_id + dr["query_id"].ToString() + ",";
            }

            query_id = query_id.TrimEnd(',');
            value = query_id;
            return value;

        }

        /// <summary>
        /// Returns the section id with using pipeline id
        /// Using ODBC Connection
        /// </summary>
        /// <param name="id"></param>
        ///  Defines the pipeline id
        /// <returns></returns>
        public static string GetODBCSectionId(string id)
        {
            string section_name = "";

            if (id.ToLower() == "all")
            {
                section_name = ClassFolder.Login_Class.region_id;
            }

            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                sql = ClassFolder.Query_Class.sql_SectionId(schemapath, id);
                //DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
                DataTable dt = ClassFolder.Database_Class.OpenODBCQuery(sql, odbc_name, user_name, pwd, database_name);

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "section_id");
                foreach (DataRow dr in distinctValues.Rows)
                {
                    section_name = section_name + dr["section_id"].ToString() + ",";
                }

                section_name = section_name.TrimEnd(',');

            }

            value = section_name;
            return value;
        }

        /// <summary>
        /// Returns the dashboard id with using user id
        /// </summary>
        /// <param name="id"></param>
        ///  Defines the user id
        /// <returns></returns>
        public static string GetDashboardByUserId(int id)
        {
            string dashboard_id = "";
            NpgsqlConnection con = new NpgsqlConnection();
            con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            con.Open();
            string sql = "select dashboard from " + schemapath + " user_login where id ='" + id + "'";
            cmd = new NpgsqlCommand(sql, con);
            dashboard_id = Convert.ToString(cmd.ExecuteScalar());
            return dashboard_id;
        }
    }
}