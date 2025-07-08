using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Connection_Class
    {
        public int connection_id { get; set; }
        public string connection_name { get; set; }
        public string dsn_name { get; set; }
        public string user_id { get; set; }
        public string password { get; set; }
        public string db_schema_name { get; set; }

        /// <summary>
        /// Returns the list of connection class data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public List<Connection_Class> getAllConnectionItems(string schema)
        {
            List<Connection_Class> mydata = new List<Connection_Class>();

            string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
            NpgsqlConnection con = new NpgsqlConnection(constr);
            string sql = ClassFolder.Query_Class.sql_ComboConnectionQry(schema);
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Connection_Class conclass = new Connection_Class();
                conclass.connection_id = Convert.ToInt32(rdr["id"]);
                conclass.connection_name = rdr["connection_name"].ToString();
                conclass.dsn_name = rdr["dsn_name"].ToString();
                conclass.user_id = rdr["user_id"].ToString();
                conclass.password = rdr["password"].ToString();
                conclass.db_schema_name = rdr["db_schemaname"].ToString();

                mydata.Add(conclass);
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
            //    Connection_Class conclass = new Connection_Class();
            //    conclass.connection_id = Convert.ToInt32(rdr["id"]);
            //    conclass.connection_name = Convert.ToInt32(rdr["connection_name"]);
            //    conclass.dsn_name = rdr["dsn_name"].ToString();
            //    conclass.user_id = rdr["user_id"].ToString();
            //    conclass.password = rdr["password"].ToString();
            //    conclass.db_schema_name = rdr["db_schemaname"].ToString();

            //    mydata.Add(conclass);
            //}
            //dt.Dispose();

            rdr.Dispose();
            con.Close();
            return mydata;

        }

        /// <summary>
        /// Returns the connection class data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <param name="id"></param>
        /// Holds the id of connection
        /// <returns></returns>
        public Connection_Class getConnectionData(string schema, int id)
        {
            Connection_Class conclass = new Connection_Class();
            string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
            NpgsqlConnection con = new NpgsqlConnection(constr);
            NpgsqlCommand cmd;
            NpgsqlDataAdapter sda;
            con.Open();

            string conn_sql = ClassFolder.Query_Class.sql_editConnectionQuery(schema, id);
            cmd = new NpgsqlCommand(conn_sql, con);
            sda = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            
            foreach (DataRow dr in dt.Rows)
            {
                conclass.connection_id = Convert.ToInt32(dr["id"]);
                conclass.connection_name = dr["connection_name"].ToString();
                conclass.dsn_name = dr["dsn_name"].ToString();
                conclass.user_id = dr["user_id"].ToString();
                conclass.password = dr["password"].ToString();
                conclass.db_schema_name = dr["db_schemaname"].ToString();
            }

            dt.Dispose();
            sda.Dispose();
            con.Close();
            return conclass;
        }

    }
}