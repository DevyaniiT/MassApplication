using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class DateClass
    {
        public static string resultyear, startCalDate, endCalDate;

        /// <summary>
        /// For calculating the monthly start date
        /// </summary>
        /// <param name="month"></param>
        /// Defines the month name
        /// <param name="year"></param>
        ///  Defines the year
        /// <returns></returns>
        public static string MonthlyStartDate(string month, string year)
        {
            try 
            {
                resultyear = year;
                string Month = Convert.ToString(Convert.ToInt32(month) + 01);
                startCalDate = resultyear + "-" + month + "-" + "01";

                return startCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the monthly end date
        /// </summary>
        /// <param name="startcaldate"></param>
        ///  Defines the start date
        /// <returns></returns>
        public static string MonthlyEndDate(string startcaldate)
        {
            try
            {
                DateTime startDate;
                startDate = Convert.ToDateTime(startcaldate);
                DateTime endDate = startDate.AddMonths(1);
                endCalDate = endDate.ToString("yyyy-MM-dd");

                DateTime lastOfThisMonth;
                lastOfThisMonth = endDate.AddDays(-1);
                endCalDate = lastOfThisMonth.ToString("yyyy-MM-dd");
                return endCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Quaterly start date
        /// </summary>
        /// <param name="duration"></param>
        /// Defines the duration
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string QuaterlyStartDate(string duration, string year)
        {
            try 
            {
                string part1;
                if (duration.ToLower() == "q1")
                {
                    part1 = "04-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q2")
                {
                    part1 = "07-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q3")
                {
                    part1 = "10-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q4")
                {
                    part1 = "01-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }
                return startCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Quaterly end date
        /// </summary>
        /// <param name="duration"></param>
        /// Defines the duration
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string QuaterlyEndDate(string duration, string year)
        {
            try 
            {
                string part1;
                if (duration.ToLower() == "q1")
                {
                    part1 = "06-30";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q2")
                {
                    part1 = "09-30";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q3")
                {
                    part1 = "12-31";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }
                if (duration.ToLower() == "q4")
                {
                    part1 = "03-31";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }
                return endCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Yearly start date
        /// </summary>
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string YearlyStartDate(string year)
        {
            try 
            {
                string part1;
                part1 = "01-01";
                resultyear = year;
                startCalDate = resultyear + "-" + part1;

                return startCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Yearly end date
        /// </summary>
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string YearlyEndDate(string year)
        {
            try
            {
                string part1;
                part1 = "12-31";
                resultyear = year;
                endCalDate = resultyear + "-" + part1;

                return endCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Half Yearly start date
        /// </summary>
        /// <param name="month"></param>
        /// Defines the month name
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string HalfYearlyStartDate(string month, string year)
        {
            try 
            {
                string part1;
                if (month.ToLower() == "hf1")
                {
                    part1 = "04-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }
                else
                {
                    part1 = "10-01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + part1;
                }

                return startCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// For calculating the Half Yearly start date
        /// </summary>
        /// <param name="month"></param>
        /// Defines the month name
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string HalfYearlyEndDate(string month, string year)
        {
            try 
            {
                string part1;
                if (month.ToLower() == "hf1")
                {
                    part1 = "09-30";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }
                else
                {
                    part1 = "03-31";
                    resultyear = year;
                    endCalDate = resultyear + "-" + part1;
                }

                return endCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      
        /// <summary>
        /// For calculating the Fortnightly start date
        /// </summary>
        /// <param name="range"></param>
        /// Defines the range
        /// <param name="month"></param>
        /// Defines the month
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string FortnightlyStartDate(string range, string month, string year)
        {
            try 
            {
                string part1;
                if (range.ToLower() == "f1")
                {
                    part1 = "01";
                    resultyear = year;
                    startCalDate = resultyear + "-" + month + "-" + part1;
                }
                else
                {
                    part1 = "16";
                    resultyear = year;
                    startCalDate = resultyear + "-" + month + "-" + part1;
                }

                return startCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        /// <summary>
        /// For calculating the Fortnightly end date
        /// </summary>
        /// <param name="range"></param>
        /// Defines the range
        /// <param name="month"></param>
        /// Defines the month
        /// <param name="year"></param>
        /// Defines the year
        /// <returns></returns>
        public static string FortnightlyEndDate(string range, string month, string year)
        {
            try 
            {
                string part1;
                if (range.ToLower() == "f1")
                {
                    part1 = "15";
                    resultyear = year;
                    endCalDate = resultyear + "-" + month + "-" + part1;
                }
                else
                {
                    part1 = "16";
                    resultyear = year;
                    startCalDate = resultyear + "-" + month + "-" + part1;

                    DateTime strtDate = Convert.ToDateTime(startCalDate);
                    DateTime lastDay = new DateTime(strtDate.Year, strtDate.Month, 1).AddMonths(1).AddDays(-1);
                    endCalDate = lastDay.ToString("yyyy-MM-dd");
                }

                return endCalDate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  For calculating the Flow Meter start date
        /// </summary>
        /// <param name="duration"></param>
        /// Defines the duration
        /// <param name="data_date"></param>
        /// Defines the data date
        /// <returns></returns>
        public static string FlowMeterStartDate(string duration, string data_date)
        {
            if (duration.ToLower() == "halfyearly")
            {
                DateTime startDate;
                startDate = Convert.ToDateTime(data_date);
                DateTime endDate = startDate.AddMonths(-6);
                startCalDate = endDate.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime startDate;
                startDate = Convert.ToDateTime(data_date);
                DateTime endDate = startDate.AddYears(-1);
                startCalDate = endDate.ToString("yyyy-MM-dd");
            }


            return startCalDate;
        }

        /// <summary>
        ///  For calculating the Flow Meter end date
        /// </summary>
        /// <param name="duration"></param>
        /// Defines the duration
        /// <param name="data_date"></param>
        /// Defines the data date
        /// <returns></returns>
        public static string FlowMeterEndDate(string duration, string data_date)
        {
            if (duration.ToLower() == "halfyearly")
            {
                DateTime startDate;
                startDate = Convert.ToDateTime(data_date);
                endCalDate = startDate.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime startDate;
                startDate = Convert.ToDateTime(data_date);
                endCalDate = startDate.ToString("yyyy-MM-dd");
            }
            return endCalDate;
        }
    }

}