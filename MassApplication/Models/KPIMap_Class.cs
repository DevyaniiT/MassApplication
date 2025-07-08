using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class KPIMap_Class
    {
        public int kpi_id { get; set; }
        public string kpi_name { get; set; }
        public string kpi_user_type { get; set; }
        public string kpi_created_by { get; set; }
        public bool role_kpi_create { get; set; }
        public bool role_kpi_read { get; set; }
        public bool role_kpi_update { get; set; }
        public bool role_kpi_delete { get; set; }
    }
}