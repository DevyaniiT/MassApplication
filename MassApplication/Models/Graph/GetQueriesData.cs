using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models.Graph
{
    public class GetQueriesData

    {
        public string function_name { get; set; }

        public string qry_script { get; set; }

        public bool IsChecked { get; set; }

        public int dbconnection_id { get; set; }

        public int query_id { get; set; }

        public string type { get; set; }

        public string connection_name { get; set; }

        public string dsn_name { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public string schemaname { get; set; }

        //public List<ChartParameters> chartparam_tuple { get; set; }
        public List<GetQueriesData> Listquery { get; set; }


    }
}