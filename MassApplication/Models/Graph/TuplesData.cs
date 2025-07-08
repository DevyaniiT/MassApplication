using MassApplication.Models.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class TuplesData
    {
        public List<ChartParameters> chartparam_tuple { get; set; }
        public List<KeyValuePair<string, string[]>> keyvalue_tuple { get; set; }
        public string json_tuple { get; set; }
        public List<Dictionary<string, string>>  Dictionary_tuple {get;set;}

        public int Active_queryID { get; set; }

}
}