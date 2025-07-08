using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassApplication.Models.Graph;
using Newtonsoft.Json;

namespace MassApplication.Controllers
{
    public class DynamicController : BaseController
    {
        public static string sliderVal = "0";
        public static string RefreshDuration = ConfigurationManager.AppSettings["refreshDuration"];
        public static string ODBC_Name = ConfigurationManager.AppSettings["ODBCName"];
        public static string ODBC_UserId = ConfigurationManager.AppSettings["user"];
        public static string ODBC_Password = ConfigurationManager.AppSettings["password"];
        public static string ODBC_Database = ConfigurationManager.AppSettings["database"];
        public static string ODBC_Name1 = ConfigurationManager.AppSettings["ODBCName1"];
        public static string ODBC_Database1 = ConfigurationManager.AppSettings["database1"];

        /// <summary>
        /// This method is used for showing dynamic report using menu_name
        /// </summary>
        /// <param name="menu_name"></param>
        /// Holds the title name of sub menu items
        /// <returns></returns>
        public ActionResult Index(string menu_name)
        {
            try
            {
                if (ClassFolder.Login_Class.access_token == null)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('No network assign...!'); window.location.href = 'HomePage'; </script>");
                }
                else
                {
                    if (schemapath == null)
                    {
                        schemapath = "public.";
                    }
                    int role_id = Convert.ToInt32(ClassFolder.Login_Class.role_id);
                    ViewBag.titlename = menu_name;
                    string action_name = menu_name.Replace(" ", String.Empty).ToLower();
                    ViewBag.UrlData = action_name;
                    ViewBag.RefreshVal = RefreshDuration;
                    ViewBag.SliderVal = sliderVal;
                    string sql = "";
                    DataTable dt = new DataTable();
                    string convertJson = "";
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    switch (action_name)
                    {
                        case "reports":
                            //sql = ClassFolder.Query_Class.sql_ReportQuery(schemapath);
                            sql = ClassFolder.Query_Class.sql_ReportById(schemapath, role_id);
                            break;
                        case "managereport":
                            //sql = ClassFolder.Query_Class.sql_ReportQueryData(schemapath);
                            sql = ClassFolder.Query_Class.sql_ReportById(schemapath, role_id);
                            break;
                        case "scheduletask":
                            sql = ClassFolder.Query_Class.sql_ScheduleQuery(schemapath);
                            break;
                        case "user":
                            sql = ClassFolder.Query_Class.sql_UserWiseQuery(schemapath);
                            break;
                        case "role":
                            sql = ClassFolder.Query_Class.sql_RoleWiseQuery(schemapath);
                            break;
                        case "query":
                            //sql = ClassFolder.Query_Class.sql_DBQuery(schemapath);
                            sql = ClassFolder.Query_Class.sql_QueryById(schemapath, role_id);
                            break;
                        case "dashboard":
                            //sql = ClassFolder.Query_Class.sql_DashboardQuery(schemapath);
                            sql = ClassFolder.Query_Class.sql_DashboardById(schemapath, role_id);
                            break;
                        case "kpi":
                            //sql = ClassFolder.Query_Class.sql_KPIQuery(schemapath);
                            sql = ClassFolder.Query_Class.sql_KPIById(schemapath, role_id);
                            break;
                        case "diagnosticsummary":
                            sql = ClassFolder.Query_Class.sql_DiagnosticSummaryQuery(schemapath);
                            break;
                        case "gc":
                            //string date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                            sql = ClassFolder.Query_Class.sql_GCQuery(schemapath, date);
                            break;
                        case "surveyor":
                            sql = ClassFolder.Query_Class.sql_SurveyQuery(schemapath, date);
                            break;
                    }
                    if (action_name == "diagnosticsummary" || action_name == "gc")
                    {
                        dt = ClassFolder.Database_Class.OpenODBCQuery(sql, ODBC_Name, ODBC_UserId, ODBC_Password, ODBC_Database);
                    }
                    else if (action_name == "surveyor")
                    {
                        dt = ClassFolder.Database_Class.OpenODBCQuery(sql, ODBC_Name1, ODBC_UserId, ODBC_Password, ODBC_Database1);
                    }
                    else
                    {
                        dt = ClassFolder.Database_Class.OpenQuery(sql);
                        if (action_name == "scheduletask") 
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string recurrenceData = dr["duration"].ToString();
                                //string frequencyData = dr["frequency_days"].ToString();
                                string frequencyData = dr["frequency"].ToString();
                                if (recurrenceData.ToLower() == "weekly") 
                                {
                                    frequencyData = frequencyData.Replace("0", "SUN");
                                    frequencyData = frequencyData.Replace("1", "MON");
                                    frequencyData = frequencyData.Replace("2", "TUE");
                                    frequencyData = frequencyData.Replace("3", "WED");
                                    frequencyData = frequencyData.Replace("4", "THURS");
                                    frequencyData = frequencyData.Replace("5", "FRI");
                                    frequencyData = frequencyData.Replace("6", "SAT");
                                    //dr["frequency_days"] = frequencyData;
                                    dr["frequency"] = frequencyData;
                                }
                            }
                        } 
                    }

                    convertJson = JsonConvert.SerializeObject(dt);
                    ViewBag.DatatableData = dt;
                    ViewBag.jsonData = convertJson;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Returns the json data for particular tab name(sub menu items name) 
        /// </summary>
        /// <param name="first_date"></param>
        /// Holds the start date 
        /// <param name="tab_name"></param>
        /// Holds the tab name(sub menu items name) 
        /// <returns></returns>
        public JsonResult GetDynamicData(string first_date, string tab_name)
        {
            try
            {
                string sql = "";
                DataTable dt = null;
                switch (tab_name.ToLower())
                {
                    case "diagnosticsummary":
                        sql = ClassFolder.Query_Class.sql_DiagnosticSummaryQuery(schemapath);
                        dt = ClassFolder.Database_Class.OpenODBCQuery(sql, ODBC_Name, ODBC_UserId, ODBC_Password, ODBC_Database);
                        break;
                    case "gc":
                        sql = ClassFolder.Query_Class.sql_GCQuery(schemapath, first_date);
                        dt = ClassFolder.Database_Class.OpenODBCQuery(sql, ODBC_Name, ODBC_UserId, ODBC_Password, ODBC_Database);
                        break;
                    case "surveyor":
                        sql = ClassFolder.Query_Class.sql_SurveyQuery(schemapath, first_date);
                        dt = ClassFolder.Database_Class.OpenODBCQuery(sql, ODBC_Name1, ODBC_UserId, ODBC_Password, ODBC_Database1);
                        break;
                }

                string convertJson = JsonConvert.SerializeObject(dt);
                ViewBag.DatatableData = dt;
                ViewBag.jsonData = convertJson;
                return Json(convertJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}