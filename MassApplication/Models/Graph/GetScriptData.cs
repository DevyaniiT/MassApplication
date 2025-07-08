using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MassApplication.Models.Graph
{
    public class GetScriptData
    {
        static string DBconnection = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
        static string[] Connectionarray = System.Text.RegularExpressions.Regex.Split(DBconnection, ";");
        static string DBscema = Connectionarray[5].ToString();
        static string[] scemaname = System.Text.RegularExpressions.Regex.Split(DBscema, "=");
        static Hashtable hashmapscript = new Hashtable();
        static Hashtable hashchartdetails = new Hashtable();
        static string[] getalldetails;
        static string[] getchartdetails;
        public static DataTable ConvertinJsonDatatblODBC(int getquery_Id)
        {
            string[] scriptfileData;
            DataTable dtt = new DataTable();
            scriptfileData = (string[])hashmapscript[getquery_Id];
            try
            {
                MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                obj.DatabaseName = scriptfileData[5];
                obj.OdbcName = scriptfileData[8];
                obj.UserId = scriptfileData[6];
                obj.Password = scriptfileData[7];

                string result = MASSODBC.MASSODBC.GetConnection(obj.OdbcName, obj.DatabaseName, obj.UserId, obj.Password);
                if (result == "true")
                {
                    dtt = obj.OpenTable(scriptfileData[1], "function");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dtt;
        }

        public static DataTable ConvertinJsonDatatbl(string getQueriesDatas)
        {
            NpgsqlConnection connection = new NpgsqlConnection(DBconnection);
            NpgsqlCommand command = new NpgsqlCommand(getQueriesDatas, connection);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            DataSet ds = new DataSet();
            connection.Open();
            command.ExecuteNonQuery();
            da.Fill(ds);
            connection.Close();
            DataTable dtt = ds.Tables[0];
            return dtt;
        }
        public static string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
        public static List<ChartParameters> GetParameters()
        {
            try
            {
                List<ChartParameters> listchatparams = new List<ChartParameters>();
                NpgsqlConnection connection = new NpgsqlConnection(DBconnection);
                string chart_sql = "SELECT chart_type_id,chart_type,chart_parameters,chart_bounds,chart_image,description,icon_path " 
                    + "FROM " + scemaname[1] + ".dashboard_chart_mst where is_deleted = 0";

                NpgsqlCommand command = new NpgsqlCommand(chart_sql, connection);
                connection.Open();
                NpgsqlDataReader rd = command.ExecuteReader();
                while (rd.Read())
                {
                    ChartParameters gd = new ChartParameters();
                    gd.chart_type_id = Convert.ToInt32(rd["chart_type_id"].ToString());
                    gd.charttype = rd["chart_type"].ToString();
                    gd.ChartParams = rd["chart_parameters"].ToString();
                    gd.chart_bound = Convert.ToInt32(rd["chart_bounds"].ToString());
                    gd.image_path = rd["chart_image"].ToString();
                    gd.IsChecked = Convert.ToBoolean("false");
                    gd.description = rd["description"].ToString();
                    gd.icon_path = rd["icon_path"].ToString();
                    gd.chart_selected_params = "00";
                    gd.chart_selected_params = "00";
                    listchatparams.Add(gd);
                    getchartdetails = new string[] { Convert.ToInt32(gd.chart_type_id).ToString(), gd.charttype, gd.ChartParams, Convert.ToInt32(gd.chart_bound).ToString() };
                    Addchart_details(gd.charttype, getchartdetails);
                }
                connection.Close();
                return listchatparams;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<GetQueriesData> Getscript()
        {
            try
            {
                int role_id = int.Parse(ClassFolder.Login_Class.role_id);
                List<GetQueriesData> listQryData = new List<GetQueriesData>();
                NpgsqlConnection connection = new NpgsqlConnection(DBconnection);
                ////scemaname[1] = "mass2";
                //string kpi_sql = "select dbconnection_id,query_id,function_name,type,"
                //  + " concat(query_script, ' ', query_where_cond, ' ', query_group_by, ' ', query_order_by) Query_scr,"
                //  + " connection_name, dsn_name, user_id, password, db_schemaname"
                //  + " from " + scemaname[1] + ".dashboard_query_mst dqm"
                //  + " left join " + scemaname[1] + ".dbconnection_mst dbm on dbm.id = dqm.dbconnection_id"
                //  + " where dqm.is_deleted = 0 and dbm.is_deleted = 0";

                string kpi_sql = "SELECT dqm.dbconnection_id, dqm.query_id, dqm.function_name, dqm.type,"
                  + "concat(dqm.query_script, ' ', dqm.query_where_cond, ' ', dqm.query_group_by, ' ', dqm.query_order_by) Query_scr,"
                  + "dbm.connection_name, dbm.dsn_name, dbm.user_id, dbm.password, dbm.db_schemaname "
                  + "from " + scemaname[1] + ".dashboard_query_mst as dqm "
                  + "LEFT JOIN " + scemaname[1] + ".dbconnection_mst as dbm on dbm.id = dqm.dbconnection_id "
                  + "LEFT JOIN " + scemaname[1] + ".query_role_map as qrm on qrm.dashboard_query_id = dqm.query_id "
                  + "WHERE dqm.is_deleted = 0 AND dbm.is_deleted = 0 AND qrm.allow_read = true "
                  + "AND qrm.role_id = "+ role_id + " ORDER BY dqm.query_id DESC";

                NpgsqlCommand command = new NpgsqlCommand(kpi_sql, connection);
                connection.Open();
                NpgsqlDataReader rd = command.ExecuteReader();
                while (rd.Read())
                {
                    GetQueriesData gd = new GetQueriesData();
                    gd.function_name = rd["function_name"].ToString();
                    gd.qry_script = rd["Query_scr"].ToString();
                    gd.dbconnection_id = Convert.ToInt32(rd["dbconnection_id"].ToString());
                    gd.query_id = Convert.ToInt32(rd["query_id"].ToString());
                    int qryid = Convert.ToInt32(rd["query_id"].ToString());
                    gd.type = rd["type"].ToString();
                    gd.connection_name = rd["connection_name"].ToString();
                    gd.schemaname = rd["db_schemaname"].ToString();
                    gd.user_name = rd["user_id"].ToString();
                    gd.password = rd["password"].ToString();
                    gd.dsn_name = rd["dsn_name"].ToString();
                    gd.IsChecked = Convert.ToBoolean("false");
                    listQryData.Add(gd);
                    getalldetails = new string[] { gd.function_name, gd.qry_script, Convert.ToInt32(gd.dbconnection_id).ToString(), Convert.ToInt32(gd.query_id).ToString(), gd.connection_name, gd.schemaname, gd.user_name, gd.password, gd.dsn_name, Convert.ToBoolean(gd.IsChecked).ToString() };
                    Addtoscriptlist(qryid, getalldetails);
                }
                connection.Close();
                return listQryData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static Hashtable Addtoscriptlist(int keys, string[] value)
        {
            if (hashmapscript.Contains(keys))
            {
                hashmapscript[keys] = value;
            }
            else
            {
                hashmapscript.Add(keys, value);
            }
            return hashmapscript;
        }
        public static object GetKPIName(int KPIkey)
        {
            object GetKPI = hashmapscript[KPIkey];
            return GetKPI;
        }
        public static void Addchart_details(string keys, string[] value)
        {
            if (hashchartdetails.Contains(keys))
            {
                hashchartdetails[keys] = value;
            }
            else
            {
                hashchartdetails.Add(keys, value);
            }
        }
        public static object GetChartDetails(string charttypes)
        {
            object GetChartDetailsVal = hashchartdetails[charttypes];
            return GetChartDetailsVal;
        }

    }
}