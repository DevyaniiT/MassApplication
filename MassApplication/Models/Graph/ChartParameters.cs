using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models.Graph
{
    public class ChartParameters
    {
        public string chart_selected_params { get; set; }
        public string chart_selected_params_edit { get; set; }
        public string chart_selected_keyvalue_params { get; set; }
        public string chart_type_edit { get; set; }
        public string chart_type_name_edit { get; set; }

        public string chart_selected_type { get; set; }


        public string labelplotvalues { get; set; }
        public string colorcodevalues { get; set; }
        public string labelplotname { get; set; }
        public string labelAlignment { get; set; }

        public int chart_type_id { get; set; }
        public string charttype { get; set; }
        public string ChartParams { get; set; }

        public int chart_bound { get; set; }

        public string image_path { get; set; }

        public string description { get; set; }
        public string kpi_data_name { get; set; }
        public int kpi_data_id { get; set; }

        public string icon_path { get; set; }

        public bool IsChecked { get; set; }
        public int kpi_user_type_id { get; set; }

        public List<ChartParameters> chartlist { get; set; }
        public List<GetQueriesData> Listquery { get; set; }

        public List<KeyValuePair<string, string[]>> keyvalue_tuple { get; set; }
        public string json_tuple { get; set; }
        public List<Dictionary<string, string>> Dictionary_tuple { get; set; }

        public DataTable data_t = new DataTable();
        public List<string> data_columns = new List<string>();

        public int Active_queryID { get; set; }
    }

}