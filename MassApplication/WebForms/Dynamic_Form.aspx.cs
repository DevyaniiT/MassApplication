using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Text.RegularExpressions;
using CrystalDecisions.Shared;

namespace MassApplication.WebForms
{
    public partial class Dynamic_Form : System.Web.UI.Page
    {
        public static Hashtable hashvariabledata = new Hashtable();
        public static string reportpath, report_name, qry, val, function_script;
        public static string schemapath = ConfigurationManager.AppSettings["SchemaPath"];
        public static DataTable query_table = new DataTable();
        string img_path = ConfigurationManager.AppSettings["rptImagePath"];
        string pdfFile = ConfigurationManager.AppSettings["pdfFile"];
        int report_id, con_id, flag;
        string report_params, report_labels;
        MASSODBC.MASSODBC ODBC_obj = new MASSODBC.MASSODBC();

        protected void Page_Init(object sender, EventArgs e)
        {
            //region = Request.QueryString["region"];
            report_id = Convert.ToInt32(Request.QueryString["report_id"]);
            report_params = Request.QueryString["report_params"].ToString();
            report_labels = Request.QueryString["report_labels"].ToString();
            con_id = Convert.ToInt32(Request.QueryString["connection_id"]);
            flag = Convert.ToInt32(Request.QueryString["flag"]);

            //if (!IsPostBack)
            //{
            //    GenerateReport();
            //}
            //else
            //{
            //    GetParameters();
            //}

            GenerateReport();
        }

