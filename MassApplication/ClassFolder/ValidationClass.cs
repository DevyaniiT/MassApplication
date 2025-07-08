using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class ValidationClass
    {
        /// <summary>
        /// Validate the dashboard URL according to role that they have to access
        /// </summary>
        /// <param name="id"></param>
        /// Holds the id exists in url
        /// <returns></returns>
        public static bool ValidateDashboardUrl(int id)
        {
            try
            {
                int role_id = int.Parse(ClassFolder.Login_Class.role_id);
                string urlId = ClassFolder.GetValue.GetRoleWiseDashboardId(role_id);
                List<string> list = new List<string>();
                string[] varVal = urlId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value in varVal)
                {
                    list.Add(value);
                }
                bool result = list.Contains(id.ToString());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Validate the KPI URL according to role that they have to access
        /// </summary>
        /// <param name="id"></param>
        /// Holds the id exists in url
        /// <returns></returns>
        public static bool ValidateKPIUrl(int id)
        {
            try
            {
                int role_id = int.Parse(ClassFolder.Login_Class.role_id);
                string urlId = ClassFolder.GetValue.GetRoleWiseKPIId(role_id);
                List<string> list = new List<string>();
                string[] varVal = urlId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string value in varVal)
                {
                    list.Add(value);
                }
                bool result = list.Contains(id.ToString());
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}