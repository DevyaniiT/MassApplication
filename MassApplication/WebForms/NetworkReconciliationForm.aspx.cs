using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MassApplication.WebForms
{
    public partial class NetworkReconciliationForm : System.Web.UI.Page
    {
        ReportDocument cry = new ReportDocument();
        string region, data_date, endCalcDate, units, duration, year, month, range, to_date, from_date, pipeline_id, type;
        string pdfFile = ConfigurationManager.AppSettings["pdfFile"];
        int report_id, flag;
        string img_path = ConfigurationManager.AppSettings["rptImagePath"];
        protected void Page_Init(object sender, EventArgs e)
        {
            //region = Request.QueryString["region"];
            data_date = Request.QueryString["data_date"];
            endCalcDate = Request.QueryString["data_date"];
            units = Request.QueryString["unit"];
            duration = Request.QueryString["duration"];
            year = Request.QueryString["year"];
            month = Request.QueryString["month"];
            range = Request.QueryString["day"];
            from_date = Request.QueryString["from_date"];
            to_date = Request.QueryString["to_date"];
            pipeline_id = Request.QueryString["pipeline_id"];
            type = Request.QueryString["type"];
            flag = Convert.ToInt32(Request.QueryString["flag"]);

            if (region == "" || region == null)
            {
                //region = ClassFolder.Login_Class.region_id;
                region = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34";
            }

            GetReportData();
        }

        public void GetReportData()
        {
            if (duration.ToLower() == "daily")
            {
                GetReportDailyData();
            }
            if (duration.ToLower() == "monthly" || duration.ToLower() == "monthlydaily")
            {
                GetReportMonthlyData();
            }
            if (duration.ToLower() == "quarterly")
            {
                GetReportQuarterlyData();
            }
            if (duration.ToLower() == "yearly" || duration.ToLower() == "yearlydaily")
            {
                GetReportYearlyData();
            }
            if (duration.ToLower() == "halfyearly" || duration.ToLower() == "halfyearlydaily")
            {
                GetReportHalfYearlyData();
            }
            if (duration.ToLower() == "fortnightly" || duration.ToLower() == "fortnightlydaily")
            {
                GetReportFortnightlyData();
            }
            if (duration.ToLower() == "custom" || duration.ToLower() == "customdaily")
            {
                GetReportCustomData();
            }
            else { }
        }

        public void GetReportDailyData()
        {
            report_id = get_reportid();
            string inputdata = duration;
            GetDurationData(data_date, endCalcDate, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportMonthlyData()
        {
            report_id = get_reportid();
            string StartDateVal = ClassFolder.DateClass.MonthlyStartDate(month, year);
            string EndDateVal = ClassFolder.DateClass.MonthlyEndDate(StartDateVal);
            string month_name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month));
            string inputdata = month_name + " " + year + "(" + duration + ")";
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportQuarterlyData()
        {
            report_id = get_reportid();
            string StartDateVal = ClassFolder.DateClass.QuaterlyStartDate(month, year);
            string EndDateVal = ClassFolder.DateClass.QuaterlyEndDate(month, year);
            DateTime strtDate = Convert.ToDateTime(StartDateVal);
            String m = strtDate.ToString("MMM");
            DateTime EndDate = Convert.ToDateTime(EndDateVal);
            String n = EndDate.ToString("MMM");
            string mon = m + "-" + n;
            string inputdata = mon + " " + year + "(" + duration + ")";
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportYearlyData()
        {
            report_id = get_reportid();
            string StartDateVal = ClassFolder.DateClass.YearlyStartDate(year);
            string EndDateVal = ClassFolder.DateClass.YearlyEndDate(year);
            string inputdata = year + "(" + duration + ")";
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportHalfYearlyData()
        {
            report_id = get_reportid();
            string StartDateVal = ClassFolder.DateClass.HalfYearlyStartDate(month, year);
            string EndDateVal = ClassFolder.DateClass.HalfYearlyEndDate(month, year);
            DateTime strtDate = Convert.ToDateTime(StartDateVal);
            String m = strtDate.ToString("MMM");
            DateTime EndDate = Convert.ToDateTime(EndDateVal);
            String n = EndDate.ToString("MMM");
            string mon = m + "-" + n;
            string inputdata = mon + " " + year + "(" + duration + ")";
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportFortnightlyData()
        {
            report_id = get_reportid();
            string StartDateVal = ClassFolder.DateClass.FortnightlyStartDate(range, month, year);
            string EndDateVal = ClassFolder.DateClass.FortnightlyEndDate(range, month, year);
            string month_name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(month));
            string inputdata = month_name + " " + year + "(" + duration + ")";
            //string inputdata = range + " " + MonthName + " " + YearValue + "(" + duration + ")";
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        public void GetReportCustomData()
        {
            report_id = get_reportid();
            string StartDateVal = from_date;
            string EndDateVal = to_date;
            string inputdata = duration;
            GetDurationData(StartDateVal, EndDateVal, duration, units, pipeline_id, report_id, inputdata);
        }
        
        int get_reportid()
        {
            int id = 37;
            if (type.ToLower().Equals("energy"))
            {
                id = 38;
            }
            if (type.ToLower().Equals("checkmetervolume"))
            {
                id = 21;
            }
            if (type.ToLower().Equals("checkmeterenergy"))
            {
                id = 22;
            }
            if (type.ToLower().Equals("exception"))
            {
                id = 7;
            }
            if (type.ToLower().Equals("groupwise"))
            {
                id = 27;
            }

            return id;
        }
        void GetDurationData(string start_date, string end_date, string duration, string unit_value, string pipeline_id, int report_id, string frequency)
        {
            if (unit_value == null)
            {
                unit_value = "";
            }
            Hashtable hashdata = ClassFolder.GetValue.GetHashReconcileValue(start_date, end_date, duration, unit_value, pipeline_id, region);
            cry = ClassFolder.GetValue.GenerateReport(report_id, pipeline_id, hashdata);
            string logo_path = System.Web.Hosting.HostingEnvironment.MapPath(img_path);

            cry.SetParameterValue("Duration", duration);
            cry.SetParameterValue("startCalDate", start_date);
            cry.SetParameterValue("endCalDate", end_date);
            cry.SetParameterValue("UnitFactor", unit_value);
            cry.SetParameterValue("Pipelinemst_id", ClassFolder.GetValue.GetPipelinename(pipeline_id));
            cry.SetParameterValue("image_url", logo_path);

            if (cry.HasRecords == false)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "NoRecordAlertMsg", "alert('There is no record found!');", true);
            }
            else
            {
                if (flag == 1)
                {
                    Network_Reconciliation.Visible = false;

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

                    ClassFolder.Email_Class.SendEmailerData();
                }

                else
                {
                    Network_Reconciliation.Visible = true;
                    Network_Reconciliation.ReportSource = cry;
                }
            }
        }
    }
}