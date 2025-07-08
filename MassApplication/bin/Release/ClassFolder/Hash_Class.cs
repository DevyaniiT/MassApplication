using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class Hash_Class
    {
        public static string query, Correctval, function_script;

        /// <summary>
        /// Used for replace VAR characters in query to its values stored in Hashtable data
        /// </summary>
        /// <param name="query"></param>
        /// Defines the sql query
        /// <param name="updateddata"></param>
        /// Defines the Hashtable data
        /// <returns></returns>
        public static string ReplaceHashdata(string query, Hashtable updateddata)
        {
            try
            {
                if (query != null)
                {
                    String function_str = (query);

                    Regex regex = new Regex(@"\[var([a-zA-Z0-9_\.\-]+)\]");
                    MatchCollection matchCollection = regex.Matches(query);

                    foreach (Match match in matchCollection)
                    {
                        String longid = match.Value.ToString().Substring(4, match.Value.ToString().ToLower().Length - 5);
                        string input = updateddata[longid].ToString();

                        String sPattern = match.Value.Replace("[", "\\[");
                        sPattern = sPattern.Replace("]", "\\]");

                        Correctval = input;
                        function_str = Regex.Replace(function_str, sPattern, Correctval);

                        function_script = function_str;
                    }
                    function_script = function_str;

                }
            }

            catch
            {
            }

            return function_script;
        }
    }
}