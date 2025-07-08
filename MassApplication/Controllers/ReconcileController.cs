using MassApplication.Models;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Controllers
{
    public class ReconcileController : BaseController
    {
        public static string sldURL = ConfigurationManager.AppSettings["SLDAddress"];
        public static string FromDate = "";
        public static string ToDate = "";
        public static string ReportName = "";
        public static string ReportOverallType = "";
        public static string ReportPipelineType = "";
        public static string ReportFrequency = "";
        public static string apiURL = "";
        public static string ParameterOverallData = "";
        public static string ParameterReconcileData = "";
        public static string ParameterData = "";
        public static int UnitValue = 0;
        public static string UnitFactor = "";
        public static dynamic overall_data;
        public static dynamic pipeline_data;

        /// <summary>
        /// This method is used for showing pipeline report using menu_name
        /// </summary>
        /// <param name="menu_name"></param>
        /// Defines a particular pipeline report name
        /// <returns></returns>
        public ActionResult Index(string menu_name)
        {
            if (ClassFolder.Login_Class.access_token == null || ClassFolder.Login_Class.access_token == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    ReportName = menu_name;
                    ViewBag.titlename = ReportName;
                    string action_name = ReportName.Replace(" ", String.Empty).ToLower();
                    //UnitFactor = GetUnitValue(action_name);
                    ViewBag.UrlData = action_name;
                    string username = ClassFolder.Login_Class.email_id;
                    string token = ClassFolder.Login_Class.access_token;
                    ReportFrequency = "daily";
                    FromDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    ToDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    UnitValue = 1000000;
                    var model = new InputClass();
                    get_menuData(action_name, username, token, ReportFrequency, FromDate, ToDate, model);

                    ViewBag.CurrentDate = FromDate;
                    if (action_name == "volumereconciliation" || action_name == "energyreconciliation")
                    {
                        var overall_response = CallGetAPIResource(apiURL, ParameterOverallData);
                        if (overall_response.IsSuccessStatusCode)
                        {
                            var overall_JsonContent = overall_response.Content.ReadAsStringAsync().Result;
                            var overall_tokenContent = getJSONdata(overall_JsonContent); 
                            var overall_pipe_data = overall_tokenContent["content"]["collection"];
                            //TempData["OverallData"] = overall_pipe_data;
                            overall_data = overall_pipe_data;
                            var pipeline_response = CallGetAPIResource(apiURL, ParameterReconcileData);
                            if (pipeline_response.IsSuccessStatusCode)
                            {
                                var pipeline_JsonContent = pipeline_response.Content.ReadAsStringAsync().Result;
                                var pipeline_tokenContent = getJSONdata(pipeline_JsonContent); 
                                var pipeline_pipe_data = pipeline_tokenContent["content"]["collection"];
                                //TempData["ReconciliationData"] = pipeline_pipe_data;
                                pipeline_data = pipeline_pipe_data;
                                return View(model);
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('" + pipeline_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                            }
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('" + overall_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                        }
                    }
                    else
                    {
                        var reconcile_response = CallGetAPIResource(apiURL, ParameterData);
                        if (reconcile_response.IsSuccessStatusCode)
                        {
                            var JsonContent = reconcile_response.Content.ReadAsStringAsync().Result;
                            var tokenContent = getJSONdata(JsonContent);
                            var pipe_data = tokenContent["content"]["collection"];
                            //TempData["ReconciliationData"] = pipe_data;
                            overall_data = null;
                            pipeline_data = pipe_data;
                            return View(model);
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('" + reconcile_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
                }
            }
        }

        /// <summary>
        /// This is a partial view for pipeline report table view
        /// that section is render when the user click on particular report
        /// Only that portion is loaded instead of entire page is loaded
        /// </summary>
        /// <param name="menuName"></param>
        /// Defines a particular pipeline report name
        /// <returns></returns>
        public ActionResult TableViewerData(string menuName)
        {
            ReportName = menuName;
            ViewBag.titlename = ReportName;
            string action_name = ReportName.Replace(" ", String.Empty).ToLower();
            ViewBag.UrlData = action_name;
            ViewBag.Fromdate = FromDate;
            ViewBag.Todate = ToDate;
            TempData["UnitValue"] = UnitValue;
            TempData["OverallData"] = overall_data;
            TempData["ReconciliationData"] = pipeline_data;
            return PartialView("_partialTableView");
        }

        /// <summary>
        /// This method is called when we click load button on pipeline reports and display a appropriate table data using these parameters
        /// </summary>
        /// <param name="titlename"></param>
        /// Holds the title name of pipeline report table
        /// <param name="unit"></param>
        /// Holds the unit factor of pipeline report table
        /// <param name="duration"></param>
        /// Holds the duration of pipeline report table
        /// <param name="fromDate"></param>
        /// Holds the start date of pipeline report table
        /// <param name="toDate"></param>
        /// Holds the end date of pipeline report table
        /// <param name="monthname"></param>
        /// Holds the month name of pipeline report table
        /// <param name="day"></param>
        /// Holds the day of pipeline report table
        /// <param name="yearname"></param>
        /// Holds the year of pipeline report table
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReconcileViewer(string titlename, string unit, string duration, string fromDate, string toDate, string monthname, string day, string yearname)
        {
            try 
            {
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                var input_model = new InputClass();
                ReportName = titlename;
                ViewBag.titlename = ReportName;
                string actionName = ReportName.Replace(" ", String.Empty).ToLower();
                ViewBag.UrlData = actionName;
                UnitFactor = unit;
                getReportValue(unit, duration, fromDate, toDate, monthname, day, yearname);
                get_menuData(actionName, username, token, ReportFrequency, FromDate, ToDate, input_model);
                if (actionName == "volumereconciliation" || actionName == "energyreconciliation")
                {
                    var overall_response = CallGetAPIResource(apiURL, ParameterOverallData);
                    if (overall_response.IsSuccessStatusCode)
                    {
                        var overall_JsonContent = overall_response.Content.ReadAsStringAsync().Result;
                        var overall_tokenContent = getJSONdata(overall_JsonContent); 
                        var overall_pipe_data = overall_tokenContent["content"]["collection"];
                        overall_data = overall_pipe_data;
                        //TempData["OverallData"] = overall_pipe_data;
                        var pipeline_response = CallGetAPIResource(apiURL, ParameterReconcileData);
                        if (pipeline_response.IsSuccessStatusCode)
                        {
                            var pipeline_JsonContent = pipeline_response.Content.ReadAsStringAsync().Result;
                            var pipeline_tokenContent = getJSONdata(pipeline_JsonContent); 
                            var pipeline_pipe_data = pipeline_tokenContent["content"]["collection"];
                            pipeline_data = pipeline_pipe_data;
                            //TempData["ReconciliationData"] = pipeline_pipe_data;
                            return RedirectToAction("TableViewerData", "Reconcile", new { @menuName = titlename });
                            //return PartialView("_partialTableView");

                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('" + pipeline_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                        }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + overall_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                    }
                }
                else
                {
                    var reconcile_response = CallGetAPIResource(apiURL, ParameterData);
                    if (reconcile_response.IsSuccessStatusCode)
                    {
                        var JsonContent = reconcile_response.Content.ReadAsStringAsync().Result;
                        var tokenContent = getJSONdata(JsonContent); 
                        var pipe_data = tokenContent["content"]["collection"];
                        //TempData["ReconciliationData"] = pipe_data;
                        overall_data = null;
                        pipeline_data = pipe_data;
                        return RedirectToAction("TableViewerData", "Reconcile", new { @menuName = titlename });
                        //return PartialView("_partialTableView");
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + reconcile_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
            }
          
        }

        /// <summary>
        /// This method is called when we click load button on exception reports and display a appropriate table data using these parameters
        /// </summary>
        /// <param name="titlename"></param>
        /// Holds the title name of exception report table
        /// <param name="fromDate"></param>
        /// Holds the start date of exception report table
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExceptionViewer(string titlename, string fromDate)
        {
            try 
            {
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                var input_model = new InputClass();
                ReportName = titlename;
                ViewBag.titlename = ReportName;
                string actionName = ReportName.Replace(" ", String.Empty).ToLower();
                ViewBag.UrlData = actionName;
                ReportFrequency = "";
                FromDate = fromDate;
                ToDate = fromDate;
                get_menuData(actionName, username, token, ReportFrequency, FromDate, ToDate, input_model);
                var reconcile_response = CallGetAPIResource(apiURL, ParameterData);
                if (reconcile_response.IsSuccessStatusCode)
                {
                    var JsonContent = reconcile_response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);  
                    var pipe_data = tokenContent["content"]["collection"];
                    UnitValue = 0;
                    overall_data = null;
                    pipeline_data = pipe_data;
                    return RedirectToAction("TableViewerData", "Reconcile", new { @menuName = titlename });
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + reconcile_response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = 'Index'; </script>");
            }

        }

        /// <summary>
        /// This method is used for tabular view where purchase, sales and IC is defined
        /// This function is redirect to new tab
        /// </summary>
        /// <param name="type"></param>
        /// Holds the type of report i.e, Volume or Energy
        /// <param name="from_date"></param>
        /// Holds the start date of report
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="unit"></param>
        /// Holds the unit of report
        /// <param name="frequency"></param>
        /// Holds the frequency(duration) of report
        /// <param name="check_id"></param>
        /// Holds the check id 
        /// <returns></returns>
        public ActionResult TableViewer(string type, string from_date, string to_date, string unit, string frequency, string check_id)
        {
            try
            {
                string titlename = type + " Reconciliation Dated: " + from_date;
                ViewBag.titlename = titlename;
                ViewBag.TitleType = type;
                ViewBag.UnitValue = unit;
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                int unitVal = getUnitValue(unit);
                TempData["UnitFactor"] = unitVal;
                string report_type = "";
                if (type.ToLower() == "volume" || type.ToLower() == "energy")
                {
                    report_type = "mass_overall_flow_data";
                }
                else
                {
                    report_type = "mass_checkmeter_flow_data";
                }
                string parameter_data = get_urlData(username, token, unitVal, from_date, to_date, frequency, type, check_id, report_type);
                var response = CallGetAPIResource("procedure/data", parameter_data);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);
                    var pipe_data = tokenContent["content"]["collection"];
                    TempData["ReconcileTableData"] = pipe_data;
                    return View();
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                }
                
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
            }
        }

        /// <summary>
        /// This method is used for chart view where graphs are defined
        /// This function is redirect to new tab 
        /// </summary>
        /// <param name="type"></param>
        /// Holds the type of report i.e, Volume or Energy
        /// <param name="from_date"></param>
        /// Holds the start date of report
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="frequency"></param>
        /// Holds the frequency(duration) of report
        /// <param name="unit"></param>
        ///  Holds the unit of report
        /// <param name="typename"></param>
        /// Holds the type of report i.e, Volume or Energy
        /// <param name="pipeline_id"></param>
        /// Holds the pipeline id 
        /// <param name="id"></param>
        ///  Holds the id 
        /// <param name="name"></param>
        ///  Holds the name
        /// <returns></returns>
        public ActionResult ChartViewer(string type, string from_date, string to_date, string frequency, string unit, string typename, string pipeline_id, string id, string name)
        {
            try
            {
                string titlename = name + " " + type + " Trend";
                ViewBag.titlename = titlename;
                ViewBag.typename = typename;
                string chartName = type + " Reconciliation";
                ViewBag.chartname = chartName;
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                int unitVal = getUnitValue(unit);
                ViewBag.UnitData = unitVal;
                ViewBag.UnitValue = unit;
                string api_URL_volume = "";
                string api_URL_energy = "";
                string parameter_chart1 = "";
                string parameter_chart2 = "";
                string parameter_chart3 = "";
                switch (typename)
                {
                    case "pipeline":
                        api_URL_volume = "reconcilation/getLossper";
                        api_URL_energy = "gascompress/graph";
                        parameter_chart1 = string.Format("?date={0}&mst_id={1}&prediction=yes&access_token={2}", from_date, pipeline_id, token);
                        parameter_chart2 = string.Format("?date={0}&mst_id={1}&access_token={2}", from_date, pipeline_id, token);
                        parameter_chart3 = getChartParameter(type, pipeline_id, from_date, to_date, frequency, username, token);
                        getChartData(type, typename, api_URL_volume, api_URL_energy, parameter_chart1, parameter_chart2, parameter_chart3);
                        break;

                    case "section":
                        api_URL_volume = "gascompress/graph";
                        api_URL_energy = api_URL_volume;
                        parameter_chart1 = string.Format("?type=mass_section_volume_reconcile&frequency={0}&date1={1}&date2={2}&username={3}&pipeline_id={4}&section_id={5}&access_token={6}", frequency, from_date, to_date, username, pipeline_id, id, token);
                        parameter_chart2 = parameter_chart1;
                        parameter_chart3 = string.Format("?type=mass_section_energy_reconcile&frequency={0}&date1={1}&date2={2}&pipeline_id={3}&username={4}&access_token={5}", frequency, from_date, to_date, pipeline_id, username, token);
                        getChartData(type, typename, api_URL_volume, api_URL_energy, parameter_chart1, parameter_chart2, parameter_chart3);
                        break;
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
            }
        }

        /// <summary>
        /// This method is used for SLD view 
        /// This function is redirect to new tab 
        /// </summary>
        /// <param name="type"></param>
        /// Holds the type of report i.e, Volume or Energy
        /// <param name="from_date"></param>
        /// Holds the start date of report
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="typename"></param>
        /// Holds the type name of report
        /// <param name="id"></param>
        /// Holds the id
        /// <returns></returns>
        public ActionResult SLDViewer(string type, string from_date, string to_date, string typename, string id)
        {
            try
            {
                string titlename = type + " Reconciliation Dated: " + from_date;
                ViewBag.titlename = titlename;
                ViewBag.SLDType = type;
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                string parameter_url = "";
                switch (typename)
                {
                    case "pipeline":
                        parameter_url = string.Format(sldURL + "sld/index.html?mode=view&pipeline_id={0}&type={1}&data_date={2}", id, typename, from_date);
                        break;

                    case "section":
                        parameter_url = string.Format(sldURL + "sld/index.html?mode=view&section_id={0}&type={1}&data_date={2}", id, typename, from_date);
                        break;
                }
                ViewBag.sldURL = parameter_url;
                return View();

            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
            }
        }

        /// <summary>
        /// This is a partial view for section wise report table view
        /// that portion is render when the user click on extend button in pipeline table
        /// Only that portion is loaded instead of entire page is loaded 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// Holds the particular row index 
        /// <param name="type"></param>
        /// Holds the type of pipeline report i.e, Volume or Energy
        /// <param name="from_date"></param>
        /// Holds the start date of report
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="unit"></param>
        /// Holds the unit of report
        /// <param name="frequency"></param>
        /// Holds the frequency(duration) of report
        /// <param name="pipeline_id"></param>
        /// Holds the id of pipeline
        /// <returns></returns>
        [HttpGet]
        public ActionResult partialSectionViewer(string rowIndex, string type, string from_date, string to_date, string unit, string frequency, string pipeline_id)
        {
            try
            {
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                var input_model = new InputClass();
                string titlename = type + " Reconciliation";
                ViewBag.titlename = titlename;
                string actionName = titlename.Replace(" ", String.Empty).ToLower();
                ViewBag.UrlData = actionName;
                int unitVal = getUnitValue(unit);
                TempData["UnitValue"] = unitVal;
                string report_type = "";
                if (type == "Volume" || type == "volume") 
                {
                    report_type = "mass_section_volume_reconcile";
                }
                else if (type == "Energy" || type == "energy")
                {
                    report_type = "mass_section_energy_reconcile";
                }
                string parameterData = string.Format("?type={0}&frequency={1}&date1={2}&date2={3}&username={4}&pipeline_id={5}&access_token={6}", report_type, frequency, from_date, to_date, username, pipeline_id, token);

                string api_url = "gascompress/graph";
                var response = CallGetAPIResource(api_url, parameterData);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);
                    var section_data = tokenContent["content"]["collection"];
                    TempData["SectionData"] = section_data;
                    TempData["SectionType"] = type;
                    TempData["SectionIndex"] = rowIndex;
                    return RedirectToAction("TableViewerData", "Reconcile", new { @menuName = titlename });
                    //return PartialView("_partialTableView");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
            }

        }

        /// <summary>
        /// Returns the unit factor in number of given unit value 
        /// </summary>
        /// <param name="value"></param>
        /// Holds the unit value
        /// <returns></returns>
        public int getUnitValue(string value)
        {
            if (value.ToLower() == "mmscm" || value.ToLower() == "mmbtu")
            {
                return 1000000;
            }
            else if (value.ToLower() == "kscm" || value.ToLower() == "kbtu")
            {
                return 1000;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Returns the unit factor in string format of given unit value  
        /// </summary>
        /// <param name="actiondata"></param>
        /// Holds the title of pipeline report
        /// <param name="unit"></param>
        /// Holds the unit value
        /// <returns></returns>
        public string GetUnitValue(string actiondata, int unit)
        {
            if (actiondata.ToLower() == "energyreconciliation" || actiondata.ToLower() == "energy")
            {
                if (unit == 1000000)
                {
                    return "MMBTU";
                }
                else if (unit == 1000)
                {
                    return "KBTU";
                }
                else
                {
                    return "BTU";
                }
            }
            else
            {
                if (unit == 1000000)
                {
                    return "MMSCM";
                }
                else if (unit == 1000)
                {
                    return "KSCM";
                }
                else
                {
                    return "SCM";
                }
            }
        }

        /// <summary>
        /// Set the values of ParameterOverallData and ParameterReconcileData that is used for call Api with parameter fields
        /// </summary>
        /// <param name="action"></param>
        /// Defines the title of report
        /// <param name="userName"></param>
        /// Defines the user name of report
        /// <param name="tokenValue"></param>
        /// Defines the token value
        /// <param name="reportFrequency"></param>
        /// Defines the Frequency of report
        /// <param name="fromDate"></param>
        /// Defines the from date of report
        /// <param name="toDate"></param>
        /// Defines the to date of report
        /// <param name="objModel"></param>
        /// Defines the instance of the Input Class
        public void get_menuData(string action, string userName, string tokenValue, string reportFrequency, string fromDate, string toDate, dynamic objModel)
        {
            string report_overall_type = "";
            string report_type = "";
            switch (action)
            {
                case "volumereconciliation":
                    report_overall_type = "mass_overall_volume_reconcile";
                    report_type = "mass_pipeline_volume_reconcile";
                    apiURL = "gascompress/graph";
                    objModel.AvailableSummaryPeriod = GetSummaryPeriod();
                    objModel.AvailableMonth = GetMonthList();
                    objModel.AvailableHour = GetHoursList();
                    objModel.Years = GetYears();
                    ViewBag.UnitData = GetVolumeUnit();
                    ParameterOverallData = string.Format("?type={0}&frequency={1}&date1={2}&date2={3}&username={4}&access_token={5}", report_overall_type, reportFrequency, fromDate, toDate, userName, tokenValue);
                    ParameterReconcileData = string.Format("?type={0}&frequency={1}&date1={2}&date2={3}&username={4}&access_token={5}", report_type, reportFrequency, fromDate, toDate, userName, tokenValue);
                    break;
                case "energyreconciliation":
                    report_overall_type = "mass_overall_energy_reconcile";
                    report_type = "mass_pipeline_energy_reconcile";
                    apiURL = "gascompress/graph";
                    objModel.AvailableSummaryPeriod = GetSummaryPeriod();
                    objModel.AvailableMonth = GetMonthList();
                    objModel.AvailableHour = GetHoursList();
                    objModel.Years = GetYears();
                    ViewBag.UnitData = GetEnergyUnit();
                    ParameterOverallData = string.Format("?type={0}&frequency={1}&date1={2}&date2={3}&username={4}&access_token={5}", report_overall_type, reportFrequency, fromDate, toDate, userName, tokenValue);
                    ParameterReconcileData = string.Format("?type={0}&frequency={1}&date1={2}&date2={3}&username={4}&access_token={5}", report_type, reportFrequency, fromDate, toDate, userName, tokenValue);
                    break;
                case "checkmetervolumereconciliation":
                    report_type = "mass_checkmeter_volume_reconcile";
                    apiURL = "procedure/data";
                    objModel.AvailableSummaryPeriod = GetSummaryPeriod();
                    objModel.AvailableMonth = GetMonthList();
                    objModel.AvailableHour = GetHoursList();
                    objModel.Years = GetYears();
                    ViewBag.UnitData = GetVolumeUnit();
                    ParameterData = string.Format("?query_id=0&username={0}&varstartCalDate={1}&varendCalDate={2}&type={3}&frequency={4}&access_token={5}", userName, fromDate, toDate, report_type, reportFrequency, tokenValue);
                    break;
                case "checkmeterenergyreconciliation":
                    report_type = "mass_checkmeter_energy_reconcile";
                    apiURL = "procedure/data";
                    objModel.AvailableSummaryPeriod = GetSummaryPeriod();
                    objModel.AvailableMonth = GetMonthList();
                    objModel.AvailableHour = GetHoursList();
                    objModel.Years = GetYears();
                    ViewBag.UnitData = GetEnergyUnit();
                    ParameterData = string.Format("?query_id=0&username={0}&varstartCalDate={1}&varendCalDate={2}&type={3}&frequency={4}&access_token={5}", userName, fromDate, toDate, report_type, reportFrequency, tokenValue);
                    break;
                case "exceptionreconciliation":
                    apiURL = "exceptions/getdata";
                    ParameterData = string.Format("?data_dat={0}&username={1}&access_token={2}", fromDate, userName, tokenValue);
                    break;
                case "reconciliationtrend":
                    report_type = "";
                    break;
                case "groupwiselinepack":
                    report_type = "mass_pipeline_groupsummary_linepack_data";
                    apiURL = "procedure/data";
                    ParameterData = string.Format("?query_id=0&username={0}&varstartCalDate={1}&varendCalDate={2}&type={3}&frequency=daily&access_token={5}", userName, fromDate, toDate, report_type, reportFrequency, tokenValue);
                    break;
            }
        }

        /// <summary>
        /// Sets the start and end date of report using FromDate and ToDate variables
        /// </summary>
        /// <param name="unit_value"></param>
        /// Defines the unit value of report
        /// <param name="durationVal"></param>
        /// Defines the duration(frequency) of report
        /// <param name="from_date"></param>
        /// Defines the start date of report
        /// <param name="to_date"></param>
        /// Defines the end date of report
        /// <param name="month_name"></param>
        /// Defines the month of report
        /// <param name="dayVal"></param>
        /// Defines the day value of report
        /// <param name="year_name"></param>
        /// Defines the year of report
        public void getReportValue(string unit_value, string durationVal, string from_date, string to_date, string month_name, string dayVal, string year_name)
        {
            UnitValue = getUnitValue(unit_value);
            ReportFrequency = durationVal.ToLower();
            TempData["UnitValue"] = UnitValue;
            ViewBag.CurrentDate = FromDate;

            if (durationVal.ToLower() == "daily")
            {
                FromDate = from_date;
                ToDate = to_date;
            }
            if (durationVal.ToLower() == "hourly")
            {
                FromDate = from_date;
                ToDate = from_date;
            }
            if (durationVal.ToLower() == "fortnightly")
            {
                string day = dayVal;
                string fortnightly_month = month_name;
                string fortnightly_year = year_name;

                FromDate = ClassFolder.DateClass.FortnightlyStartDate(day, fortnightly_month, fortnightly_year);
                ToDate = ClassFolder.DateClass.FortnightlyEndDate(day, fortnightly_month, fortnightly_year);

            }
            if (durationVal.ToLower() == "monthly")
            {
                string monthly_month = month_name;
                string monthly_year = year_name;
                FromDate = ClassFolder.DateClass.MonthlyStartDate(monthly_month, monthly_year);
                ToDate = ClassFolder.DateClass.MonthlyEndDate(FromDate);

            }
            if (durationVal.ToLower() == "quarterly")
            {
                string quarterly_month = month_name;
                string quarterly_year = year_name;
                FromDate = ClassFolder.DateClass.QuaterlyStartDate(quarterly_month, quarterly_year);
                ToDate = ClassFolder.DateClass.QuaterlyEndDate(quarterly_month, quarterly_year);
            }
            if (durationVal.ToLower() == "halfyearly")
            {
                string halfyearly_month = month_name;
                string halfyearly_year = year_name;
                FromDate = ClassFolder.DateClass.HalfYearlyStartDate(halfyearly_month, halfyearly_year);
                ToDate = ClassFolder.DateClass.HalfYearlyEndDate(halfyearly_month, halfyearly_year);
            }
            if (durationVal.ToLower() == "yearly")
            {
                string yearly_year = year_name;

                FromDate = ClassFolder.DateClass.YearlyStartDate(yearly_year);
                ToDate = ClassFolder.DateClass.YearlyEndDate(yearly_year);
            }
            if (durationVal.ToLower() == "custom")
            {
                FromDate = from_date;
                ToDate = to_date;
            }
        }
       
        /// <summary>
        /// Reyurns the url parameter of the pipeline or section report data
        /// </summary>
        /// <param name="userName"></param>
        /// Holds the user name of the report
        /// <param name="tokendata"></param>
        /// Holds the token value of report
        /// <param name="unit"></param>
        /// Holds the unit of report
        /// <param name="from_date"></param>
        /// Holds the start date of report
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="frequency"></param>
        /// Holds the frequency(duration) of report
        /// <param name="type"></param>
        /// Holds the type of report i.e, Volume or Energy
        /// <param name="id"></param>
        /// Holds the id the report
        /// <param name="reportType"></param>
        /// Holds the report type i.e, pipeline or section
        /// <returns></returns>
        public string get_urlData(string userName, string tokendata, int unit, string from_date, string to_date, string frequency, string type, string id, string reportType)
        {
            string url_parameter = "";
            if (type.ToLower() == "volume" || type.ToLower() == "energy")
            {
                url_parameter = string.Format("?query_id=26&username={0}&varUnitFactor={1}&varstartCalDate={2}&varendCalDate={3}&type={4}&frequency={5}&access_token={6}", userName, unit, from_date, to_date, reportType, frequency, tokendata);
            }
            else
            {
                url_parameter = string.Format("?query_id=26&username={0}&varUnitFactor={1}&varstartCalDate={2}&varendCalDate={3}&type={4}&frequency={5}&varSection_id={6}&access_token={7}", userName, unit, from_date, to_date, reportType, frequency, id, tokendata);
            }

            return url_parameter;
        
        }
        
        /// <summary>
        /// Return the chart parameter of report for displaying Chart Viewer
        /// </summary>
        /// <param name="type"></param>
        /// Holds the type of report
        /// <param name="id"></param>
        /// Holds the id parameter value
        /// <param name="from_date"></param>
        /// Holds the start date of report 
        /// <param name="to_date"></param>
        /// Holds the end date of report
        /// <param name="frequency"></param>
        /// Holds the frequency(duration) of report
        /// <param name="username"></param>
        /// Holds the user name of report
        /// <param name="token"></param>
        /// Holds the token value
        /// <returns></returns>
        public string getChartParameter(string type, string id, string from_date, string to_date, string frequency, string username, string token)
        {
            string parameterList = "";
            if (type.ToLower() == "energy")
            {
                parameterList = string.Format("?type=mass_pipeline_energy_reconcile&pipeline_id={0}&frequency={1}&date1={2}&date2={3}&username={4}&access_token={5}", id, frequency, from_date, to_date, username, token);
            }
            else if (type.ToLower() == "check meter energy")
            {
                to_date = from_date;
                from_date = Convert.ToDateTime(from_date).AddDays(2).AddMonths(-1).ToString("yyyy-MM-dd");
                parameterList = string.Format("?gc_id={0}&table={1}&type=CheckMeter&date1={2}&date2={3}&access_token={4}", id, "reconcilation_check_meter_energy_daily", from_date, to_date, token);
            }                    
            else if (type.ToLower() == "check meter volume")
            {
                to_date = from_date;
                from_date = Convert.ToDateTime(from_date).AddDays(2).AddMonths(-1).ToString("yyyy-MM-dd");
                parameterList = string.Format("?gc_id={0}&table={1}&type=CheckMeter&date1={2}&date2={3}&access_token={4}", id, "reconcilation_check_meter_volume_daily", from_date, to_date, token);
            }
            else { }

            return parameterList;
        }

        /// <summary>
        /// Storing the API response data on ViewBag for Chart Viewer 
        /// </summary>
        /// <param name="type"></param>
        /// Holds the type of the report
        /// <param name="content_type"></param>
        /// Holds the content type of report
        /// <param name="api_url_volume"></param>
        /// Holds the api url for volume data
        /// <param name="api_url_energy"></param>
        /// Holds the api url for energy data
        /// <param name="parameter_chartdata1"></param>
        /// Holds the parameter data for chart1
        /// <param name="parameter_chartdata2"></param>
        /// Holds the parameter data for chart2
        /// <param name="parameter_chartdata3"></param>
        /// Holds the parameter data for chart3
        public void getChartData(string type, string content_type, string api_url_volume, string api_url_energy, string parameter_chartdata1, string parameter_chartdata2, string parameter_chartdata3) 
        {
            try 
            {
                if (type.ToLower() == "volume")
                {
                    var chart1_response = CallGetAPIResource(api_url_volume, parameter_chartdata1);
                    if (chart1_response.IsSuccessStatusCode)
                    {
                        var chart1_JsonContent = chart1_response.Content.ReadAsStringAsync().Result;
                        var chart1_tokenContent = getJSONdata(chart1_JsonContent);
                        var chart1_Message = chart1_tokenContent["content"]["error"];
                        ViewBag.ChartData1_Message = chart1_Message;
                        //var chart1_data = "";
                        switch (content_type) 
                        {
                            case "pipeline":
                                if (chart1_Message == false)
                                {
                                    var chart1_data = chart1_tokenContent["content"]["collection"]["a"];
                                    ViewBag.ChartData1 = chart1_data;
                                }
                                break;
                            case "section":
                                if (chart1_Message == false)
                                {
                                    var chart1_data = chart1_tokenContent["content"]["collection"];
                                    ViewBag.ChartData1 = chart1_data;
                                }
                                break;

                        }
                        var chart2_response = CallGetAPIResource(api_url_volume, parameter_chartdata2);
                        if (chart2_response.IsSuccessStatusCode)
                        {
                            var chart2_JsonContent = chart2_response.Content.ReadAsStringAsync().Result;
                            var chart2_tokenContent = getJSONdata(chart2_JsonContent);
                            var chart2_Message = chart2_tokenContent["content"]["error"];
                            ViewBag.ChartData2_Message = chart2_Message;
                            //var chart2_data;
                            switch (content_type)
                            {
                                case "pipeline":
                                    if (chart2_Message == false)
                                    {
                                        var chart2_data = chart2_tokenContent["content"]["collection"]["a"];
                                        ViewBag.ChartData2 = chart2_data;
                                    }
                                    break;
                                case "section":
                                    if (chart2_Message == false)
                                    {
                                        var chart2_data = chart2_tokenContent["content"]["collection"];
                                        ViewBag.ChartData2 = chart2_data;
                                    }
                                    break;

                            }
                        }
                    }

                }
                else
                {
                    //string parameter_chartdata = getChartParameter(type, id, from_date, to_date, frequency, username, token);

                    var chart1_response = CallGetAPIResource(api_url_energy, parameter_chartdata3);
                    if (chart1_response.IsSuccessStatusCode)
                    {
                        var chart1_JsonContent = chart1_response.Content.ReadAsStringAsync().Result;
                        var chart1_tokenContent = getJSONdata(chart1_JsonContent);
                        var chart1_Message = chart1_tokenContent["content"]["error"];
                        ViewBag.ChartData1_Message = chart1_Message;
                        if (chart1_Message == false)
                        {
                            var chart1_data = chart1_tokenContent["content"]["collection"];
                            ViewBag.ChartData1 = chart1_data;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
               
        }

    }
}