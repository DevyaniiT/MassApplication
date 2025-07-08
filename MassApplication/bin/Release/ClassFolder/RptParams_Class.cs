using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class RptParams_Class
    {
        public static string qry;
        public static string query_val
        {
            get { return qry; }
            set { qry = value; }
        }
    }
}