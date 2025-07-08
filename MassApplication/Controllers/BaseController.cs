using MassApplication.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MassApplication.Controllers
{
    public class BaseController : Controller
    {
        public static string Baseurl = ConfigurationManager.AppSettings["BaseAddress"];
        public static int roleId = Convert.ToInt32(ClassFolder.Login_Class.role_id);
        public static string schemapath = ConfigurationManager.AppSettings["SchemaPath"];
        public static string constring = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
        public static string region = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34";

        /// <summary>
        /// This method is used when user successfully login and enter the home page where default dashboard shows 
        /// This method redirect to HomeDashboard View of Dashboard Controller 
        /// </summary>
        /// <returns></returns>
        public ActionResult HomePage()
        {
            try 
            {
                if (ClassFolder.Login_Class.access_token == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ClassFolder.Login_Class.region_id = region;
                    return Redirect("~/Dashboard/HomeDashboard");
                    //return Content("<script language='javascript' type='text/javascript' src='/Scripts/SweetAlert2/sweetalert2.all.min.js'>Swal.fire({position: 'top-end',title: '" + ClassFolder.Login_Class.login_status + "',showConfirmButton: false,timer: 800}, function() { window.location.href = '~/Dashboard/HomeDashboard';});</script>");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '~/Login/Index'; </script>");
            }
        }

        /// <summary>
        /// This method called a partial view of dashboard table list in the sub menu items of View dashboard menu item
        /// </summary>
        /// <returns></returns>
        public ActionResult DashboardViewer()
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Dashboard_class db = new Dashboard_class();
                int role_id = Convert.ToInt32(ClassFolder.Login_Class.role_id);
                List<Dashboard_class> DbItems = db.getAllDashboardItems(schemapath, role_id);
                //List<Dashboard_class> DbItems = db.getAllRoleWiseDashboardItems(schemapath, role_id);
                ViewBag.DashboardViewerItems = DbItems;
                ViewBag.ActiveDashboard = int.Parse(ClassFolder.Login_Class.active_dashboard);
                //ViewBag.ActiveDashboard = int.Parse(ClassFolder.Login_Class.dashboard_Id);
                
                return PartialView("_partialDashboardViewer");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method called a partial view of menu table list 
        /// Used for dynamically created menu items
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuViewer()
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }

                Menu_Class db = new Menu_Class();
                int role_id = Convert.ToInt32(ClassFolder.Login_Class.role_id);
                List<Menu_Class> menuItems = db.getItems(schemapath, role_id.ToString());
                ViewBag.MenuViewerItems = menuItems;
                return PartialView("_partialMenuViewer");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Returns the html format for dynamically created menu items
        /// </summary>
        /// <param name="id"></param>
        /// Holds the parent id
        /// <returns></returns>
        public string MenuIDViewer(int id)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string menuHtml = "";
                int role_id = Convert.ToInt32(ClassFolder.Login_Class.role_id);
                string query = ClassFolder.Query_Class.sql_menuByID(schemapath, id, role_id);
                DataTable headingMenus = ClassFolder.Database_Class.OpenQuery(query);
                menuHtml = "<ul class='mb-0 list-inline'>";
                foreach (DataRow row in headingMenus.Rows)
                {
                    if (row["allow_read"].ToString().ToLower() == "true" && row["allow_create"].ToString().ToLower() == "true" && row["allow_update"].ToString().ToLower() == "true" && row["allow_delete"].ToString().ToLower() == "true")
                    {
                        menuHtml += "<li class='list-inline-item' title='" + row["menu_name"].ToString() + "'>";
                        //menuHtml += "<a href='" + row["menu_route"].ToString().Replace("~", "") + "'><img src='" + row["menu_icon"].ToString().Replace("~", "") + "' style='width:30px'><p class='side_span'>" + row["menu_name"].ToString() + "</p></a>";
                        menuHtml += "<a href='" + row["menu_route"].ToString().Replace("~", "..") + "'><img src='" + row["menu_icon"].ToString().Replace("~", "..") + "' style='width:30px'><p class='side_span'>" + row["menu_name"].ToString() + "</p></a>";
                        menuHtml += "</li>";
                    }

                }
                menuHtml += "</ul>";

                return menuHtml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Returns the list of query type
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> getQueryType()
        {
            return new List<SelectListItem>
            {
             new SelectListItem {Text = "Static", Value = "Static"},
             new SelectListItem {Text = "Dynamic", Value = "Dynamic"},
             new SelectListItem {Text = "Diagnostic", Value = "Diagnostic"},
            };
        }

        /// <summary>
        /// Returns true or false corresponsing to the status of the connection table data
        /// </summary>
        /// <param name="conn_class"></param>
        /// Holds the data of connnection table using Connection_Class class
        /// <returns></returns>
        public static bool getConnectionStatus(Connection_Class conn_class)
        {
            try
            {
                bool status;
                int connection_id;
                string dsn, user_id, password, database_name, conn_sql;

                connection_id = conn_class.connection_id;
                dsn = conn_class.dsn_name.Trim();
                user_id = conn_class.user_id.Trim();
                password = conn_class.password.Trim();
                database_name = conn_class.db_schema_name.Trim();

                conn_sql = "UPDATE " + schemapath + "dbconnection_mst Set connection_name='" + dsn + "', dsn_name='" + dsn + "',user_id='" + user_id + "',password='" + password + "',db_schemaname='" + database_name + "' WHERE id = " + connection_id + "";
                status = ClassFolder.Database_Class.ExecuteQuery(conn_sql);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the list of dashboard name
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public static List<SelectListItem> getAllDashboardName(string schema, int roleID)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
                using (NpgsqlConnection con = new NpgsqlConnection(constr))
                {
                    string query = ClassFolder.Query_Class.sql_DashboardDDById(schema, roleID);

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["dashboard_id"].ToString(),
                                    Value = sdr["dashboard_name"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Returns the list of role name
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the name of schema of database
        /// <returns></returns>
        public static List<SelectListItem> getAllRoleName(string schema)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
                using (NpgsqlConnection con = new NpgsqlConnection(constr))
                {
                    string query = ClassFolder.Query_Class.sql_RoleName(schema);

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["role_id"].ToString(),
                                    Value = sdr["role_name"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Returns the list of role name
        /// </summary>
        /// <param name="schema"></param>
        ///  Holds the name of schema of database
        /// <returns></returns>
        public static List<SelectListItem> getAllUserTypeName(string schema)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
                using (NpgsqlConnection con = new NpgsqlConnection(constr))
                {
                    string query = ClassFolder.Query_Class.sql_UserTypeName(schema);

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["role_id"].ToString(),
                                    Value = sdr["role_name"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Returns the response of the API method
        /// </summary>
        /// <param name="parameter_url"></param>
        /// Holds the parameter url
        /// <param name="parameter_data"></param>
        /// Holds the parameter data
        /// <returns></returns>
        public dynamic getCallFunction(string parameter_url, string parameter_data)
        {
            try
            {
                var jss_data = (dynamic)null;
                var response = CallGetAPIResource(parameter_url, parameter_data);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);
                    jss_data = tokenContent["content"]["collection"];
                }
                return jss_data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the response of the API method
        /// </summary>
        /// <param name="parameter_url"></param>
        /// Holds the parameter url
        /// <param name="parameter_data"></param>
        /// Holds the parameter data
        /// <returns></returns>
        public dynamic getCallResponse(string parameter_url, string parameter_data)
        {
            try
            {
                var jss_data = (dynamic)null;
                var response = CallGetAPIResource(parameter_url, parameter_data);
                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);
                    //jss_data = tokenContent["content"]["collection"];
                    jss_data = tokenContent;
                }
                return jss_data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns the deserialize content of the response of API method
        /// </summary>
        /// <param name="JsonContent"></param>
        /// Holds the response of API method
        /// <returns></returns>
        public dynamic getJSONdata(dynamic JsonContent)
        {
            var jss = new JavaScriptSerializer();
            var tokenContent = jss.Deserialize<dynamic>(JsonContent);
            return tokenContent;
        }

        /// <summary>
        /// This method is called a partial View for section data
        /// This method is called when we change the network according to that selected network its section name shows in checkbox list tag
        /// </summary>
        /// <param name="PipelineId"></param>
        /// Holds the id of pipeline which is selected 
        /// <returns></returns>
        public PartialViewResult GetDropDwnData(string PipelineId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
            string query = ClassFolder.RptParams_Class.query_val;
            string function_script = "";
            string region = PipelineId;
            if (query == null || region == "")
            {
                ViewBag.ActualData = "";
            }
            else
            {
                using (NpgsqlConnection con = new NpgsqlConnection(constr))
                {
                    string function_str = (query);
                    string sPattern, Correctval;
                    Regex regex = new Regex(@"\[var([a-zA-Z0-9_\.\-]+)\]");
                    MatchCollection matchCollection = regex.Matches(query);
                    foreach (Match match in matchCollection)
                    {
                        string longid = match.ToString();
                        string input = region;

                        sPattern = match.Value.Replace("[", "\\[");
                        sPattern = sPattern.Replace("]", "\\]");

                        Correctval = input;
                        function_str = Regex.Replace(function_str, sPattern, Correctval);

                        function_script = function_str;
                    }
                    using (NpgsqlCommand cmd = new NpgsqlCommand(function_script))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Text = sdr["id"].ToString(),
                                    Value = sdr["value"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
                ViewBag.ActualData = items;
            }

            return PartialView("_partialDropdwnSection", items);
        }

        /// <summary>
        /// Returns the list of user type role 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetUserTypeDD()
        {
            return new List<SelectListItem>
            {
             new SelectListItem {Text = "Public", Value = "0", Selected=true},
             new SelectListItem {Text = "Private", Value = "2"},
             new SelectListItem {Text = "System KPI", Value = "1"},
            };
        }

        /// <summary>
        /// Returns the list of summary wise duration list
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetSummaryPeriod()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Hourly", Value = "hourly"},
            new SelectListItem {Text = "Daily", Value = "daily", Selected=true},
            new SelectListItem {Text = "Fortnightly", Value = "fortnightly"},
            new SelectListItem {Text = "Monthly", Value = "monthly"},
            new SelectListItem {Text = "Quarterly", Value = "quarterly"},
            new SelectListItem {Text = "HalfYearly", Value = "halfyearly"},
            new SelectListItem {Text = "Yearly", Value = "yearly"},
            new SelectListItem {Text = "Custom", Value = "custom"},
        };
        }

        /// <summary>
        /// Returns the list of volume wise unit 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetVolumeUnit()
        {
            return new List<SelectListItem>
            {
             //new SelectListItem {Text = "SELECT", Value = "0: select"},
             new SelectListItem {Text = "MMSCM", Value = "MMSCM", Selected=true},
             new SelectListItem {Text = "KSCM", Value = "KSCM"},
             new SelectListItem {Text = "SCM", Value = "SCM"},
            };
        }

        /// <summary>
        /// Returns the list of energy wise unit 
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetEnergyUnit()
        {
            return new List<SelectListItem>
            {
             //new SelectListItem {Text = "SELECT", Value = "0: select"},
             new SelectListItem {Text = "MMBTU", Value = "MMBTU", Selected=true},
             new SelectListItem {Text = "KBTU", Value = "KBTU"},
             new SelectListItem {Text = "BTU", Value = "BTU"},
            };
        }

        /// <summary>
        /// Returns the list of unit in integer format
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetUnits()
        {
            return new List<SelectListItem>
            {
             new SelectListItem {Text = "1000000", Value = "1000000", Selected=true},
             new SelectListItem {Text = "1000", Value = "1000"},
             new SelectListItem {Text = "1", Value = "1"},
            };
        }

        /// <summary>
        /// Returns the list of month name
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetMonthList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "January", Value = "01", Selected=true},
            new SelectListItem {Text = "February", Value = "02"},
            new SelectListItem {Text = "March", Value = "03"},
            new SelectListItem {Text = "April", Value = "04"},
            new SelectListItem {Text = "May", Value = "05"},
            new SelectListItem {Text = "June", Value = "06"},
            new SelectListItem {Text = "July", Value = "07"},
            new SelectListItem {Text = "August", Value = "08"},
            new SelectListItem {Text = "September", Value = "09"},
            new SelectListItem {Text = "October", Value = "10"},
            new SelectListItem {Text = "November", Value = "11"},
            new SelectListItem {Text = "December", Value = "12"},
        };
        }

        /// <summary>
        /// Returns the list of hours
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetHoursList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "00:00", Value = "00:00", Selected=true},
            new SelectListItem {Text = "01:00", Value = "01:00"},
            new SelectListItem {Text = "02:00", Value = "02:00"},
            new SelectListItem {Text = "03:00", Value = "03:00"},
            new SelectListItem {Text = "04:00", Value = "04:00"},
            new SelectListItem {Text = "05:00", Value = "05:00"},
            new SelectListItem {Text = "06:00", Value = "06:00"},
            new SelectListItem {Text = "07:00", Value = "07:00"},
            new SelectListItem {Text = "08:00", Value = "08:00"},
            new SelectListItem {Text = "09:00", Value = "09:00"},
            new SelectListItem {Text = "10:00", Value = "10:00"},
            new SelectListItem {Text = "11:00", Value = "11:00"},
            new SelectListItem {Text = "12:00", Value = "12:00"},
            new SelectListItem {Text = "13:00", Value = "13:00"},
            new SelectListItem {Text = "14:00", Value = "14:00"},
            new SelectListItem {Text = "15:00", Value = "15:00"},
            new SelectListItem {Text = "16:00", Value = "16:00"},
            new SelectListItem {Text = "17:00", Value = "17:00"},
            new SelectListItem {Text = "18:00", Value = "18:00"},
            new SelectListItem {Text = "19:00", Value = "19:00"},
            new SelectListItem {Text = "20:00", Value = "20:00"},
            new SelectListItem {Text = "21:00", Value = "21:00"},
            new SelectListItem {Text = "22:00", Value = "22:00"},
            new SelectListItem {Text = "23:00", Value = "23:00"}
        };
        }
        
        /// <summary>
        /// Returns the list of the last 4 years 
        /// </summary>
        /// <returns></returns>
        public static List<int> GetYears()
        {
            List<int> Years = new List<int>();
            DateTime startYear = DateTime.Now;
            while (DateTime.Now.AddYears(-4).Year <= startYear.Year)
            {
                Years.Add(startYear.Year);
                startYear = startYear.AddYears(-1);
            }

            //ViewBag.Years = Years;
            return Years;

        }
        /// <summary>
        /// This method is used for getting days i.e, from 1 to 31
        /// </summary>
        /// <returns></returns>
        public static List<int> GetDays()
        {
            List<int> daysVal = new List<int>();
            for (int i = 1; i <= 31; i++)
            {
                daysVal.Add(i);
            }
            return daysVal;
        }
        /// <summary>
        /// This method is used for getting week names
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetWeekList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Sunday", Value = "0", Selected=true},
            new SelectListItem {Text = "Monday", Value = "1"},
            new SelectListItem {Text = "Tuesday", Value = "2"},
            new SelectListItem {Text = "Wednesday", Value = "3"},
            new SelectListItem {Text = "Thursday", Value = "4"},
            new SelectListItem {Text = "Friday", Value = "5"},
            new SelectListItem {Text = "Saturday", Value = "6"},
        };
        }

        /// <summary>
        /// Used for calling API POST method 
        /// Returns the response of the API Method
        /// </summary>
        /// <param name="api_url"></param>
        /// Holds the url of the api method
        /// <param name="HeaderList"></param>
        /// Holds the list of header portion
        /// <param name="RequestList"></param>
        /// Holds the list of requested data
        /// <returns></returns>
        public static HttpResponseMessage CallPostAPIResource(string api_url, List<KeyValuePair<string, string>> HeaderList, Dictionary<string, string> RequestList)
        {
            try 
            {
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (KeyValuePair<string, string> pair in HeaderList)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(pair.Key, pair.Value);
                }

                var RequestBody = RequestList;
                var APIResponse = client.PostAsync(Baseurl + api_url, new FormUrlEncodedContent(RequestBody)).Result;
                return APIResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used for calling API GET method 
        /// Returns the response of the API Method
        /// </summary>
        /// <param name="api_url"></param>
        /// Holds the url of the api method
        /// <param name="ParameterList"></param>
        /// Holds the list of parameter data 
        /// <returns></returns>
        public static HttpResponseMessage CallGetAPIResource(string api_url, string ParameterList)
        {
            try 
            {
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var APIResponse = client.GetAsync(Baseurl + api_url + ParameterList).Result;
                return APIResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Returns the list of station name
        /// </summary>
        /// <param name="dropdown_item"></param>
        /// Holds the json data of station table
        /// <returns></returns>
        public static List<SelectListItem> dropdown_station_item(dynamic dropdown_item) 
        {
            List<SelectListItem> dropdown_station = new List<SelectListItem>();
            foreach (var element in dropdown_item)
            {
                dropdown_station.Add(new SelectListItem
                {
                    Text = element["station_id"].ToString(),
                    Value = element["station_short_name"].ToString()
                });
            }

            return dropdown_station;
        }
    }
}