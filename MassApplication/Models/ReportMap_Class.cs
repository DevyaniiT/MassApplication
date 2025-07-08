using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class ReportMap_Class
    {
        public int report_id { get; set; }
        public string report_name { get; set; }
        public string report_user_type { get; set; }
        public string report_created_by { get; set; }
        public bool role_report_create { get; set; }
        public bool role_report_read { get; set; }
        public bool role_report_update { get; set; }
        public bool role_report_delete { get; set; }
    }
}