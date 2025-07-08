using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class ResultJsonData
    {
       public string keydata { get; set; }
        public string []valdata { get; set; }

        public List<ResultJsonData> resultjsondata { get; set; }
    }
}