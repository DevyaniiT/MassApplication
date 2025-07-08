using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class CrystalConnection_Class
    {
        public static string odbc_name = ConfigurationManager.AppSettings["ODBCName"];
        public static string server_name = ConfigurationManager.AppSettings["server"];
        public static string database_name = ConfigurationManager.AppSettings["database"];
        public static string user_name = ConfigurationManager.AppSettings["user"];
        public static string pwd = ConfigurationManager.AppSettings["password"];
        //public static Hashtable hashtableQuery = new Hashtable();

        public static string server
        {
            get { return server_name; }
            set { server_name = value; }
        }
        public static string database
        {
            get { return database_name; }
            set { database_name = value; }
        }
        public static string user
        {
            get { return user_name; }
            set { user_name = value; }
        }
        public static string password
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }
}