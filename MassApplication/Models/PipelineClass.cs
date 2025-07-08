using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class PipelineClass
    {
        public int pipelinemst_id { get; set; }
        public string pipeline_name { get; set; }
        public string pipeline_shortname { get; set; }
        public string frm_station { get; set; }
        public string t_station { get; set; }
        public string modified_byy { get; set; }
        public string modified_on { get; set; }

        /// <summary>
        /// Returns the pipeline data
        /// </summary>
        /// <param name="objData"></param>
        /// Holds the json data of pipeline table
        /// <returns></returns>
        public PipelineClass getPipelineData(dynamic objData)
        {
            try
            {
                PipelineClass mydata = new PipelineClass();
                mydata.pipelinemst_id = objData["pipelinemst_id"]; 
                mydata.pipeline_name = objData["pipeline_name"]; 
                mydata.pipeline_shortname = objData["pipeline_shortname"]; 
                mydata.frm_station = objData["frm_station"];
                mydata.t_station = objData["t_station"];
                mydata.modified_byy = objData["modified_byy"];
                mydata.modified_on = objData["modified_on"];

                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}