        public void GenerateReport()
        {
            try
            {
                hashvariabledata.Clear();
                //hashquerydata.Clear();

                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                List<string> list1 = new List<string>();
                //string[] varVal = report_params.Split(',');
                string[] varVal = report_params.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value1 in varVal)
                {
                    string datavalues = "";
                    datavalues = value1.Replace(";", ",");
                    list1.Add(datavalues);
                    //list1.Add(value1);
                }

                List<string> list2 = new List<string>();
                report_labels = report_labels.ToString().TrimEnd(',');
                string[] LabelVal = report_labels.Split(',');
                foreach (string value2 in LabelVal)
                {
                    list2.Add(value2);
                }

                for (int i = 0; i < list1.Count; i++)
                {
                    hashvariabledata.Add(list2[i].ToString(), list1[i].ToString());
                }

                if (hashvariabledata.ContainsKey("varPipelinemst_id"))
                {
                    if (hashvariabledata.ContainsValue("all") || hashvariabledata.ContainsValue("All")) 
                    {
                        hashvariabledata["varPipelinemst_id"] = ClassFolder.Login_Class.region_id;
                    }
                }
                if (hashvariabledata.ContainsKey("varSection_id"))
                {
                    if (hashvariabledata.ContainsValue("all") || hashvariabledata.ContainsValue("All"))
                    {
                        hashvariabledata["varSection_id"] = ClassFolder.GetValue.GetODBCSectionId(hashvariabledata["varPipelinemst_id"].ToString());
                        //hashvariabledata["varSection_id"] = ClassFolder.GetValue.GetSectionId(hashvariabledata["varPipelinemst_id"].ToString());
                    }
                }
                if (hashvariabledata.ContainsKey("varstartCalDate"))
                {
                    string hashKey = hashvariabledata["varstartCalDate"].ToString();
                    string startDate = getDataDate(hashKey);
                    hashvariabledata["varstartCalDate"] = startDate;
                }
                if (hashvariabledata.ContainsKey("varendCalDate"))
                {
                    string hashKey = hashvariabledata["varendCalDate"].ToString();
                    string endDate = getDataDate(hashKey);
                    hashvariabledata["varendCalDate"] = endDate;

                }

                hashvariabledata.Add("image_url", img_path);
                string sql, query, path_name, file_name;
                ReportDocument cry = new ReportDocument();
                MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();

                sql = ClassFolder.Query_Class.sql_reportData(schemapath, report_id);
                DataTable dt = new DataTable();

                string conn_sql = ClassFolder.Query_Class.sql_getConnId(schemapath, con_id);
                dt = ClassFolder.Database_Class.OpenQuery(conn_sql);
                dt.Dispose();
                foreach (DataRow dr in dt.Rows)
                {
                    obj.DatabaseName = dr["db_schemaname"].ToString();
                    obj.OdbcName = dr["dsn_name"].ToString();
                    obj.UserId = dr["user_id"].ToString();
                    obj.Password = dr["password"].ToString();
                }

                ODBC_obj = obj;

                dt = ClassFolder.Database_Class.OpenQuery(sql);
                //dt = obj.OpenTable(sql, "query");
                if (dt.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "NoRecordAlertMsg", "alert('No record found!!!');", true);
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        qry = dt.Rows[i]["parameters"].ToString();
                        query = dt.Rows[i]["query_script"].ToString();
                        path_name = dt.Rows[i]["report_path"].ToString();
                        file_name = dt.Rows[i]["report_file"].ToString();
                        report_name = dt.Rows[i]["report_name"].ToString();

                        val = GetReportQuery(query, hashvariabledata);  //Substituting var variables in partcular values in query 
                        hashvariabledata.Add(qry, val);
                        //hashquerydata.Add(qry, val);

                        string path = path_name + file_name;
                        reportpath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                        cry.Load(reportpath);

                        //cry.DataSourceConnections[0].SetConnection(ClassFolder.CrystalConnection_Class.server, ClassFolder.CrystalConnection_Class.database, ClassFolder.CrystalConnection_Class.user, ClassFolder.CrystalConnection_Class.password);
                        cry.DataSourceConnections[0].SetConnection(ClassFolder.CrystalConnection_Class.server, ODBC_obj.DatabaseName, ODBC_obj.UserId, ODBC_obj.Password);
                        //cry.SetParameterValue("report_title", report_name);

                        string logo_path = System.Web.Hosting.HostingEnvironment.MapPath(img_path);
                        //cry.SetParameterValue("image_url", logo_path);  //actual image path
                        //cry.SetParameterValue("image_url", img_path);

                        ParameterFieldDefinitions crParameterdef;
                        crParameterdef = cry.DataDefinition.ParameterFields;
                        foreach (ParameterFieldDefinition def in crParameterdef)
                        {
                            if (def.Name.Equals("report_title")) // check if parameter exists in report
                            {
                                cry.SetParameterValue("report_title", report_name); // set the parameter value in the report
                            }
                            if (def.Name.Equals("image_url"))
                            {
                                cry.SetParameterValue("image_url", logo_path);
                            }
                            if (def.Name.Equals(qry))
                            {
                                cry.SetParameterValue(qry, val);
                            }

                            foreach (DictionaryEntry entry in hashvariabledata)
                            {
                                string hashpattern = entry.Key.ToString().Replace("var", "");
                                //hashpattern = hashpattern.Replace("]", "");
                                if (def.Name.Equals(hashpattern))
                                {
                                    if (hashpattern.ToLower() == "pipelinemst_id")
                                    {
                                        //cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetPipelinename(entry.Value.ToString()));
                                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetODBCPipelinename(entry.Value.ToString()));
                                    }
                                    else if (hashpattern.ToLower() == "section_id")
                                    {
                                        cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetODBCSectionname(entry.Value.ToString()));
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
                            }
                        }

                        //cry.SetParameterValue(qry, val);
                        //Dynamic_Report.Visible = true;
                        //Dynamic_Report.ReportSource = cry;
                    }

