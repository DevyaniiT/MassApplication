using MassApplication.Models.Graph;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Diagnostic_Class
    {
        public int diagnostic_id { get; set; }
        public string abbrevation_name { get; set; }
        public string pipeline_name { get; set; }
        public string asset_name { get; set; }
        public string station_name { get; set; }
        public string consumer_name { get; set; }
        public string meter_type { get; set; }
        public string maintenance_name { get; set; }
        public int stream_no { get; set; }
        public int asset_id { get; set; }
        public int mmst_id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int is_deleted { get; set; }
        public string ghg { get; set; }
        public string status { get; set; }
        public string health { get; set; }
        public string calibration { get; set; }
        public string transducer_failure { get; set; }
        public string detection_problem { get; set; }
        public string ultrasonic_noise { get; set; }
        public string process_condition_pressure { get; set; }
        public string process_condition_temprature { get; set; }
        public string fouling { get; set; }
        public string change_in_flow_profile { get; set; }
        public string high_velocity { get; set; }
        public string diagnostic_date { get; set; }
        public List<Dashboard_kpi_class> kpiListItem { get; set; }

        /// <summary>
        /// Returns the Diagnostic_Class data
        /// Mapping diagnostic data with its KPIs 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="name"></param>
        /// Holds the name of abbrevation
        /// <param name="data_val"></param>
        /// Holds the Diagnostic_Class data
        /// <param name="asset_id"></param>
        /// Holds the id of asset
        /// <param name="stream_no"></param>
        /// Holds the number of stream
        /// <param name="start_date"></param>
        /// Holds the start date
        /// <param name="end_date"></param>
        /// Holds the end date
        /// <returns></returns>
        public Diagnostic_Class getDiagnosticDynamicItem(string schema, string name, Diagnostic_Class data_val, int asset_id, int stream_no, string start_date, string end_date)
        {
            try 
            {
                Diagnostic_Class mydata = new Diagnostic_Class();
                List<Dashboard_kpi_class> mykpidata = new List<Dashboard_kpi_class>();
                string query_sql = ClassFolder.Query_Class.GetDiagnosticKpiMapQuery(schema, name);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(query_sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //DataTable resultdtt = Dashboard_kpi_class.ChartDynamicQueryData(schema, Convert.ToInt32(dr["query_id"]), asset_id, stream_no, start_date, end_date, dr["type"].ToString());
                    DataTable resultdtt = Dashboard_kpi_class.GetDataTableData(schema, Convert.ToInt32(dr["query_id"]), asset_id, stream_no, 0, start_date, end_date, null);
                    if (dr["type"].ToString().ToLower() == "diagnostic")
                    {
                        DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt);
                        //resultdtt = new DataTable();
                        resultdtt = dtNew.Copy();
                    }

                    string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    mykpidata.Add(new Dashboard_kpi_class
                    {
                        kpi_id = Convert.ToInt32(dr["kpi_id"]),
                        kpi_query_id = Convert.ToInt32(dr["query_id"]),
                        chart_type_id = Convert.ToInt32(dr["chart_type_id"]),
                        kpi_name = dr["kpi_name"].ToString(),
                        chart_type = dr["chart_type"].ToString(),
                        chart_params = dr["chart_params"].ToString(),
                        chart_bounds = dr["chart_bounds"].ToString(),
                        chart_image = dr["chart_image"].ToString(),
                        query_type = dr["type"].ToString(),
                        json_tuple = convertJson
                    });

                    mydata.diagnostic_id = Convert.ToInt32(dr["id"]);
                    mydata.abbrevation_name = dr["abbrevation_name"].ToString();
                    mydata.consumer_name = data_val.consumer_name;
                    mydata.meter_type = data_val.meter_type;
                    mydata.stream_no = data_val.stream_no;
                    mydata.maintenance_name = data_val.maintenance_name;
                    mydata.asset_name = data_val.asset_name;
                    mydata.pipeline_name = data_val.pipeline_name;
                    mydata.station_name = data_val.station_name;
                    mydata.make = data_val.make;
                    mydata.model = data_val.model;
                    mydata.asset_id = data_val.asset_id;
                    mydata.mmst_id = data_val.mmst_id;
                    mydata.diagnostic_date = start_date;
                    mydata.kpiListItem = mykpidata;

                }
                dt.Dispose();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the Diagnostic_Class data
        /// Mapping diagnostic data with its KPIs  
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="name"></param>
        /// Holds the name of abbrevation
        /// <param name="asset_id"></param>
        /// Holds the id of asset
        /// <param name="consumer_name"></param>
        /// Holds the name of consumer
        /// <param name="meter_type"></param>
        /// Holds the meter type name
        /// <param name="stream_no"></param>
        /// Holds the number of stream
        /// <param name="maintenance_name"></param>
        /// Holds the name of maintenance
        /// <param name="pipeline_name"></param>
        /// Holds the name of pipeline
        /// <param name="start_date"></param>
        /// Holds the start date
        /// <returns></returns>
        public Diagnostic_Class getDiagnosticDynamicItems(string schema, string name, int asset_id, string consumer_name, string meter_type, int stream_no, string maintenance_name, string pipeline_name, string start_date)
        {
            try
            {
                Diagnostic_Class mydata = new Diagnostic_Class();
                List<Dashboard_kpi_class> mykpidata = new List<Dashboard_kpi_class>();
                string query_sql = ClassFolder.Query_Class.GetDiagnosticKpiMapQuery(schema, name);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(query_sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //DataTable resultdtt = Dashboard_kpi_class.ChartDynamicQueryData(schema, Convert.ToInt32(dr["query_id"]), asset_id, stream_no, start_date, start_date, dr["type"].ToString());
                    DataTable resultdtt = Dashboard_kpi_class.GetDataTableData(schema, Convert.ToInt32(dr["query_id"]), asset_id, stream_no, 0, start_date, start_date, null);
                    if (dr["type"].ToString().ToLower() == "diagnostic")
                    {
                        DataTable dtNew = ClassFolder.TableLayoutClass.CreateNewTable(resultdtt);
                        //resultdtt = new DataTable();
                        resultdtt = dtNew.Copy();
                    }

                    string convertJson = GetScriptData.DataTableToJSONWithJavaScriptSerializer(resultdtt);

                    mykpidata.Add(new Dashboard_kpi_class
                    {
                        kpi_id = Convert.ToInt32(dr["kpi_id"]),
                        kpi_query_id = Convert.ToInt32(dr["query_id"]),
                        chart_type_id = Convert.ToInt32(dr["chart_type_id"]),
                        kpi_name = dr["kpi_name"].ToString(),
                        chart_type = dr["chart_type"].ToString(),
                        chart_params = dr["chart_params"].ToString(),
                        chart_bounds = dr["chart_bounds"].ToString(),
                        chart_image = dr["chart_image"].ToString(),
                        query_type = dr["type"].ToString(),
                        json_tuple = convertJson
                    });

                    //mydata.diagnostic_id = Convert.ToInt32(dr["id"]);
                    mydata.abbrevation_name = abbrevation_name;
                    mydata.consumer_name = consumer_name;
                    mydata.meter_type = meter_type;
                    mydata.stream_no = stream_no;
                    mydata.maintenance_name = maintenance_name;
                    mydata.pipeline_name = pipeline_name;
                    //mydata.asset_name = asset_name;
                    //mydata.station_name = station_name;
                    //mydata.make = data_val.make;
                    //mydata.model = data_val.model;
                    //mydata.mmst_id = data_val.mmst_id;
                    mydata.asset_id = asset_id;
                    mydata.diagnostic_date = start_date;
                    mydata.kpiListItem = mykpidata;

                }
                dt.Dispose();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}