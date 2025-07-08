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
    public class Parameter_Class
    {
        public string param_id { get; set; }
        public int report_id { get; set; }
        public int queryId { get; set; }
        public string param_label { get; set; }
        public string param_variable { get; set; }
        public string param_type { get; set; }
        public string param_method { get; set; }
        public string param_query { get; set; }
        public string param_data_values { get; set; }

        /// <summary>
        /// Returns the inputs of report_query_param using its report id
        /// This method is called when we load the report 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of report
        /// <returns></returns>
        public List<Parameter_Class> getReportParams(string schema, string id)
        {
            try 
            {
                List<Parameter_Class> myData = new List<Parameter_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboReportParamQryUnOrder(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Parameter_Class pc = new Parameter_Class();
                    pc.param_id = rdr["id"].ToString();
                    pc.report_id = Convert.ToInt32(rdr["report_id"]);
                    pc.param_label = rdr["param_label"].ToString();
                    pc.param_variable = rdr["param_variable"].ToString();
                    pc.param_type = rdr["param_type"].ToString();
                    pc.param_method = rdr["param_method"].ToString();
                    pc.param_query = rdr["param_query"].ToString();

                    myData.Add(pc);
                }
                rdr.Dispose();
                con.Close();

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Parameter_Class pc = new Parameter_Class();
                //    pc.param_id = dr["id"].ToString();
                //    pc.report_id = Convert.ToInt32(dr["report_id"]);
                //    pc.param_label = dr["param_label"].ToString();
                //    pc.param_variable = dr["param_variable"].ToString();
                //    pc.param_type = dr["param_type"].ToString();
                //    pc.param_method = dr["param_method"].ToString();
                //    pc.param_query = dr["param_query"].ToString();

                //    myData.Add(pc);
                //}
                //dt.Dispose();

                return myData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the inputs of report_query_param using its report id
        /// This method is called when we edit schedule the report 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of report
        /// <param name="parameters"></param>
        /// Holds the job report arguments
        /// <returns></returns>
        public List<Parameter_Class> getScheduleReportParams(string schema, string id, string parameters)
        {
            try
            {
                string[] varVal = parameters.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                List<string> val_list = new List<string>();
                foreach (string value1 in varVal)
                {
                    string datavalues = "";
                    datavalues = value1.Replace(";", ",");
                    val_list.Add(datavalues);
                }

                List<Parameter_Class> myData = new List<Parameter_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboReportParamQryUnOrder(schema, id);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                int rdCount = 0;
                while (rdr.Read())
                {
                    Parameter_Class pc = new Parameter_Class();
                    pc.param_id = rdr["id"].ToString();
                    pc.report_id = Convert.ToInt32(rdr["report_id"]);
                    pc.param_label = rdr["param_label"].ToString();
                    pc.param_variable = rdr["param_variable"].ToString();
                    pc.param_type = rdr["param_type"].ToString();
                    pc.param_method = rdr["param_method"].ToString();
                    pc.param_query = rdr["param_query"].ToString();
                    pc.param_data_values = val_list[rdCount];
                    myData.Add(pc);
                    rdCount++;
                }
                rdr.Dispose();
                con.Close();

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Parameter_Class pc = new Parameter_Class();
                //    pc.param_id = dr["id"].ToString();
                //    pc.report_id = Convert.ToInt32(dr["report_id"]);
                //    pc.param_label = dr["param_label"].ToString();
                //    pc.param_variable = dr["param_variable"].ToString();
                //    pc.param_type = dr["param_type"].ToString();
                //    pc.param_method = dr["param_method"].ToString();
                //    pc.param_query = dr["param_query"].ToString();

                //    myData.Add(pc);
                //}
                //dt.Dispose();

                return myData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Parameter_Class> getReportParam(string schema)
        {
            try
            {
                List<Parameter_Class> myData = new List<Parameter_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);
                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboNewReportParamQry(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Parameter_Class pc = new Parameter_Class();
                    pc.param_id = rdr["id"].ToString();
                    pc.report_id = Convert.ToInt32(rdr["report_id"]);
                    pc.param_label = rdr["param_label"].ToString();
                    pc.param_variable = rdr["param_variable"].ToString();
                    pc.param_type = rdr["param_type"].ToString();
                    pc.param_method = rdr["param_method"].ToString();
                    pc.param_query = rdr["param_query"].ToString();

                    myData.Add(pc);
                }
                rdr.Dispose();
                con.Close();

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Parameter_Class pc = new Parameter_Class();
                //    pc.param_id = dr["id"].ToString();
                //    pc.report_id = Convert.ToInt32(dr["report_id"]);
                //    pc.param_label = dr["param_label"].ToString();
                //    pc.param_variable = dr["param_variable"].ToString();
                //    pc.param_type = dr["param_type"].ToString();
                //    pc.param_method = dr["param_method"].ToString();
                //    pc.param_query = dr["param_query"].ToString();

                //    myData.Add(pc);
                //}
                //dt.Dispose();

                return myData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}