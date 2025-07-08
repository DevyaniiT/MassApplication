using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;
using System.Data;

namespace MassApplication.Models
{

    public class DataManagmentFile
    {
        static string DBconnection = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString;

        static string[] Connectionarray = System.Text.RegularExpressions.Regex.Split(DBconnection, ";");
        static string DBscema = Connectionarray[3].ToString();
        static string[] scemaname = System.Text.RegularExpressions.Regex.Split(DBscema, "=");

       static int getkpiID;

      static NpgsqlConnection connection = new NpgsqlConnection(DBconnection);
        public static int InsertGraphData(int Chart_ids,string KPIName,string chart_parametersVal,int Chart_bounds)
        {

          
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO " + scemaname[1] + ".dashboard_kpi_charts(chart_type_id, kpi_name, chart_params, chart_bounds,chart_position, created_by,is_deleted) VALUES (@chart_type_id, @kpi_name, @chart_params, @chart_bounds,@chart_position, @created_by,@is_deleted) RETURNING kpi_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new NpgsqlParameter("@chart_type_id", Convert.ToInt64(Chart_ids)));
            cmd.Parameters.Add(new NpgsqlParameter("@kpi_name", KPIName));
            cmd.Parameters.Add(new NpgsqlParameter("@chart_params", chart_parametersVal));
            cmd.Parameters.Add(new NpgsqlParameter("@chart_bounds", Chart_bounds));
            cmd.Parameters.Add(new NpgsqlParameter("@chart_position", 1));
            cmd.Parameters.Add(new NpgsqlParameter("@created_by", 2));
          //  cmd.Parameters.Add(new NpgsqlParameter("@created_on", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            cmd.Parameters.Add(new NpgsqlParameter("@is_deleted", Convert.ToInt32(0)));
            getkpiID = (int)cmd.ExecuteScalar();
            cmd.Dispose();
        

            return getkpiID;
        }

        public static void InsertGraphMapData(int I_kpi,int I_qryID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO " + scemaname[1] + ".dashboard_query_map(kpi_id,query_id) VALUES (@kpi_id,@query_id)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new NpgsqlParameter("@kpi_id", Convert.ToInt32(I_kpi)));
            cmd.Parameters.Add(new NpgsqlParameter("@query_id", I_qryID));
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}