using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class DBQueryClass
    {
        public int queryId { get; set; }
        public string query_name { get; set; }
        public string query_type { get; set; }
        public string query_qrydata { get; set; }
        public string query_script { get; set; }
        public string query_where { get; set; }
        public string query_group_by { get; set; }
        public string query_order_by { get; set; }
        public int con_id { get; set; }
        public int query_user_id { get; set; }

        /// <summary>
        /// Returns the DBQueryClass data mapping with dbconnection table
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the id of query
        /// <returns></returns>
        public DBQueryClass getQueryData(string schema, int id)
        {
            DBQueryClass mydata = new DBQueryClass();
            string sql_query = ClassFolder.Query_Class.sql_editQryQuery(schema, id);
            DataTable dt_qry = ClassFolder.Database_Class.OpenQuery(sql_query);
            foreach (DataRow dr in dt_qry.Rows)
            {
                mydata.queryId = Convert.ToInt32(dr["query_id"]);
                mydata.query_name = dr["function_name"].ToString();
                mydata.query_type = dr["type"].ToString();
                mydata.query_qrydata = dr["qry_script"].ToString();
                //mydata.query_script = dr["query_script"].ToString();
                //mydata.query_where = dr["query_where_cond"].ToString();
                //mydata.query_group_by = dr["query_group_by"].ToString();
                //mydata.query_order_by = dr["query_order_by"].ToString();
                mydata.con_id = Convert.ToInt32(dr["id"]);
                mydata.query_user_id = Convert.ToInt32(dr["query_user_id"]);
            }
            dt_qry.Dispose();
            return mydata;

        }
    }
}