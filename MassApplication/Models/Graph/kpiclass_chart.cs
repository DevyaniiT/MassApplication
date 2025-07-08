using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models.Graph
{
    public class kpiclass_chart
    {
        public int chart_id { get; set; }
        public int kpi_ID { get; set; }
        public int kpi_query_ID { get; set; }
        public string kpi_name { get; set; }
        public string kpi_description { get; set; }
        public string kpi_position { get; set; }
        public string kpi_chart_type { get; set; }
        public string kpi_chart_parameters { get; set; }
        public int chart_bound { get; set; }
        public bool is_deleted { get; set; }
        public bool system_kpi { get; set; }
        public int kpi_user_id { get; set; }
    }
}