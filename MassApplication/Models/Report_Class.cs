using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Report_Class
    {
        public int report_id { get; set; }
        public int qry_id { get; set; }
        public int conn_id { get; set; }
        public string report_name { get; set; }
        public string report_description { get; set; }
        public string report_path { get; set; }
        public string report_query { get; set; }
        public string report_file { get; set; }
        public string report_status { get; set; }
        public bool is_deleted { get; set; }
        public int report_user_id { get; set; }
        public List<Parameter_Class> param_data { get; set; }
        public List<Query_Class> query_data { get; set; }
        public Connection_Class connData { get; set; }
        //public List<Connection_Class> conn_data { get; set; }

        /// <summary>
        /// Return the list of report table data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="rid"></param>
        /// Holds the id of role currently it is not in use
        /// <returns></returns>
        public List<Report_Class> getReportItems(string schema, int rid)
        {
            try
            {
                List<Report_Class> mydata = new List<Report_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboReportQry(schema, rid);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Report_Class report_class_data = new Report_Class();
                    report_class_data.report_id = Convert.ToInt32(rdr["report_id"]);
                    report_class_data.report_name = rdr["report_name"].ToString();
                    report_class_data.report_description = rdr["report_description"].ToString();
                    report_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    report_class_data.report_user_id = Convert.ToInt32(rdr["report_user_id"]);
                    mydata.Add(report_class_data);
                }

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Report_Class report_class_data = new Report_Class();
                //    report_class_data.report_id = Convert.ToInt32(dr["report_id"]);
                //    report_class_data.report_name = dr["report_name"].ToString();
                //    report_class_data.report_description = dr["report_description"].ToString();
                //    report_class_data.is_deleted = Convert.ToBoolean(dr["is_deleted"]);
                //    mydata.Add(report_class_data);
                //}
                //dt.Dispose();

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
        /// Return the list of report table data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="rid"></param>
        /// Holds the id of role currently it is not in use
        /// <returns></returns>
        public List<Report_Class> getAllReportItems(string schema, int rid)
        {
            try 
            {
                List<Report_Class> mydata = new List<Report_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboReportQryData(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Report_Class report_class_data = new Report_Class();
                    report_class_data.report_id = Convert.ToInt32(rdr["report_id"]);
                    report_class_data.report_name = rdr["report_name"].ToString();
                    report_class_data.report_description = rdr["report_description"].ToString();
                    report_class_data.is_deleted = Convert.ToBoolean(rdr["is_deleted"]);
                    report_class_data.report_status = rdr["status"].ToString();
                    report_class_data.report_user_id = Convert.ToInt32(rdr["report_user_id"]);
                    mydata.Add(report_class_data);
                }

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Report_Class report_class_data = new Report_Class();
                //    report_class_data.report_id = Convert.ToInt32(dr["report_id"]);
                //    report_class_data.report_name = dr["report_name"].ToString();
                //    report_class_data.report_description = dr["report_description"].ToString();
                //    report_class_data.is_deleted = Convert.ToBoolean(dr["is_deleted"]);
                //    report_class_data.report_status = dr["status"].ToString();
                //    mydata.Add(report_class_data);
                //}
                //dt.Dispose();

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
        /// Return the report table data with respective to its query and parameters 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of report
        /// <returns></returns>
        public Report_Class getReportData(string schema, int id)
        {
            try 
            {
                Report_Class mydata = new Report_Class();
                List<Parameter_Class> myparamdata = new List<Parameter_Class>();
                List<Query_Class> myquerydata = new List<Query_Class>();

                string sql_param = ClassFolder.Query_Class.sql_editParamReportQuery(schema, id);
                string sql_query = ClassFolder.Query_Class.sql_editQryReportQuery(schema, id);

                DataTable dt_qry = ClassFolder.Database_Class.OpenQuery(sql_query);
                Connection_Class conclass = new Connection_Class();
                foreach (DataRow dr in dt_qry.Rows)
                {
                    myquerydata.Add(new Query_Class
                    {
                        queryId = Convert.ToInt32(dr["query_id"]),
                        type = dr["type"].ToString(),
                        parameterData = dr["parameters"].ToString(),
                        query_qrydata = dr["report_query"].ToString()

                    });

                    conclass.connection_id = Convert.ToInt32(dr["id"]);
                    conclass.dsn_name = dr["dsn_name"].ToString();
                    conclass.user_id = dr["user_id"].ToString();
                    conclass.password = dr["password"].ToString();
                    conclass.db_schema_name = dr["db_schemaname"].ToString();

                    mydata.report_id = Convert.ToInt32(dr["report_id"]);
                    mydata.report_name = dr["report_name"].ToString();
                    mydata.report_description = dr["report_description"].ToString();
                    mydata.report_path = dr["report_path"].ToString();
                    mydata.report_file = dr["report_file"].ToString();
                    mydata.report_query = dr["report_query"].ToString();
                    mydata.report_user_id = Convert.ToInt32(dr["report_user_id"]);
                    mydata.param_data = myparamdata;
                    mydata.query_data = myquerydata;
                    mydata.connData = conclass;
                }

                MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                obj.DatabaseName = conclass.db_schema_name;
                obj.OdbcName = conclass.dsn_name;
                obj.UserId = conclass.user_id;
                obj.Password = conclass.password;

                DataTable dt = ClassFolder.Database_Class.OpenQuery(sql_param);
                foreach (DataRow dr in dt.Rows)
                {
                    myparamdata.Add(new Parameter_Class
                    {
                        param_id = dr["id"].ToString(),
                        param_label = dr["param_label"].ToString(),
                        param_variable = dr["param_variable"].ToString(),
                        param_type = dr["param_type"].ToString(),
                        param_method = dr["param_method"].ToString(),
                        param_query = dr["param_query"].ToString()

                    });
                }
                dt.Dispose();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Report_Class> getAllReportItem(string schema)
        {
            try 
            {
                List<Report_Class> mydata = new List<Report_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_AllReportData(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    Report_Class report_class_data = new Report_Class();
                    report_class_data.report_id = Convert.ToInt32(rdr["report_id"]);
                    report_class_data.report_name = rdr["report_name"].ToString();
                    mydata.Add(report_class_data);
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