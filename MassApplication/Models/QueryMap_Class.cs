using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class QueryMap_Class
    {
        public int query_id { get; set; }
        public string query_name { get; set; }
        public string query_user_type { get; set; }
        public string query_created_by { get; set; }
        public bool role_query_create { get; set; }
        public bool role_query_read { get; set; }
        public bool role_query_update { get; set; }
        public bool role_query_delete { get; set; }
    }
}