using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MassApplication.Models;
using Npgsql;

namespace MassApplication.Controllers
{
    public class LoginController : BaseController
    {
        /// <summary>
        /// This method is called when the user login in application
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This method is used for verify the user name or password through web API
        /// If user name or password is valid then the user redirect to Home page
        /// Otherwise Error message is shown
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetToken(LoginClass login)
        {
            try 
            {
                List<KeyValuePair<string, string>> HeaderData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Basic", "bXktdHJ1c3RlZC1jbGllbnQ6c2VjcmV0")
                };

                Dictionary<string, string> RequestData = new Dictionary<string, string>();
                RequestData.Add("username", login.username);
                RequestData.Add("password", login.password);
                RequestData.Add("grant_type", "password");

                var response = CallPostAPIResource("oauth/token", HeaderData, RequestData);

                if (response.IsSuccessStatusCode)
                {
                    var JsonContent = response.Content.ReadAsStringAsync().Result;
                    var tokenContent = getJSONdata(JsonContent);
                    ClassFolder.Login_Class.access_token = tokenContent["access_token"];
                    ClassFolder.Login_Class.email_id = tokenContent["fname"];
                    ClassFolder.Login_Class.role_id = tokenContent["role_id"].ToString();
                    ClassFolder.Login_Class.user_id = tokenContent["id"].ToString();
                    ClassFolder.Login_Class.dashboard_Id = tokenContent["dashboard"];
                    ClassFolder.Login_Class.login_status = tokenContent["error_description"];
                    //ClassFolder.Login_Class.dashboard_Id = ClassFolder.GetValue.GetDashboardByUserId(int.Parse(ClassFolder.Login_Class.user_id));


                    return RedirectToAction("HomePage", "Base");
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
        /// This method is used for verify the user name or password through the sql query
        /// If user name or password is valid then the user redirect to Home page
        /// Otherwise Error message is shown 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Verify(LoginClass login)
        {
            try
            {
                if (schemapath == null)
                {
                    schemapath = "public.";
                }
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);
                con.Open();
                string login_sql = "select * from " + schemapath + "user_login ul where ul.email_id = '" + login.username + "' and password='" + login.password + "'";
                NpgsqlCommand cmd = new NpgsqlCommand(login_sql, con);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ClassFolder.Login_Class.region_id = dr["region"].ToString();
                    ClassFolder.Login_Class.email_id = dr["email_id"].ToString();
                    ClassFolder.Login_Class.pwd = dr["password"].ToString();
                    ClassFolder.Login_Class.role_id = dr["role_id"].ToString();
                    ClassFolder.Login_Class.user_id = dr["id"].ToString();
                    ClassFolder.Login_Class.dashboard_Id = dr["dashboard"].ToString();
                    ClassFolder.Login_Class.login_status = "Login Successfull";
                    dr.Close();
                    cmd.Dispose();
                    con.Close();
                    return RedirectToAction("HomePage", "Base");
                }
                else
                {
                    con.Close();
                    return Content("<script language='javascript' type='text/javascript'>alert('UserId & Password Is not correct Try again..!!'); window.location.href = 'Index'; </script>");
                }

            }

            catch
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something went wrong!!!'); window.location.href = 'Index'; </script>");
            }

        }
    }
}