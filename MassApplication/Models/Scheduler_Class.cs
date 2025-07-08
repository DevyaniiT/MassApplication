using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;

namespace MassApplication.Models
{
    public class Scheduler_Class
    {
        public int email_id { get; set; }
        public int job_id { get; set; }
        public string from_email { get; set; }
        public string to_email { get; set; }
        public string cc_email { get; set; }
        public string bcc_email { get; set; }
        public string body_email { get; set; }
        public string sub_email { get; set; }
        public string schedule_email { get; set; }
        public string job_name { get; set; }
        public string job_description { get; set; }
        public string duration { get; set; }
        public string created_by { get; set; }
        public string schedule_status { get; set; }
        public string mode { get; set; }
        public int is_deleted { get; set; }
        public string schedule_start_date { get; set; }
        public string recurrenceItems { get; set; }
        public string frequencyDays { get; set; }
        public int period_id { get; set; }
        public string param_label { get; set; }
        public string param_variable { get; set; }
        public string param_networks { get; set; }
        public string param_network_selection { get; set; }
        public string param_section_selection { get; set; }
        public string param_network_variable { get; set; }
        public string param_section_variable { get; set; }
        public int job_report_id { get; set; }
        public string job_report_name { get; set; }
        public string job_report_url { get; set; }
        public string job_report_arguments { get; set; }
        public string job_from_date { get; set; }
        public string job_to_date { get; set; }
        public string[] job_dateData { get; set; }

        /// <summary>
        /// Returns scheduler data 
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="id"></param>
        /// Holds the job id
        /// <returns></returns>
        public Scheduler_Class getSchedulerData(string schema, int id)
        {
            Scheduler_Class mydata = new Scheduler_Class();
            string sql_query = ClassFolder.Query_Class.sql_editSchedularQuery(schema, id);
            DataTable dt_qry = ClassFolder.Database_Class.OpenQuery(sql_query);
            foreach (DataRow dr in dt_qry.Rows)
            {
                mydata.job_id = Convert.ToInt32(dr["id"]);
                mydata.job_name = dr["job_name"].ToString();
                mydata.job_description = dr["job_description"].ToString();
                mydata.job_report_id = Convert.ToInt32(dr["report_id"]);
                mydata.job_report_name = dr["job_path_filename"].ToString();
                mydata.recurrenceItems = dr["job_timeinterval"].ToString();
                mydata.period_id = Convert.ToInt32(dr["period_id"]);
                //mydata.query_qrydata = dr["job_lastexetime"].ToString();
                //mydata.schedule_start_date = Convert.ToDateTime(dr["job_nextexetime"]).ToString("yyyy-MM-dd'T'HH:mm:ss");08/04/2022 03:15:00
                mydata.schedule_start_date = Convert.ToDateTime(dr["job_nextexetime"]).ToString("yyyy-MM-dd HH:mm:ss");
                mydata.duration = dr["job_status"].ToString();
                mydata.schedule_status = dr["status"].ToString();
                mydata.email_id = Convert.ToInt32(dr["job_id"]);
                mydata.to_email = dr["email_to"].ToString();
                mydata.cc_email = dr["email_cc"].ToString();
                mydata.bcc_email = dr["email_bcc"].ToString();
                mydata.sub_email = dr["subject"].ToString();
                mydata.body_email = dr["body"].ToString();
                mydata.frequencyDays = dr["frequency_days"].ToString();
                mydata.job_report_arguments = dr["arguments"].ToString();
                mydata.mode = dr["job_type"].ToString();
                mydata.job_report_url = dr["url"].ToString();
                mydata.is_deleted = Convert.ToInt32(dr["is_deleted"]);
            }
            dt_qry.Dispose();

            return mydata;

        }
       
        /// <summary>
        /// Not in use
        /// Older version for displaying email schedular data in grid format
        /// Using older table emailerjobs_scheduled
        /// Currently we are using pipeline_job_master table
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public List<Scheduler_Class> getAllScheduleItems(string schema)
        {
            try
            {
                List<Scheduler_Class> mydata = new List<Scheduler_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_ComboScheduleQry(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Scheduler_Class sc = new Scheduler_Class();
                    sc.job_id = Convert.ToInt32(rdr["schdr_jobid"]);
                    sc.email_id = Convert.ToInt32(rdr["id"]);
                    sc.job_name = rdr["job_name"].ToString();
                    sc.job_description = rdr["job_description"].ToString();
                    sc.duration = rdr["duration"].ToString();
                    sc.is_deleted = Convert.ToInt32(rdr["id_deleted"]);
                    //sc.is_deleted = Convert.ToBoolean(rdr["id_deleted"]);
                    sc.created_by = rdr["created_by"].ToString();
                    sc.schedule_status = rdr["status"].ToString();

                    mydata.Add(sc);
                }
                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}