                    if (flag == 1)
                    {
                        //GetSchedularData(cry);
                        GetDownloadReportInPDF(cry);
                    }
                    else
                    {
                        Dynamic_Report.Visible = true;
                        //ViewState["Customers_Data"] = dsCustomers;
                        Dynamic_Report.ReportSource = cry;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "myErrorScript", "alert('" + ex.Message + "');", true);
            }

        }
        public void GetParameters()
        {
            try
            {
                ReportDocument cry = new ReportDocument();
                cry.Load(reportpath);
                //cry.DataSourceConnections[0].SetConnection(ClassFolder.CrystalConnection_Class.server, ClassFolder.CrystalConnection_Class.database, ClassFolder.CrystalConnection_Class.user, ClassFolder.CrystalConnection_Class.password);
                cry.DataSourceConnections[0].SetConnection(ClassFolder.CrystalConnection_Class.server, ODBC_obj.DatabaseName, ODBC_obj.UserId, ODBC_obj.Password);
                string logo_path = System.Web.Hosting.HostingEnvironment.MapPath(img_path);
                //cry.SetParameterValue("image_url", logo_path);

                ParameterFieldDefinitions crParameterdef;
                crParameterdef = cry.DataDefinition.ParameterFields;
                foreach (ParameterFieldDefinition def in crParameterdef)
                {
                    if (def.Name.Equals("report_title"))
                    {
                        cry.SetParameterValue("report_title", report_name);
                    }
                    if (def.Name.Equals("image_url"))
                    {
                        cry.SetParameterValue("image_url", logo_path);
                    }

                    foreach (DictionaryEntry entry in hashvariabledata)
                    {
                        string hashpattern = entry.Key.ToString().Replace("var", "");
                        //hashpattern = hashpattern.Replace("]", "");
                        if (def.Name.Equals(hashpattern))
                        {
                            if (hashpattern.ToLower() == "pipelinemst_id")
                            {
                                //cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetPipelinename(entry.Value.ToString()));
                                cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetODBCPipelinename(entry.Value.ToString()));

                            }
                            else if (hashpattern.ToLower() == "section_id")
                            {
                                //cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetSectionname(entry.Value.ToString()));
                                cry.SetParameterValue(hashpattern, ClassFolder.GetValue.GetODBCSectionname(entry.Value.ToString()));

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
                    }
                }

                Dynamic_Report.Visible = true;
                Dynamic_Report.ReportSource = cry;
                // ClientScript.RegisterStartupScript(this.GetType(), "Pop", "OpenModal();", true);
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "myErrorScript", "alert('" + ex.Message + "');", true);
            }
        }
        public string GetReportQuery(string sql, Hashtable updateddata)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void GetSchedularData(ReportDocument cry)
        {
            try
            {
                Dynamic_Report.Visible = false;

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfFile;
                CrExportOptions = cry.ExportOptions;  //Report document  object has to be given here
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                cry.Export();

                string to = ClassFolder.Email_Class.to_mail;
                string cc = ClassFolder.Email_Class.cc_mail;
                string bcc = ClassFolder.Email_Class.bcc_mail;
                //string sub = ClassFolder.Email_Class.mail_subject;
                string sub = report_name;
                string mbody = ClassFolder.Email_Class.mail_body;
                //string mbody = "Please find attachment";
                string schedule = ClassFolder.Email_Class.mail_duration;

                ClassFolder.Email_Class.SendEmailerData(to, cc, bcc, sub, mbody, schedule);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "NoRecordAlertMsg", "alert('Mail sent!!!');", true);

            }
            catch (Exception ex)
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "NoRecordAlertMsg", "alert("+ ex.Message +");", true);
                throw new Exception(ex.Message);
            }
        }
        public void GetDownloadReportInPDF(ReportDocument cry) 
        {
            try
            {
                DateTime todaydate = DateTime.Now;
                string runtime_date = todaydate.ToString("yyyy-MM-dd");
                string titleName = report_name + runtime_date;
                ExportFormatType formatType = ExportFormatType.PortableDocFormat;
                cry.ExportToHttpResponse(formatType, Response, true, titleName);
                cry.Export();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string getDataDate(string dateVal) 
        {
            string data_date = "";
            switch (dateVal.ToLower())
            {
                case "today":
                    data_date = DateTime.Today.ToString("yyyy-MM-dd");
                    break;
                case "yesterday":
                    data_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case "thismonth":
                    DateTime today = DateTime.Now;
                    data_date = new DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd");
                    break;
                case "previousmonth":
                    data_date = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "thisweek":
                    data_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case "previousweek":
                    data_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                default:
                    data_date = dateVal;
                    break;
            }

            return data_date;
        }
    }
}