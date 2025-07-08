using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Query_Class
    {
        public int queryId { get; set; }
        public string query_name { get; set; }
        public string type { get; set; }
        public string parameterData { get; set; }
        public string query_qrydata { get; set; }
        public string query_script { get; set; }
        public string query_where { get; set; }
        public string query_group_by { get; set; }
        public string query_order_by { get; set; }

        /// <summary>
        /// Returns the list of dashboard query class data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public List<Query_Class> getAllRoleWiseQueryItems(string schema)
        {
            try
            {
                List<Query_Class> mydata = new List<Query_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string query_sql = ClassFolder.Query_Class.sql_queryDataOnly(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(query_sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Query_Class query_class_data = new Query_Class();
                    query_class_data.queryId = int.Parse(rdr["query_id"].ToString());
                    query_class_data.query_name = rdr["function_name"].ToString();
                    mydata.Add(query_class_data);
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