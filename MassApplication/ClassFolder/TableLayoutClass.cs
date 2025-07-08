using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class TableLayoutClass
    {
        /// <summary>
        /// Used for create columns of temporary table
        /// </summary>
        /// <param name="bAdd2"></param>
        /// Defines true or false situation
        /// True for column second also exists
        /// false for only one column exists
        /// <returns></returns>
        public static DataTable CreateTempTable(bool bAdd2)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Path", typeof(string));
            dt.Columns.Add("Value1", typeof(Double));
            if (bAdd2 == true)
            {
                dt.Columns.Add("Value2", typeof(Double));
            }
            dt.Columns.Add("Max_Value", typeof(Double));
            dt.Columns.Add("Min_Value", typeof(Double));
            return dt;
        }

        /// <summary>
        ///  Used for create temporary table
        /// </summary>
        /// <param name="tabledata"></param>
        /// Defines the table
        /// <returns></returns>
        public static DataTable CreateNewTable(DataTable tabledata)
        {
            string graph_name = "";
            //int max_val = 0;
            //int min_val = 0;
            DataTable dtData = null;
            int count = tabledata.Columns.Count;
            for (int i = 0; i < tabledata.Rows.Count; i++)
            {
                graph_name = tabledata.Rows[i][0].ToString();
                //max_val = Convert.ToInt32(tabledata.Rows[i][count-2]);
                //min_val = Convert.ToInt32(tabledata.Rows[i][count-1]);
                switch (graph_name.ToUpper())
                {
                    case "AUTOMATIC GAIN":
                        dtData = CreateTempTable(true);
                        for (int j = 0; j < tabledata.Columns.Count; j++)
                        {
                            string scolumn = tabledata.Columns[j].ColumnName;

                            if (scolumn != "graph_name" && scolumn != "max_val" && scolumn != "min_val")
                            {
                                string svalue1 = tabledata.Rows[i][j].ToString();
                                string svalue2 = tabledata.Rows[i][j + 1].ToString();
                                string max_val = tabledata.Rows[i]["max_val"].ToString();
                                string min_val = tabledata.Rows[i]["min_val"].ToString();

                                DataRow dr = dtData.NewRow();
                                dr[0] = scolumn;
                                dr[1] = Convert.ToDouble(svalue1);
                                dr[2] = Convert.ToDouble(svalue2);
                                dr[3] = Convert.ToDouble(max_val);
                                dr[4] = Convert.ToDouble(min_val);
                                dtData.Rows.Add(dr);
                                j = j + 1;
                            }

                        }
                        break;
                    default:
                        dtData = CreateTempTable(false);
                        for (int j = 0; j < tabledata.Columns.Count; j++)
                        {
                            string scolumn = tabledata.Columns[j].ColumnName;

                            if (scolumn != "graph_name" && scolumn != "graphname" && scolumn != "max_val" && scolumn != "min_val")
                            {
                                string svalue1 = tabledata.Rows[i][j].ToString();
                                string max_val = tabledata.Rows[i]["max_val"].ToString();
                                string min_val = tabledata.Rows[i]["min_val"].ToString();

                                DataRow dr = dtData.NewRow();
                                dr[0] = scolumn;
                                dr[1] = Convert.ToDouble(svalue1);
                                dr[2] = Convert.ToDouble(max_val);
                                dr[3] = Convert.ToDouble(min_val);
                                dtData.Rows.Add(dr);
                            }
                        }
                        break;
                }
            }
            return dtData;
        }
    }
}