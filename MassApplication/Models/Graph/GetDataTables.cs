using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class GetDataTables
    {
        public string keys { get; set; }
        public string Value { get; set; }

        public List<Dictionary<string, string>> Dictionary_tuple { get; set; }
    }
}