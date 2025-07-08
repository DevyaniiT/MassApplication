using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MASSODBC;

namespace MassApplication.ClassFolder
{
    public class Database_Class
    {
        /// <summary>
        /// Returns the connection string
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            string constring = "";
            if (ConfigurationManager.ConnectionStrings["DatabaseServer"] != null)
            {
                constring = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
            }
            return constring;
        }

        /// <summary>
        /// Returns the DataTable 
        /// </summary>
        /// <param name="sql"></param>
        /// Defines the sql query
        /// <returns></returns>
        public static DataTable OpenQuery(string sql)
        {
            DataTable dt;
            try
            {
                string constr = GetConnectionString();
                if (constr.Length == 0)
                {
                    throw new Exception("Connection string not defined");
                }
                dt = new DataTable();
                NpgsqlConnection newconn = new NpgsqlConnection(constr);
                newconn.Open();
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter(sql, newconn);
                ad.Fill(dt);
                ad.Dispose();
                ad = null;
                newconn.Close();
                newconn = null;
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Execute the Query
        /// </summary>
        /// <param name="sql"></param>
        /// Defines the sql query
        /// <returns></returns>
        public static bool ExecuteQuery(string sql)
        {
            try
            {
                string constr = GetConnectionString();
                if (constr.Length == 0)
                {
                    throw new Exception("Connection string not defined");
                }
                NpgsqlConnection newconn = new NpgsqlConnection(constr);
                NpgsqlCommand ngcmdParameter = new NpgsqlCommand();
                ngcmdParameter.Connection = newconn;
                newconn.Open();
                ngcmdParameter.CommandText = sql;
                ngcmdParameter.ExecuteNonQuery();
                newconn.Close();
                newconn = null;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable OpenQueryCommandText(string sql)
        {
            DataTable dt;
            try
            {
                string constr = GetConnectionString();
                if (constr.Length == 0)
                {
                    throw new Exception("Connection string not defined");
                }
                NpgsqlConnection newconn = new NpgsqlConnection(constr);
                NpgsqlCommand ngcmdParameter = new NpgsqlCommand();
                ngcmdParameter.Connection = newconn;
                newconn.Open();
                ngcmdParameter.CommandText = sql;
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter(ngcmdParameter);
                dt = new DataTable();
                ad.Fill(dt);
                ad.Dispose();
                ad = null;
                newconn.Close();
                newconn = null;
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="query_sql"></param>
        /// <returns></returns>
        public static NpgsqlDataReader OpenReader(string query_sql)
        {
            try
            {
                string constr = GetConnectionString();
                if (constr.Length == 0)
                {
                    throw new Exception("Connection string not defined");
                }
                NpgsqlConnection con = new NpgsqlConnection(constr);
                con.Open();
                NpgsqlCommand cmd;
                NpgsqlDataReader rdr;
                cmd = new NpgsqlCommand(query_sql, con);
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Execute the query and return the id of a particular query
        /// </summary>
        /// <param name="sql"></param>
        /// Defines the sql query
        /// <returns></returns>
        public static int ExecuteScalar(string sql)
        {
            int numValue = 0;
            try
            {
                string constr = GetConnectionString();
                if (constr.Length == 0)
                {
                    throw new Exception("Connection string not defined");
                }
                NpgsqlConnection newconn = new NpgsqlConnection(constr);
                newconn.Open();
                NpgsqlCommand ngcmdParameter;
                ngcmdParameter = new NpgsqlCommand(sql, newconn);
                numValue = Convert.ToInt32(ngcmdParameter.ExecuteScalar());
                newconn.Close();
                return numValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the DataTable
        /// Used for ODBC Connection
        /// </summary>
        /// <param name="sql"></param>
        /// Defines the query
        /// <param name="odbc_name"></param>
        /// Defines the ODBC Name
        /// <param name="user_id"></param>
        /// Defines the user name
        /// <param name="pwd"></param>
        /// Defines the password
        /// <param name="dbname"></param>
        /// Defines the database
        /// <returns></returns>
        public static DataTable OpenODBCQuery(string sql, string odbc_name, string user_id, string pwd, string dbname)
        {
            DataTable dt;
            try
            {
                
                MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                obj.DatabaseName = dbname;
                obj.OdbcName = odbc_name;
                obj.UserId = user_id;
                obj.Password = pwd;
                dt = new DataTable();
                dt = obj.OpenTable(sql, "query");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used for checking the ODBC Connection
        /// </summary>
        /// <param name="odbc_name"></param>
        /// Defines the ODBC Name
        /// <param name="user_id"></param>
        /// Defines the user name
        /// <param name="pwd"></param>
        /// Defines the password
        /// <param name="dbname"></param>
        ///  Defines the database
        /// <returns></returns>
        public static string CheckODBC(string odbc_name, string user_id, string pwd, string dbname)
        {
            try 
            {
                string output = "";
                MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                ClassFolder.Error.GetErroronNotepad("---MASSODBC CREATED---");
                obj.OdbcName = odbc_name;
                obj.DatabaseName = dbname;
                obj.UserId = user_id;
                obj.Password = pwd;
                ClassFolder.Error.GetErroronNotepad("OdbcName:" + obj.OdbcName);
                ClassFolder.Error.GetErroronNotepad("DatabaseName:" + obj.DatabaseName);
                ClassFolder.Error.GetErroronNotepad("UserId:" + obj.UserId);
                ClassFolder.Error.GetErroronNotepad("Password:" + obj.Password);
                output = MASSODBC.MASSODBC.GetConnection(obj.OdbcName, obj.DatabaseName, obj.UserId, obj.Password);
                ClassFolder.Error.GetErroronNotepad("OUTPUT:" + output);
                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}