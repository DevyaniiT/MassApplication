using CrystalDecisions.CrystalReports.Engine;
using MassApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Controllers
{
    public class ReportController : BaseController
    {
        NpgsqlConnection con = new NpgsqlConnection();
        public static string rpt_label, rpt_values, uploadedFile;
        public static string reportpath, report_name, qry, function_script, val_query;
        public static Hashtable hashtabledata = new Hashtable();
        public static Hashtable hashquerydata = new Hashtable();
        public static Hashtable hashvalue = new Hashtable();
        public static List<string> val = null;
        public static List<string> variableList = null;
        public static int connection_ID = 0;
        public static string rpt_path = ConfigurationManager.AppSettings["reportPath"];
        string img_path = ConfigurationManager.AppSettings["rptImgPath"];
        public static HttpPostedFileBase file_upload;

        /// <summary>
        /// Not in used
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            hashtabledata.Clear();
            hashquerydata.Clear();
            hashvalue.Clear();
            if (ClassFolder.Login_Class.access_token == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No network assign...!'); window.location.href = 'HomePage'; </script>");
            }
            else
            {
                return View();
            }

        }
        
        /// <summary>
        /// This is called when we want to add new report in the report_mst table
        /// Now, not in used
        /// </summary>
        /// <returns></returns>
        public ActionResult TableIndex()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            Report_Class db = new Report_Class();
            List<Report_Class> menuItems = db.getReportItems(schemapath, roleId);
            //ViewBag.items = menuItems;
            return PartialView("_partialReportTableData", menuItems);
        }
        
        /// <summary>
        /// This method is called when we click on the load icon in the report grid
        /// then a popup window shows which have some parameter fields that map with the report id
        /// </summary>
        /// <param name="report_id"></param>
        /// This parameter is used to fetch all the parameter field of related report data
        /// <returns></returns>
        public ActionResult InputSqlData(int report_id)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            hashtabledata.Clear();
            hashquerydata.Clear();
            hashvalue.Clear();
            Parameter_Class db = new Parameter_Class();
            List<Parameter_Class> paramItems = db.getReportParams(schemapath, report_id.ToString());
            List<string> var_Values = paramItems.Select(x => x.param_variable).ToList();
            ViewBag.items = paramItems;
            variableList = var_Values;
            //TempData["Variable"] = var_Values;
            //int c_id = ClassFolder.GetValue.GetConnectionId(report_id);

            NpgsqlConnection newconn = new NpgsqlConnection(constring);
            newconn.Open();
            NpgsqlCommand ngcmdParameter;
            string sqldata = "SELECT dbconnection_id from " + schemapath + "report_mst as rm left join mass2.report_query_map as rqm on rm.report_id = rqm.report_id left join mass2.report_query_mst as rqmm on rqm.query_id = rqmm.query_id where rm.report_id = " + report_id + "";
            ngcmdParameter = new NpgsqlCommand(sqldata, newconn);
            int c_id = Convert.ToInt32(ngcmdParameter.ExecuteScalar());
            ngcmdParameter.Dispose();
            newconn.Close();

            connection_ID = c_id;
            hashtabledata.Add("report_id", report_id);
            return PartialView("_partialParamView", paramItems);
        }

        /// <summary>
        /// This method is used to load the report in crystal report format
        /// </summary>
        /// <param name="formdata"></param>
        /// This parameter holds all the input field values(report parameter values) of the popup modal
        /// <param name="items"></param>
        /// This parameter holds the dropdown item values of report paramters
        /// <returns></returns>
        public ActionResult QueryData(FormCollection formdata, List<SelectListItem> items)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    string id = hashtabledata["report_id"].ToString();
                    List<string> list1 = variableList;
                    string network = formdata["NetworkList"];
                    string vallabel = "";

                    foreach (string val in list1)     //Collecting all var variables
                    {
                        vallabel += val + ",";
                    }
                    string labeldata = vallabel;
                    string values = formdata["paramsdata"];
                    values = values + "," + network;

                    string pipeline_id = formdata["network_selection_daily"];
                    string section_id = formdata["section_selection_daily"];
                    if (pipeline_id != null)
                    {
                        if (pipeline_id.ToLower() == "all")
                        {
                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            string region_id = region.Replace(",", ";");
                            values = values + "," + region_id;
                        }
                        else
                        {
                            getValues();
                        }
                    }
                    if (section_id != null)
                    {
                        if (section_id.ToLower() == "all")
                        {
                            string section = ClassFolder.GetValue.GetSectionId(network);
                            section = section.Replace(",", ";");
                            values = values + "," + section;

                        }
                        else
                        {
                            getValues();
                        }
                    }
                    void getValues()
                    {
                        if (items != null)
                        {
                            string s = "";
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in items)
                            {
                                if (item.Selected)
                                {
                                    sb.Append(item.Text + ";");
                                }

                            }
                            s = sb.ToString().TrimEnd(';');

                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            values = values + "," + s;
                        }
                    }

                    rpt_label = labeldata;
                    rpt_values = values;
                    string pageurl = "~/WebForms/Dynamic_Form.aspx?report_id=" + id + "&report_labels=" + labeldata + "&report_params=" + values + "&connection_id=" + connection_ID;
                    Response.Redirect(pageurl);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else { }

            return View();
        }

        /// <summary>
        ///  This method is used to load the report in Grid format
        /// </summary>
        /// <param name="formdata"></param>
        /// This parameter holds all the input field values(report parameter values) of the popup modal
        /// <param name="items"></param>
        /// This parameter holds the dropdown item values of report paramters
        /// <returns></returns>
        [HttpPost]
        public ActionResult GridQueryData(FormCollection formdata, List<SelectListItem> items)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    //hashtabledata.Clear();
                    hashquerydata.Clear();
                    hashvalue.Clear();
                    int id = Convert.ToInt32(hashtabledata["report_id"]);
                    //List<string> list1 = (List<string>)TempData["Variable"];
                    List<string> list1 = variableList;
                    string network = formdata["NetworkList"];
                    string vallabel = "";

                    foreach (string val in list1)     //Collecting all var variables
                    {
                        vallabel += val + ",";
                    }
                    string labeldata = vallabel;
                    string values = formdata["paramsdata"];
                    values = values + "," + network;

                    string pipeline_id = formdata["network_selection_daily"];
                    string section_id = formdata["section_selection_daily"];
                    if (pipeline_id != null)
                    {
                        if (pipeline_id.ToLower() == "all")
                        {
                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            string region_id = region.Replace(",", ";");
                            values = values + "," + region_id;
                        }
                        else
                        {
                            getValues();
                        }
                    }
                    if (section_id != null)
                    {
                        if (section_id.ToLower() == "all")
                        {
                            string section = ClassFolder.GetValue.GetSectionId(network);
                            section = section.Replace(",", ";");
                            values = values + "," + section;

                        }
                        else
                        {
                            getValues();
                        }
                    }
                    void getValues()
                    {
                        if (items != null)
                        {
                            string s = "";
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in items)
                            {
                                if (item.Selected)
                                {
                                    sb.Append(item.Text + ";");
                                }

                            }
                            s = sb.ToString().TrimEnd(';');

                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            values = values + "," + s;
                        }
                    }

                    List<string> val_list = new List<string>();
                    string[] varVal = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value1 in varVal)
                    {
                        string datavalues = "";
                        datavalues = value1.Replace(";", ",");
                        val_list.Add(datavalues);
                    }

                    List<string> lbl_list = new List<string>();
                    vallabel = vallabel.ToString().TrimEnd(',');
                    string[] LabelVal = vallabel.Split(',');
                    foreach (string value2 in LabelVal)
                    {
                        lbl_list.Add(value2);
                    }

                    for (int i = 0; i < list1.Count; i++)
                    {
                        hashvalue.Add(lbl_list[i].ToString(), val_list[i].ToString());
                    }

                    hashvalue.Add("image_url", img_path);
                    string sql, query, path_name, file_name;
                    sql = ClassFolder.Query_Class.sql_reportData(schemapath, id);
                    DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        qry = dt.Rows[i]["parameters"].ToString();
                        query = dt.Rows[i]["query_script"].ToString();
                        path_name = dt.Rows[i]["report_path"].ToString();
                        file_name = dt.Rows[i]["report_file"].ToString();
                        report_name = dt.Rows[i]["report_name"].ToString();

                        val_query = GetReportQuery(query, hashvalue);  //Substituting var variables in partcular values in query 
                        hashvalue.Add(qry, val_query);
                        hashquerydata.Add(qry, val_query);
                        ViewBag.ReportName = report_name;
                        string path = path_name + file_name;
                        reportpath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                    }

                    rpt_label = labeldata;
                    rpt_values = values;
                    ViewBag.ConnectionId = connection_ID;
                    ViewBag.DataHashValue = hashquerydata;
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else { }
            return View();
        }
        
        /// <summary>
        /// This method is used when we have query and the hashtable data that holds the value of var character data in reports
        /// this method is executed when we replace the query that have var character in it to the hashtable data 
        /// then his method return the exact query that map to the report
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="updateddata"></param>
        /// <returns></returns>
        public string GetReportQuery(string sql, Hashtable updateddata)
        {
            Hashtable hashdata = new Hashtable();
            string Correctval, sPattern, sMatch;
            string function_str = (sql);

            Regex regex = new Regex(@"\[var([a-zA-Z0-9_\.\-]+)\]");
            MatchCollection matchCollection = regex.Matches(sql);

            foreach (Match match in matchCollection)
            {
                sMatch = match.Value.Replace("[", "");
                sMatch = sMatch.Replace("]", "");

                string longid = sMatch.ToString();
                string input = updateddata[longid].ToString();

                sPattern = match.Value.Replace("[", "\\[");
                sPattern = sPattern.Replace("]", "\\]");

                if (longid.ToLower() == "varstation_type")
                {
                    Correctval = "'" + input + "'";
                    function_str = Regex.Replace(function_str, sPattern, Correctval);
                }

                Correctval = input;
                function_str = Regex.Replace(function_str, sPattern, Correctval);

                function_script = function_str;
            }

            function_script = function_str;
            return function_script;
        }
        
        /// <summary>
        /// This method is when user load the report in grid format and then show the report in crstal report format
        /// then he can switch from grid format to crystal report format using this method 
        /// </summary>
        public void ReportPopUp()
        {
            try
            {
                string id = hashtabledata["report_id"].ToString();
                string pageurl = Url.Content("~/WebForms/Dynamic_Form.aspx?report_id=" + id + "&report_labels=" + rpt_label + "&report_params=" + rpt_values + "&connection_id=" + connection_ID);
                Response.Redirect(pageurl);
            }
            catch
            {

            }
            //Response.Write("<script> window.open('" + pageurl + "','_blank'); </script>");
        }

        /// <summary>
        /// This is a Schedular View
        /// This method is called when we click on the schedular icon in the report grid
        /// then a popup window shows which have some parameter fields that map with the report id
        /// </summary>
        /// <param name="report_id"></param>
        /// This parameter is used to fetch all the parameter field of related report data
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmailerInputData(int report_id)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            hashtabledata.Clear();
            Parameter_Class db = new Parameter_Class();
            List<Parameter_Class> paramItems = db.getReportParams(schemapath, report_id.ToString());
            List<string> var_Values = paramItems.Select(x => x.param_variable).ToList();
            ViewBag.items = paramItems;

            con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
            con.Open();
            string jobReportNameSql = "select report_name from " + schemapath + " report_mst where report_id =" + report_id + "";
            NpgsqlCommand cmd = new NpgsqlCommand(jobReportNameSql, con);
            string jobReportName = Convert.ToString(cmd.ExecuteScalar());
            cmd.Dispose();
            con.Close();

            ViewBag.JobReportName = jobReportName;
            ViewBag.DayItems = GetDays();
            ViewBag.WeekItems = GetWeekList();
            TempData["Variable"] = var_Values;
            hashtabledata.Add("report_id", report_id);
            return PartialView("_partialSchedularView", paramItems);
            //return PartialView("_partialSchedularViewDemo", paramItems);
        }

        /// <summary>
        /// This method is used to submit all the scheduling related details
        /// </summary>
        /// <param name="formdata"></param>
        /// This parameter holds all the input field values(report parameter values) of the popup modal
        /// <param name="items"></param>
        ///  This parameter holds the dropdown item values of report paramters
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailerData(FormCollection formdata, List<SelectListItem> items)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    ReportDocument cry = new ReportDocument();
                    Hashtable ht = new Hashtable();
                    int id = Convert.ToInt32(hashtabledata["report_id"]);
                    //string id = hashtabledata["report_id"].ToString();
                    List<string> list1 = (List<string>)TempData["Variable"];
                    string network = formdata["NetworkList"];
                    string mail_to = formdata["mail_to"];
                    string mail_cc = formdata["mail_cc"];
                    string mail_bcc = formdata["mail_bcc"];
                    string mail_subject = formdata["mail_sub"];
                    string mail_body = formdata["mail_body"];
                    string mail_schedule = formdata["schedule_time"];
                    string mail_reminder = formdata["reminder"];
                    string mail_reminder_date = formdata["reminder_date"];
                    string mail_reminder_todate = formdata["reminder_todate"];
                    string mail_reminder_day = formdata["reminder_day"];
                    string mail_reminder_week = formdata["reminder_week"];
                    string mail_reminder_month = formdata["reminder_month"];
                    string vallabel = "";
                    string reminder_duration;

                    if (mail_reminder.ToLower() == "daily")
                    {
                        //reminder_duration = mail_reminder_date + "(" + mail_reminder + ")";
                        reminder_duration = mail_reminder + "(" + mail_reminder_date + ")";
                    }
                    else if (mail_reminder.ToLower() == "weekly")
                    {
                        //reminder_duration = mail_reminder_week;
                        reminder_duration = mail_reminder + "(" + mail_reminder_week + ")";
                    }
                    else if (mail_reminder.ToLower() == "monthly")
                    {
                        //reminder_duration = mail_reminder_month;
                        reminder_duration = mail_reminder + "(" + mail_reminder_month + ")";
                    }
                    else
                    {
                        //reminder_duration = mail_reminder_date + mail_reminder_todate;
                        reminder_duration = mail_reminder + "(" + mail_reminder_date + "TO" + mail_reminder_todate + ")";

                    }

                    foreach (string val in list1)     //Collecting all var variables
                    {
                        vallabel += val + ",";
                    }
                    string labeldata = vallabel;
                    string values = formdata["paramsdata"];
                    values = values + "," + network;

                    string pipeline_id = formdata["network_selection_daily"];
                    ht.Add("network_selection", pipeline_id);
                    string section_id = formdata["section_selection_daily"];
                    if (pipeline_id != null)
                    {
                        if (pipeline_id.ToLower() == "all")
                        {
                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            string region_id = region.Replace(",", ";");
                            values = values + "," + region_id;
                        }
                        else
                        {
                            getValues();
                        }
                    }
                    if (section_id != null)
                    {
                        if (section_id.ToLower() == "all")
                        {
                            string section = ClassFolder.GetValue.GetSectionId(network);
                            section = section.Replace(",", ";");
                            values = values + "," + section;

                        }
                        else
                        {
                            getValues();
                        }
                    }
                    void getValues()
                    {
                        if (items != null)
                        {
                            string s = "";
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in items)
                            {
                                if (item.Selected)
                                {
                                    sb.Append(item.Text + ";");
                                }

                            }
                            s = sb.ToString().TrimEnd(';');

                            //labeldata = "[varDuration],[varendCalDate],[varstartCalDate],[varUnitFactor],[varPipelinemst_id],";
                            values = values + "," + s;
                        }
                    }

                    List<string> var_list = new List<string>();
                    string[] varVal = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value1 in varVal)
                    {
                        string datavalues = "";
                        datavalues = value1.Replace(";", ",");
                        var_list.Add(datavalues);
                    }

                    List<string> label_list = new List<string>();
                    labeldata = labeldata.ToString().TrimEnd(',');
                    string[] LabelVal = labeldata.Split(',');
                    foreach (string value2 in LabelVal)
                    {
                        label_list.Add(value2);
                    }

                    for (int i = 0; i < var_list.Count; i++)
                    {
                        ht.Add(label_list[i].ToString(), var_list[i].ToString());
                    }

                    string paramjsonData = JsonConvert.SerializeObject(ht);
                    string job_url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    job_url = job_url + "WebForms/Dynamic_Form.aspx?";
                    //string job_duration = ht["[varDuration]"].ToString();
                    int is_deleted = 0;
                    int job_interval = 1440;
                    //string created_on = DateTime.Now.ToString();
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //connectionString();
                    con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                    NpgsqlCommand cmd;
                    con.Open();
                    string report_sql = "SELECT report_name from " + schemapath + "report_mst where report_id = " + id + "";
                    cmd = new NpgsqlCommand(report_sql, con);
                    string rpt_name = cmd.ExecuteScalar().ToString();
                    cmd.Dispose();

                    string email_sql = "Insert into " + schemapath + "emailerjobs_emails(email_to, email_cc, email_bcc, subject, body, created_on) VALUES('" + mail_to + "','" + mail_cc + "','" + mail_bcc + "','" + mail_subject + "','" + mail_body + "','" + created_on + "') RETURNING schdr_jobid;";
                    cmd = new NpgsqlCommand(email_sql, con);
                    int email_id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    string emailSchedule_sql = "Insert into " + schemapath + "emailerjobs_scheduled(job_name,report_id, intervaltime, duration, param_value, schdr_jobid, url, id_deleted,created_on)VALUES('" + rpt_name + "'," + id + "," + job_interval + ",'" + reminder_duration + "','" + paramjsonData + "'," + email_id + ",'" + job_url + "'," + is_deleted + ",'" + created_on + "') RETURNING id;";
                    cmd = new NpgsqlCommand(emailSchedule_sql, con);
                    int job_id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    con.Close();

                    //return Content("<script language='javascript' type='text/javascript'>alert('Mail sent successfully!!!'); window.location.href = 'ScheduleGrid'; </script>");
                    return Content("<script language='javascript' type='text/javascript'>alert('Mail sent successfully!!!'); window.location.href = '/Dynamic/Index?menu_name=scheduletask'; </script>");
                }
                catch (Exception ex)
                {
                    //return Content("<script language='javascript' type='text/javascript'>alert(" + ex + ");</script>");
                    return Content(ex.Message);
                }
            }
            else { }

            return View();
        }
        
        public ReportDocument GetReportUrl(string label, string variables, int id)
        {
            Hashtable localhash = new Hashtable();
            //string url="";
            List<string> var_list = new List<string>();
            string[] varVal = variables.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value1 in varVal)
            {
                string datavalues = "";
                datavalues = value1.Replace(";", ",");
                var_list.Add(datavalues);
            }

            List<string> label_list = new List<string>();
            label = label.ToString().TrimEnd(',');
            string[] LabelVal = label.Split(',');
            foreach (string value2 in LabelVal)
            {
                label_list.Add(value2);
            }

            for (int i = 0; i < var_list.Count; i++)
            {
                localhash.Add(label_list[i].ToString(), var_list[i].ToString());
            }

            localhash.Add("image_url", img_path);
            string sql, query, path_name, file_name;
            ReportDocument cry = new ReportDocument();
            sql = ClassFolder.Query_Class.sql_reportData(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                qry = dt.Rows[i]["parameters"].ToString();
                query = dt.Rows[i]["query_script"].ToString();
                path_name = dt.Rows[i]["report_path"].ToString();
                file_name = dt.Rows[i]["report_file"].ToString();
                report_name = dt.Rows[i]["report_name"].ToString();

                val_query = GetReportQuery(query, localhash);  //Substituting var variables in partcular values in query 
                localhash.Add(qry, val_query);

                string path = path_name + file_name;
                reportpath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                cry.Load(reportpath);

                cry.DataSourceConnections[0].SetConnection(ClassFolder.CrystalConnection_Class.server, ClassFolder.CrystalConnection_Class.database, ClassFolder.CrystalConnection_Class.user, ClassFolder.CrystalConnection_Class.password);
                cry.SetParameterValue("report_title", report_name);
                cry.SetParameterValue("image_url", img_path);

                foreach (DictionaryEntry entry in localhash)
                {
                    string hashpattern = entry.Key.ToString().Replace("[var", "");
                    hashpattern = hashpattern.Replace("]", "");

                    if (hashpattern.ToLower() == "pipelinemst_id")
                    {
                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetPipelinename(entry.Value.ToString()));
                    }
                    else if (hashpattern.ToLower() == "section_id")
                    {
                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetSectionname(entry.Value.ToString()));
                    }
                    else if (hashpattern.ToLower() == "check_meter_id")
                    {
                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetCheckMetername(entry.Value.ToString()));
                    }
                    else if (hashpattern.ToLower() == "station_id")
                    {
                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetStationName(entry.Value.ToString()));
                    }
                    else if (hashpattern.ToLower() == "gc_id")
                    {
                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetGCName(entry.Value.ToString()));
                    }
                    else
                    {
                        cry.SetParameterValue(hashpattern, entry.Value.ToString());
                    }
                }

                cry.SetParameterValue(qry, val);
            }
            return cry;
        }

        /// <summary>
        /// This method is used to show the grid of report 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReportGrid()
        {
            hashtabledata.Clear();
            if (ClassFolder.Login_Class.access_token == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No network assign...!'); window.location.href = 'HomePage'; </script>");
            }
            else
            {
                return View();
            }

        }

        /// <summary>
        /// This is called when we want to fetch reports data in the report_mst table
        /// Now, not in used 
        /// </summary>
        /// <returns></returns>
        public ActionResult TableData()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            Report_Class db = new Report_Class();
            List<Report_Class> menuItems = db.getAllReportItems(schemapath, roleId);
            ViewBag.items = menuItems;
            return PartialView("_partialReportTableView", menuItems);
        }
       
        /// <summary>
        /// This method is used to active and disable the report status
        /// </summary>
        /// <param name="report_id"></param>
        /// holds the report id column value
        /// <param name="IsActiveState"></param>
        /// holds the is_deleted column value
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRptData(string report_id, string IsActiveState)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string id = report_id;
                string is_deleted = IsActiveState;
                string sql = ClassFolder.Query_Class.UpdateReport_mst(schemapath, is_deleted, id);
                bool status = ClassFolder.Database_Class.ExecuteQuery(sql);
                //return RedirectToAction("ReportGrid", "Report");
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Manage Report" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is used to add new report
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewRpt()
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            Connection_Class db = new Connection_Class();
            List<Connection_Class> ConnItems = db.getAllConnectionItems(schemapath);
            ViewBag.ConnectionItems = ConnItems;
            ViewBag.UserRoleType = GetUserTypeDD();
            return PartialView("_partialNewRptView");
        }

        /// <summary>
        /// This method is used to insert the details of new report related data 
        /// </summary>
        /// <param name="reportdata"></param>
        /// holds the Report_Class class data
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewRptData(Report_Class reportdata)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    string connectionString = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
                    using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                    {
                        int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                        int id = reportdata.report_id;
                        int report_roleID = roleId;
                        int connectionId = reportdata.conn_id;
                        int qry_id = reportdata.qry_id;
                        string name = reportdata.report_name;
                        string description = reportdata.report_description;
                        int report_user_id = reportdata.report_user_id;
                        string query = reportdata.report_query;
                        string path = rpt_path;
                        string file = reportdata.report_file;
                        int is_deleted = 0;
                        int kpi_val = 1;
                        //string created_on = DateTime.Now.ToString();
                        string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        int created_by = 1;
                        int optional = 0;
                        string default_value = null;

                        NpgsqlCommand cmd = new NpgsqlCommand();
                        cmd.Connection = con;
                        con.Open();
                        string report_sql = "Insert into " + schemapath + "report_mst(report_name,kpi_type,report_path,report_file,created_on,created_by,modified_on,modified_by,report_description,report_user_id) Values('" + name + "'," + kpi_val + ",'" + path + "','" + file + "','" + created_on + "'," + user_id + ",'" + created_on + "'," + user_id + ",'" + description + "'," + report_user_id + ") RETURNING report_id;";
                        cmd = new NpgsqlCommand(report_sql, con);
                        int rpt_id = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Dispose();

                        int q_id;
                        string label, variable, method, type, qry, param_sql, q_type, q_param, q_script, query_sql;

                        if (reportdata.query_data != null)
                        {
                            //report_query_mst variables
                            List<Models.Query_Class> queryclass = reportdata.query_data.ToList();

                            var query_result = from s in queryclass
                                               select s;
                            foreach (var Qrydata in query_result)
                            {
                                q_id = Convert.ToInt32(Qrydata.queryId);
                                q_type = Qrydata.type.Trim();
                                q_param = Qrydata.parameterData.Trim();
                                q_script = Qrydata.query_qrydata.Trim();
                                q_script = Qrydata.query_qrydata.Trim().ToLower();
                                q_script = q_script.Replace("varunitfactor", "varUnitFactor");
                                q_script = q_script.Replace("varduration", "varDuration");
                                q_script = q_script.Replace("varstartcaldate", "varstartCalDate");
                                q_script = q_script.Replace("varendcaldate", "varendCalDate");
                                q_script = q_script.Replace("varpipelinemst_id", "varPipelinemst_id");
                                q_script = q_script.Replace("vardaycount", "varDayCount");

                                string rest = "";
                                string where = "";
                                string order_by = "";
                                string group_by = "";
                                string select = "";
                                int pos = 0;
                                //string from = "";
                                pos = q_script.LastIndexOf("where");
                                if (pos > 0)
                                {
                                    select = q_script.Substring(0, pos);
                                    where = q_script.Substring(pos);
                                }
                                else
                                    select = q_script;
                                //Need to be think later how we will handle if order by or group by comes without where
                                if (where.Length > 0)
                                {
                                    pos = where.LastIndexOf("order by");
                                    if (pos > 0)
                                    {
                                        rest = where;
                                        where = where.Substring(0, pos);
                                        order_by = rest.Substring(pos);
                                    }
                                    pos = where.LastIndexOf("group by");
                                    if (pos > 0)
                                    {
                                        rest = where;
                                        where = where.Substring(0, pos);
                                        group_by = rest.Substring(pos);
                                    }
                                }

                                select = select.Replace("'", "''");
                                where = where.Replace("'", "''");
                                group_by = group_by.Replace("'", "''");
                                order_by = order_by.Replace("'", "''");

                                query_sql = "Insert into " + schemapath + "report_query_mst(function_name, type, query_script, query_where_cond, query_group_by, query_order_by,parameters,created_on,created_by,is_deleted,dbconnection_id) Values('" + reportdata.report_name + "','" + q_type + "','" + select + "','" + where + "','" + group_by + "','" + order_by + "','" + q_param + "','" + created_on + "'," + created_by + "," + is_deleted + "," + connectionId + ") RETURNING query_id;";
                                cmd = new NpgsqlCommand(query_sql, con);
                                int qrry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Dispose();

                                string map_sql = "Insert into " + schemapath + "report_query_map(report_id,query_id) Values(" + rpt_id + "," + qrry_id + ")";
                                cmd = new NpgsqlCommand(map_sql, con);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }

                        if (reportdata.param_data != null)
                        {
                            //report_query_param variables
                            List<Parameter_Class> paramclass = reportdata.param_data.ToList();

                            var param_result = from s in paramclass
                                               select s;
                            foreach (var data in param_result)
                            {
                                label = data.param_label.Trim();
                                variable = data.param_variable.Trim();
                                type = data.param_type.Trim();
                                method = data.param_method.Trim();
                                qry = data.param_query.Trim();

                                param_sql = "Insert into " + schemapath + "report_query_param(report_id,param_label,param_variable,param_type,param_method,param_query,is_deleted,optional,default_value) Values(" + rpt_id + ",'" + label + "','" + variable + "','" + type + "','" + method + "','" + qry + "'," + is_deleted + "," + optional + ",'" + default_value + "') Returning id";
                                cmd = new NpgsqlCommand(param_sql, con);
                                int prm_id = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Dispose();
                            }
                        }

                    }

                    con.Close();
                    //return RedirectToAction("ReportGrid", "Report");
                    return RedirectToAction("Index", "Dynamic", new { @menu_name = "Manage Report" });
                }

                catch (Exception ex)
                {
                    return Content("ERROR;" + ex.Message.ToString());
                    //return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = 'Index'; </script>");
                }
            }

            return View();
        }

        /// <summary>
        /// This method is called when we want to edit the report table using specific report id
        /// then that report detail related to that id is show on popup modal
        /// </summary>
        /// <param name="report_id"></param>
        /// This parameter hold the report id
        /// <returns></returns>
        public ActionResult EditReportData(int report_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Report_Class rc = new Report_Class();
                Report_Class paramItems = rc.getReportData(schemapath, report_id);

                Connection_Class db = new Connection_Class();
                List<Connection_Class> ConnItems = db.getAllConnectionItems(schemapath);
                ViewBag.ConnectionValues = ConnItems;
                ViewBag.EditUserRoleType = GetUserTypeDD();
                //return PartialView("_partialEditReportView", paramItems);
                return PartialView("_partialEditReportViewDemo", paramItems);
            }
            catch
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = 'ReportGrid'; </script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = '/Dynamic/Index?menu_name=Manage Report'; </script>");
            }

        }

        /// <summary>
        /// This method is used for editing/updating the report after submit button using its dashboard id
        /// </summary>
        /// <param name="rptData"></param>
        /// Basically this parameter is an instance of Report_Class class that hold the report related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRptData(Report_Class rptData)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    bool editStatus;
                    int user_id = int.Parse(ClassFolder.Login_Class.user_id);
                    //report_mst variables
                    int id = rptData.report_id;
                    string name = rptData.report_name;
                    string description = rptData.report_description;
                    int report_user_id = rptData.report_user_id;
                    string query = rptData.report_query;
                    string path = rptData.report_path;
                    string file = rptData.report_file;
                    int is_deleted = 0;
                    //string created_on = DateTime.Now.ToString();
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int created_by = 1;
                    int optional = 0;
                    string default_value = null;
                    int q_id = 0;
                    int connection_id = rptData.conn_id;
                    string label, variable, method, type, qry, param_sql, q_type, q_param, q_script, query_sql;

                    //connectionString();
                    con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    string query_id = ClassFolder.GetValue.GetNetworkId(id);

                    string querymapDelete = "DELETE FROM " + schemapath + "report_query_map WHERE query_id IN(" + query_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(querymapDelete);

                    string queryDelete = "DELETE FROM " + schemapath + "report_query_mst WHERE query_id IN(" + query_id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(queryDelete);

                    string paramDelete = "DELETE FROM " + schemapath + "report_query_param WHERE report_id IN (" + id + ")";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(paramDelete);

                    if (rptData.query_data != null)
                    {
                        List<Models.Query_Class> queryclassdata = rptData.query_data.ToList();
                        var query_result = from s in queryclassdata
                                           select s;
                        foreach (var Qrydata in query_result)
                        {
                            q_id = Convert.ToInt32(Qrydata.queryId);
                            q_type = Qrydata.type.Trim();
                            q_param = Qrydata.parameterData.Trim();
                            q_script = Qrydata.query_qrydata.Trim();
                            q_script = Qrydata.query_qrydata.Trim().ToLower();
                            q_script = q_script.Replace("varunitfactor", "varUnitFactor");
                            q_script = q_script.Replace("varduration", "varDuration");
                            q_script = q_script.Replace("varstartcaldate", "varstartCalDate");
                            q_script = q_script.Replace("varendcaldate", "varendCalDate");
                            q_script = q_script.Replace("varpipelinemst_id", "varPipelinemst_id");
                            q_script = q_script.Replace("vardaycount", "varDayCount");
                            string rest = "";
                            string where = "";
                            string order_by = "";
                            string group_by = "";
                            string select = "";
                            int pos = 0;
                            //string from = "";
                            pos = q_script.LastIndexOf("where");
                            if (pos > 0)
                            {
                                select = q_script.Substring(0, pos);
                                where = q_script.Substring(pos);
                            }
                            else
                                select = q_script;
                            //Need to be think later how we will handle if order by or group by comes without where
                            if (where.Length > 0)
                            {
                                pos = where.LastIndexOf("order by");
                                if (pos > 0)
                                {
                                    rest = where;
                                    where = where.Substring(0, pos);
                                    order_by = rest.Substring(pos);
                                }
                                pos = where.LastIndexOf("group by");
                                if (pos > 0)
                                {
                                    rest = where;
                                    where = where.Substring(0, pos);
                                    group_by = rest.Substring(pos);
                                }
                            }

                            select = select.Replace("'", "''");
                            where = where.Replace("'", "''");
                            group_by = group_by.Replace("'", "''");
                            order_by = order_by.Replace("'", "''");

                            query_sql = "Insert into " + schemapath + "report_query_mst(function_name, type, query_script, query_where_cond, query_group_by, query_order_by,parameters,created_on,created_by,is_deleted,dbconnection_id) Values('" + rptData.report_name + "','" + q_type + "','" + select + "','" + where + "','" + group_by + "','" + order_by + "','" + q_param + "','" + created_on + "'," + created_by + "," + is_deleted + "," + connection_id + ") RETURNING query_id;";
                            cmd = new NpgsqlCommand(query_sql, con);
                            int qrry_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();

                            string map_sql = "Insert into " + schemapath + "report_query_map(report_id,query_id) Values(" + id + "," + qrry_id + ")";
                            cmd = new NpgsqlCommand(map_sql, con);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    if (rptData.param_data != null)
                    {
                        List<Parameter_Class> paramclassdata = rptData.param_data.ToList();
                        var param_result = from s in paramclassdata
                                           select s;
                        foreach (var data in param_result)
                        {
                            label = data.param_label.Trim();
                            variable = data.param_variable.Trim();
                            type = data.param_type.Trim();
                            method = data.param_method.Trim();
                            qry = data.param_query.Trim();

                            param_sql = "Insert into " + schemapath + "report_query_param(report_id,param_label,param_variable,param_type,param_method,param_query,is_deleted,optional,default_value) Values(" + id + ",'" + label + "','" + variable + "','" + type + "','" + method + "','" + qry + "'," + is_deleted + "," + optional + ",'" + default_value + "') Returning id";
                            cmd = new NpgsqlCommand(param_sql, con);
                            int prm_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                        }
                    }

                    string report_sql = "UPDATE " + schemapath + "report_mst Set report_name='" + name + "', report_file='" + file + "', report_description='" + description + "', report_user_id=" + report_user_id + "modified_on='" + created_on + "',modified_by=" + user_id + " WHERE report_id = " + id + "";
                    editStatus = ClassFolder.Database_Class.ExecuteQuery(report_sql);

                    //return RedirectToAction("ReportGrid", "Report");
                    return RedirectToAction("Index", "Dynamic", new { @menu_name = "Manage Report" });
                }

                catch (Exception ex)
                {
                    return Content("ERROR;" + ex.Message);
                }

            }

            return View();
        }

        /// <summary>
        /// This method is called when we want to edit the connection using specific connection id
        /// then that connection detail related to that id is show on popup modal
        /// </summary>
        /// <param name="conn_id"></param>
        /// This parameter hold the connection id
        /// <returns></returns>
        public ActionResult EditConnData(int conn_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Connection_Class conclass = new Connection_Class();
                Connection_Class connItems = conclass.getConnectionData(schemapath, conn_id);

                return PartialView("_partialEditConnectionView", connItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to edit connection table data
        /// </summary>
        /// <param name="conn_id"></param>
        /// holds connection id
        /// <param name="report_id"></param>
        /// holds report id
        /// <returns></returns>
        public ActionResult EditConnectionValue(int conn_id, int report_id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Connection_Class conclass = new Connection_Class();
                Connection_Class connItems = conclass.getConnectionData(schemapath, conn_id);
                TempData["reportIdValue"] = report_id;
                return PartialView("_partialEditInEditConnectionView", connItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to edit connection table data
        /// </summary>
        /// <param name="conData"></param>
        /// Using instance of Connection_Class, it hold all updated values
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditConnectionData(Connection_Class conData)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                bool connStatus;
                connStatus = getConnectionStatus(conData);
                return RedirectToAction("AddNewRpt", "Report");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to edit connection table data
        /// </summary>
        /// <param name="conData"></param>
        /// Using instance of Connection_Class, it hold all updated values
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditConnectionEntry(Connection_Class conData)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                bool status;
                int connection_id;
                int id = (int)TempData["reportIdValue"];
                string dsn, user_id, password, database_name, conn_sql;

                connection_id = conData.connection_id;
                dsn = conData.dsn_name.Trim();
                user_id = conData.user_id.Trim();
                password = conData.password.Trim();
                database_name = conData.db_schema_name.Trim();

                conn_sql = "UPDATE " + schemapath + "dbconnection_mst Set connection_name='" + dsn + "', dsn_name='" + dsn + "',user_id='" + user_id + "',password='" + password + "',db_schemaname='" + database_name + "' WHERE id = " + connection_id + "";
                status = ClassFolder.Database_Class.ExecuteQuery(conn_sql);

                return RedirectToAction("EditReportData", "Report", new { @report_id = id });
                //return Content("Updated successfully!!!");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to test the connection that is valid or not
        /// </summary>
        /// <param name="cs"></param>
        /// holds the connection details
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConnectionTest(Connection_Class cs)
        {
            try
            {
                string result = ClassFolder.Database_Class.CheckODBC(cs.dsn_name, cs.user_id, cs.password, cs.db_schema_name);
                if (result == "true")
                {
                    return Content("Test successfully!!!");
                }
                else
                {
                    return Content("Please check connection entries!!!");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used to save the connection details
        /// </summary>
        /// <param name="cs"></param>
        /// holds the connection details
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConnectionSave(Connection_Class cs)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                using (NpgsqlConnection con = new NpgsqlConnection(constring))
                {
                    NpgsqlCommand cmd;
                    con.Open();
                    int db_type = 1;
                    int is_deleted = 0;
                    //string created_on = DateTime.Now.ToString();
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    int connection_timeout = 5000;
                    int query_timeout = 120;

                    string result = ClassFolder.Database_Class.CheckODBC(cs.dsn_name, cs.user_id, cs.password, cs.db_schema_name);
                    if (result == "true")
                    {
                        //string conn_SelectSql = "SELECT * FROM " + schemapath + "dbconnection_mst WHERE dsn_name='" + cs.dsn_name + "' AND user_id='" + cs.user_id + "' AND password='" + cs.password + "' AND db_schemaname='" + cs.db_schema_name + "'";
                        string conn_SelectSql = ClassFolder.Query_Class.sql_ConnSelectQuery(schemapath, cs.dsn_name, cs.user_id, cs.password, cs.db_schema_name);
                        DataTable dt = ClassFolder.Database_Class.OpenQuery(conn_SelectSql);

                        if (dt.Rows.Count == 0)
                        {
                            //string conn_InsertSql = "Insert into " + schemapath + "dbconnection_mst(connection_name, db_type, driver_type, dsn_name, user_id, password, db_schemaname, connection_timeout, query_timeout, created_on, is_deleted) Values('" + cs.dsn_name + "'," + db_type + "," + db_type + ",'" + cs.dsn_name + "','" + cs.user_id + "','" + cs.password + "','" + cs.db_schema_name + "'," + connection_timeout + "," + query_timeout + ",'" + created_on + "'," + is_deleted + ") RETURNING id;";
                            string conn_InsertSql = ClassFolder.Query_Class.sql_ConnInsertQuery(schemapath, cs.dsn_name, db_type, cs.user_id, cs.password, cs.db_schema_name, connection_timeout, query_timeout, created_on, is_deleted);
                            cmd = new NpgsqlCommand(conn_InsertSql, con);
                            int connection_id = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Dispose();
                            con.Close();
                            return RedirectToAction("AddNewRpt", "Report");
                        }
                        else
                        {
                            return Content("ERROR;Connection Entries already exists!!!");
                        }
                    }
                    else
                    {
                        con.Close();
                        return Content("ERROR;Please check connection entries!!!");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to show the grid of schedular table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScheduleGrid()
        {
            if (region == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No network assign...!'); window.location.href = 'HomePage'; </script>");
            }
            else
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                Scheduler_Class db = new Scheduler_Class();
                List<Scheduler_Class> menuItems = db.getAllScheduleItems(schemapath);
                ViewBag.ScheduleItems = menuItems;
                return View();
            }

        }

        /// <summary>
        /// This method is called when we click on pause button in the schedular grid
        /// This method is used to pause the schedule report by set it its is_deleted column value i.e, 0 or 1
        /// Now, not in used
        /// </summary>
        /// <param name="id"></param>
        /// holds report id column value
        /// <param name="is_deleted"></param>
        /// holds is_deleted column value
        /// <returns></returns>
        public ActionResult PauseSchedule(int id, int is_deleted)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            if (is_deleted == 0)
            {
                is_deleted = 1;
            }
            else { is_deleted = 0; }

            bool status;
            string sql = "UPDATE " + schemapath + "pipeline_job_master Set status=" + is_deleted + " WHERE id = " + id + "";
            status = ClassFolder.Database_Class.ExecuteQuery(sql);
            //return RedirectToAction("ScheduleGrid", "Report");
            return RedirectToAction("Index", "Dynamic", new { @menu_name = "Schedule Task" });
        }

        /// <summary>
        /// This method is called when we click on pause button in the schedular grid
        /// This method is used to deleted the schedule report by set it its is_deleted column value i.e, 0 or 1
        /// </summary>
        /// <param name="id"></param>
        ///  holds report id column value
        /// <param name="is_deleted"></param>
        /// holds is_deleted column value
        /// <returns></returns>
        public ActionResult DeleteSchedule(int id, int is_deleted)
        {
            if (schemapath == null)
            {
                schemapath = "public.";
            }
            if (is_deleted == 0)
            {
                is_deleted = 1;
            }

            bool status;
            string sql = "UPDATE " + schemapath + "pipeline_job_master Set is_deleted=" + is_deleted + " WHERE id= " + id + "";
            status = ClassFolder.Database_Class.ExecuteQuery(sql);
            //return RedirectToAction("ScheduleGrid", "Report");
            return RedirectToAction("Index", "Dynamic", new { @menu_name = "Schedule Task" });
        }

        /// <summary>
        /// This method is used to upload the file(crystal report file)
        /// </summary>
        /// <param name="file"></param>
        /// holds the name of the selected file
        /// <returns></returns>
        public ContentResult UploadFileData(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    uploadedFile = _FileName;
                    //string _path = Path.Combine(Server.MapPath(rpt_path), _FileName);
                    string _path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(rpt_path), _FileName);

                    if (System.IO.File.Exists(_path))
                    {
                        //System.IO.File.Delete(_path);
                        //file.SaveAs(_path);
                        file_upload = file;
                        return Content("Exists");
                    }

                    file.SaveAs(_path);
                }

                return Content("File Uploaded Successfully!!!");
            }
            catch (Exception ex)
            {
                //return Content("File upload failed!!!");
                return Content(ex.Message);
            }
        }
        
        /// <summary>
        /// This method is used for confirmation regarding to overwrite it or not
        /// If yes then the selected file is overwrite
        /// otherwise skip the function
        /// </summary>
        /// <param name="confirm_value"></param>
        /// holds the YES or No values in it
        /// <returns></returns>
        public ContentResult submitdata(string confirm_value)
        {
            try
            {
                if (confirm_value == "yes")
                {
                    string _path = Path.Combine(Server.MapPath(rpt_path), uploadedFile);
                    //FileUpload fd = new FileUpload();
                    System.IO.File.Delete(_path);
                    file_upload.SaveAs(_path);
                    return Content("File Uploaded Successfully!!!");
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailSubmitData(Scheduler_Class sc)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    int id = Convert.ToInt32(hashtabledata["report_id"]);
                    string job_name = sc.job_name;
                    string job_description = sc.job_description;
                    string job_duration = sc.duration;
                    string mail_reminder_date = sc.schedule_start_date;
                    string newTime = Convert.ToDateTime(mail_reminder_date).ToString("yyyy-MM-dd HH:mm:ss");

                    string mail_schedule = sc.recurrenceItems;
                    string frequencyItems = sc.frequencyDays;
                    int periodId = sc.period_id;
                    string job_status = sc.schedule_status;
                    string mail_to = sc.to_email;
                    string mail_cc = sc.cc_email;
                    string mail_bcc = sc.bcc_email;
                    string mail_subject = sc.sub_email;
                    string mail_body = sc.body_email;
                    string report_label = sc.param_label;
                    string report_variable = sc.param_variable;
                    string network_selection_data = sc.param_network_selection;
                    string section_selection_data = sc.param_section_selection;
                    string network_data = sc.param_network_variable;
                    string section_data = sc.param_section_variable;
                    string network_parameters = sc.param_networks;
                    
                    string[] job_dateData = sc.job_dateData;
                    string job_from_date = sc.job_from_date;
                    string job_to_date = sc.job_to_date;

                    string report_network_values = "";
                    string report_section_values = "";
                    if (network_selection_data != null)
                    {
                        if (network_selection_data.ToLower() == "all")
                        {
                            report_network_values = network_selection_data;
                            //string region_id = region.Replace(",", ";");
                            //report_variable = report_variable + "," + region_id;
                            report_variable = report_variable + "," + network_selection_data;
                        }
                        else
                        {
                            report_network_values = network_selection_data;
                            network_data = network_data.Replace(",", ";");
                            report_variable = report_variable + "," + network_data;
                        }
                    }
                    if (section_selection_data != null)
                    {
                        if (section_selection_data.ToLower() == "all")
                        {
                            report_section_values = section_selection_data;
                            //string section = ClassFolder.GetValue.GetSectionId(network_parameters);
                            //section = section.Replace(",", ";");
                            //report_variable = report_variable + "," + section;
                            report_variable = report_variable + "," + section_selection_data;
                        }
                        else
                        {
                            report_section_values = section_selection_data;
                            section_data = section_data.Replace(",", ";");
                            report_variable = report_variable + "," + section_data;
                        }
                    }
                    int jobStatus = 0;
                    if (job_status.ToLower() == "false")
                    {
                        jobStatus = 1;
                    }

                    string job_url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    job_url = job_url + "WebForms/Dynamic_Form.aspx?";

                    string job_arguments = job_url + "report_id=[report_id]&report_labels=[report_labels]&report_params=[report_params]&connection_id=[connection_id]&flag=[flag]";
                    string rpt_name = "";
                    int connection_id = 0;
                    string created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    con.ConnectionString = ClassFolder.Database_Class.GetConnectionString();
                    NpgsqlCommand cmd;
                    con.Open();

                    //string report_sql = "SELECT report_name from " + schemapath + "report_mst where report_id = " + id + "";
                    //cmd = new NpgsqlCommand(report_sql, con);
                    //string rpt_name = cmd.ExecuteScalar().ToString();
                    //cmd.Dispose();

                    //string sqldata = "SELECT dbconnection_id from " + schemapath + "report_mst as rm left join mass2.report_query_map as rqm on rm.report_id = rqm.report_id left join mass2.report_query_mst as rqmm on rqm.query_id = rqmm.query_id where rm.report_id = " + id + "";
                    //cmd = new NpgsqlCommand(sqldata, con);
                    //int connection_id = Convert.ToInt32(cmd.ExecuteScalar());
                    //cmd.Dispose();

                    string sqldata = "SELECT report_name, dbconnection_id from " + schemapath + "report_mst as rm left join mass2.report_query_map as rqm on rm.report_id = rqm.report_id left join mass2.report_query_mst as rqmm on rqm.query_id = rqmm.query_id where rm.report_id = " + id + "";
                    cmd = new NpgsqlCommand(sqldata, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        rpt_name = rdr["report_name"].ToString();
                        connection_id = int.Parse(rdr["dbconnection_id"].ToString());
                    }
                    rdr.Dispose();
                    cmd.Dispose();

                    string email_sql = "Insert into " + schemapath + "emailerjobs_emails(email_to, email_cc, email_bcc, subject, body, created_on) VALUES('" + mail_to + "','" + mail_cc + "','" + mail_bcc + "','" + mail_subject + "','" + mail_body + "','" + created_on + "') RETURNING schdr_jobid;";
                    cmd = new NpgsqlCommand(email_sql, con);
                    int email_id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();

                    //int email_id = 0;
                    Hashtable hashVal = new Hashtable();
                    hashVal.Add("url", job_arguments);
                    hashVal.Add("report_id", id);
                    hashVal.Add("connection_id", connection_id);
                    hashVal.Add("email_id", email_id);
                    hashVal.Add("flag", 1);
                    hashVal.Add("report_labels", report_label);
                    hashVal.Add("report_params", report_variable);
                    if (job_dateData != null)
                    {
                        hashVal.Add("FROMDATE", job_dateData[0]);
                        hashVal.Add("TODATE", job_dateData[1]);
                    }
                    string paramjsonData = JsonConvert.SerializeObject(hashVal);

                    string emailSchedule_sql = "Insert into " + schemapath + "pipeline_job_master(job_name, job_description, report_id, job_path_filename, job_timeinterval, pipelinemst_id,pipeline_section_id, period_id, job_lastexetime, job_nextexetime, job_logfilepath, job_status, status, created_by, created_on, job_id, frequency_days, arguments, job_type, destination, url, job_method, is_deleted) "
                       + "VALUES ('" + job_name + "','" + job_description + "'," + id + ",'" + rpt_name + "'," + mail_schedule + ", null, null, " + periodId + ", '" + mail_reminder_date + "', "
                       + "'" + newTime + "', null, '" + job_duration + "', " + jobStatus + ", 1, "
                       + "'" + created_on + "', " + email_id + ", '" + frequencyItems + "', '" + paramjsonData + "', "
                       + "'Report', 'EMAIL', '" + job_url + "', null, 0) RETURNING id;";

                    cmd = new NpgsqlCommand(emailSchedule_sql, con);
                    int job_id = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Dispose();
                    con.Close();
                    return Content("Inserted successfully!!!");
                    //return Content("<script language='javascript' type='text/javascript'>alert('Mail sent successfully!!!'); window.location.href = '/Dynamic/Index?menu_name=scheduletask'; </script>");
                    //return RedirectToAction("Index", "Dynamic", new { @menu_name = "Schedule Task" });
                }
                catch (Exception ex)
                {
                    return Content("ERROR;" + ex.Message.ToString());
                }
            }
            return View();
        }

        /// <summary>
        /// This method is called when we want to edit the schedular table using specific id
        /// then that schedular detail related to that id is show on popup modal
        /// </summary>
        /// <param name="id"></param>
        /// This parameter hold the job id
        /// <returns></returns>
        public ActionResult EditSchedularData(int id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                List<SelectListItem> scheduleDuration = new List<SelectListItem>() {
                  new SelectListItem { Text = "SELECT", Value = "" },
                  new SelectListItem { Text = "DAILY", Value = "daily" },
                  new SelectListItem { Text = "WEEKLY", Value = "weekly" },
                  new SelectListItem { Text = "MONTHLY", Value = "monthly" }
                };
                List<SelectListItem> paramDateDuration = new List<SelectListItem>() {
                  new SelectListItem { Text = "SELECT", Value = "" },
                  new SelectListItem { Text = "TODAY", Value = "TODAY"},
                  new SelectListItem { Text = "YESTERDAY", Value = "YESTERDAY"},
                  new SelectListItem { Text = "THIS MONTH", Value = "THISMONTH"},
                  new SelectListItem { Text = "PREVIOUS MONTH", Value = "PREVIOUSMONTH"},
                  new SelectListItem { Text = "THIS WEEK", Value = "THISWEEK"},
                  new SelectListItem { Text = "PREVIOUS WEEK", Value = "PREVIOUSWEEK"},
                  new SelectListItem { Text = "CUSTOM", Value = "CUSTOM"},
                };
                Scheduler_Class sc = new Scheduler_Class();
                Scheduler_Class scheduleItems = sc.getSchedulerData(schemapath, id);
                Parameter_Class db = new Parameter_Class();
                List<Parameter_Class> paramItems = null;
                dynamic paramObj = JsonConvert.DeserializeObject(scheduleItems.job_report_arguments);
                string report_param_data = paramObj["report_params"];
                if (report_param_data != null) 
                {
                    string report_param_FromDate = paramObj["FROMDATE"];
                    string report_param_ToDate = paramObj["TODATE"];
                    Hashtable dateHashData = new Hashtable();
                    dateHashData.Add("FROMDATE", report_param_FromDate);
                    dateHashData.Add("TODATE", report_param_ToDate);
                    report_param_data = GetDateParameter(report_param_data, dateHashData);
                    ViewBag.ReportParams = report_param_data;

                    paramItems = db.getScheduleReportParams(schemapath, scheduleItems.job_report_id.ToString(), report_param_data);
                    List<string> var_Values = paramItems.Select(x => x.param_variable).ToList();
                    TempData["Variable"] = var_Values;
                }

                ViewBag.DayItems = GetDays();
                ViewBag.WeekItems = GetWeekList();
                ViewBag.ScheduleDuration = scheduleDuration;
                ViewBag.DateDuration = paramDateDuration;
                ViewBag.ParameterItems = paramItems;
                return PartialView("_partialEditSchedulerView", scheduleItems);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public string GetDateParameter(string sql, Hashtable updateddata)
        {
            string Correctval, sPattern, sMatch;
            string function_str = (sql);

            Regex regex = new Regex(@"\[([a-zA-Z0-9_\.\-]+)\]");
            MatchCollection matchCollection = regex.Matches(sql);

            foreach (Match match in matchCollection)
            {
                sMatch = match.Value.Replace("[", "");
                sMatch = sMatch.Replace("]", "");

                string longid = sMatch.ToString();
                string input = updateddata[longid].ToString();

                sPattern = match.Value.Replace("[", "\\[");
                sPattern = sPattern.Replace("]", "\\]");

                Correctval = input;
                function_str = Regex.Replace(function_str, sPattern, Correctval);

                function_script = function_str;
            }

            function_script = function_str;
            return function_script;
        }

        /// <summary>
        /// This method is used to active and disable the schedular status
        /// </summary>
        /// <param name="schedular_id"></param>
        /// holds the schedular id column value
        /// <param name="IsActiveState"></param>
        ///  holds the is_deleted column value
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSchedulerData(string schedular_id, string IsActiveState)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string id = schedular_id;
                string is_deleted = IsActiveState;
                string sql = ClassFolder.Query_Class.UpdateScheduler_mst(schemapath, is_deleted, id);
                bool status = ClassFolder.Database_Class.ExecuteQuery(sql);
                return RedirectToAction("Index", "Dynamic", new { @menu_name = "Schedule Task" });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable DerializeDataTable(string json)
        {
            var table = JsonConvert.DeserializeObject<DataTable>("[" + json + "]");
            return table;
        }
        
        /// <summary>
        /// This method is used for showing the job history grid format
        /// </summary>
        /// <param name="id"></param>
        /// Holds the job id
        /// <returns></returns>
        public ActionResult GetJobHistory(int id) 
        {
            string job_sql = ClassFolder.Query_Class.sql_JobHistory(schemapath, id);
            DataTable dt = ClassFolder.Database_Class.OpenQuery(job_sql);
            ViewBag.JobHistoryData = dt;
            dt.Dispose();
            return View();
        }
    }
}