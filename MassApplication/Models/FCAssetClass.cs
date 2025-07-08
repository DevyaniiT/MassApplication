using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class FCAssetClass
    {
        public int fc_id { get; set; }
        public string fc_name { get; set; }
        public int fc_network_id { get; set; }
        public string fc_network { get; set; }
        public string fc_type { get; set; }
        public int fc_meter_type_id { get; set; }
        public string fc_meter_type { get; set; }
        public int fc_station_id { get; set; }
        public string fc_station_name { get; set; }
        public int fc_gc_id { get; set; }
        public string fc_gc_name { get; set; }
        public string fc_chords { get; set; }
        public int fc_count { get; set; }
        public string fc_created_by { get; set; }
        public string fc_created_on { get; set; }

        /// <summary>
        /// Returns the fc asset data
        /// </summary>
        /// <param name="objData"></param>
        /// Holds the json data of fc table
        /// <returns></returns>
        public FCAssetClass getFCAssetData(dynamic objData)
        {
            try
            {
                FCAssetClass mydata = new FCAssetClass();
                mydata.fc_name = objData[0]["assetname"];
                mydata.fc_id = objData[0]["assetid"];
                mydata.fc_network_id = objData[0]["assetregion"];
                mydata.fc_network = objData[0]["assetregionname"];
                mydata.fc_type = objData[0]["assettype"];
                mydata.fc_meter_type_id = objData[0]["meter_type"];
                mydata.fc_station_id = objData[0]["assetstation"];
                mydata.fc_station_name = objData[0]["stationname"];
                mydata.fc_gc_id = objData[0]["gcid"];
                mydata.fc_gc_name = objData[0]["gcname"];
                mydata.fc_chords = objData[0]["chords"];
                mydata.fc_count = objData[0]["count"];
                mydata.fc_created_by = objData[0]["created_byy"];
                mydata.fc_created_on = objData[0]["created_on"];

                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}