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
    public class GC_Class
    {
        public int gc_id { get; set; }
        public string gc_name { get; set; }
        public string abbrevation_name { get; set; }
        public int gc_network_id { get; set; }
        public string gc_network { get; set; }
        public string gc_type { get; set; }
        public int gc_meter_type_id { get; set; }
        public string gc_meter_type { get; set; }
        public int gc_station_id { get; set; }
        public string gc_station_name { get; set; }
        public string gc_chords { get; set; }
        public int gc_count { get; set; }
        public string gc_created_by { get; set; }
        public string gc_created_on { get; set; }
        public string gc_modified_by { get; set; }
        public string gc_modified_on { get; set; }
        public string gc_date { get; set; }
        public List<Dashboard_kpi_class> kpiItems { get; set; }

        /// <summary>
        /// Returns the GC_Class data
        /// Mapping gc data with its KPIs 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="name"></param>
        /// Holds the name of abbrevation
        /// <param name="data_val"></param>
        /// Holds the GC_Class data
        /// <param name="gc_id"></param>
        /// Holds the id of gc
        /// <param name="start_date"></param>
        /// Holds the start date
        /// <param name="end_date"></param>
        /// Holds the end date
        /// <returns></returns>
        public GC_Class getGCDynamicItem(string schema, string name, GC_Class data_val, int gc_id, string start_date, string end_date)
        {
            try 
            {
                GC_Class mydata = new GC_Class();
                List<Dashboard_kpi_class> mykpidata = new List<Dashboard_kpi_class>();
                string query_sql = ClassFolder.Query_Class.GetDiagnosticKpiMapQuery(schema, name);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(query_sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //DataTable resultdtt = Dashboard_kpi_class.ChartDynamicGCQueryData(schema, Convert.ToInt32(dr["query_id"]), gc_id, start_date, end_date, dr["type"].ToString());
                    DataTable resultdtt = Dashboard_kpi_class.GetDataTableData(schema, Convert.ToInt32(dr["query_id"]), 0, 0, gc_id, start_date, end_date, null);
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

                    mydata.gc_id = data_val.gc_id;
                    mydata.abbrevation_name = dr["abbrevation_name"].ToString();
                    mydata.gc_name = data_val.gc_name;
                    mydata.gc_type = data_val.gc_type;
                    mydata.gc_network = data_val.gc_network;
                    mydata.gc_station_name = data_val.gc_station_name;
                    mydata.gc_date = start_date;
                    mydata.kpiItems = mykpidata;

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
        /// Returns the GC_Class data
        /// Mapping gc data with its KPIs 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="name"></param>
        /// Holds the name of abbrevation
        /// <param name="gc_id"></param>
        /// Holds the id of gc
        /// <param name="gc_name"></param>
        /// Holds the gc name
        /// <param name="gc_network"></param>
        /// Holds the gc network
        /// <param name="gc_station"></param>
        /// Holds the gc station
        /// <param name="start_date"></param>
        /// Holds the start date
        /// <returns></returns>
        public GC_Class getGCDynamicItems(string schema, string name, int gc_id, string gc_name, string gc_network, string gc_station, string start_date)
        {
            try
            {
                GC_Class mydata = new GC_Class();
                List<Dashboard_kpi_class> mykpidata = new List<Dashboard_kpi_class>();
                string query_sql = ClassFolder.Query_Class.GetDiagnosticKpiMapQuery(schema, name);
                DataTable dt = ClassFolder.Database_Class.OpenQuery(query_sql);
                foreach (DataRow dr in dt.Rows)
                {
                    //DataTable resultdtt = Dashboard_kpi_class.ChartDynamicGCQueryData(schema, Convert.ToInt32(dr["query_id"]), gc_id, start_date, start_date, dr["type"].ToString());
                    DataTable resultdtt = Dashboard_kpi_class.GetDataTableData(schema, Convert.ToInt32(dr["query_id"]), 0, 0, gc_id, start_date, start_date, null);
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

                    mydata.gc_id = gc_id;
                    mydata.abbrevation_name = name;
                    mydata.gc_name = gc_name;
                    mydata.gc_network = gc_network;
                    mydata.gc_station_name = gc_station;
                    mydata.gc_date = start_date;
                    mydata.kpiItems = mykpidata;

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