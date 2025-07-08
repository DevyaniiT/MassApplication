using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class DashboardMap_Class
    {
        public int dashboard_id { get; set; }
        public string dashboard_name { get; set; }
        public string dashboard_user_type { get; set; }
        public string dashboard_created_by { get; set; }
        public bool role_dashboard_create { get; set; }
        public bool role_dashboard_read { get; set; }
        public bool role_dashboard_update { get; set; }
        public bool role_dashboard_delete { get; set; }
    }
}