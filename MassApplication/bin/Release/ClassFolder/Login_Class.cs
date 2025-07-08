using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class Login_Class
    {
        public static string region, email, password, role, dbId, get_token, loginState, userid;
        public static string active_db = ConfigurationManager.AppSettings["ActiveDB"];
        public static string access_token
        {
            get { return get_token; }
            set { get_token = value; }
        }
        public static string email_id
        {
            get { return email; }
            set { email = value; }
        }
        public static string pwd
        {
            get { return password; }
            set { password = value; }
        }
        public static string role_id
        {
            get { return role; }
            set { role = value; }
        }
        public static string dashboard_Id
        {
            get { return dbId; }
            set { dbId = value; }
        }
        public static string region_id
        {
            get { return region; }
            set { region = value; }
        }
        public static string active_dashboard
        {
            get { return active_db; }
            set { active_db = value; }
        }
        public static string login_status
        {
            get { return loginState; }
            set { loginState = value; }
        }

        public static string user_id
        {
            get { return userid; }
            set { userid = value; }
        }

    }
}