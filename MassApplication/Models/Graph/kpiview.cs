using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models.Graph
{
    public class kpiview
    {
        public int kpi_id { get; set; }
        public int chart_type_id { get; set; }
        public string kpi_name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public bool is_deleted { get; set; }
        public string query_script { get; set; }
        public string chart_type { get; set; }
        public List<kpiview> getAllkpiview(string schema)
        {
            List<kpiview> mydata = new List<kpiview>();
            //string constr = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString;
            string constr = ConfigurationManager.ConnectionStrings["DatabaseServer"].ConnectionString;
            NpgsqlConnection con = new NpgsqlConnection(constr);
            string sql = ClassFolder.Query_Class.sql_kpiview_Qry(schema);
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                kpiview sc = new kpiview();
                sc.kpi_id = Convert.ToInt32(rdr["kpi_id"]);
                sc.chart_type_id = Convert.ToInt32(rdr["chart_type_id"]);
                sc.kpi_name = rdr["kpi_name"].ToString();
                sc.description= rdr["description"].ToString();
                sc.status = rdr["status"].ToString();
                sc.is_deleted= Convert.ToBoolean(rdr["is_deleted"]);
                sc.chart_type = rdr["chart_type"].ToString();

                mydata.Add(sc);
            }
            rdr.Dispose();
            con.Close();
            return mydata;
        }
        public kpiview getkpiData(string schema, int id)
        {
            kpiview mydata = new kpiview();
            //List<Parameter_Class> myparamdata = new List<Parameter_Class>();
            //List<Query_Class> myquerydata = new List<Query_Class>();
            //List<Connection_Class> myconndata = new List<Connection_Class>();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString;
            NpgsqlConnection con = new NpgsqlConnection(constr);
            NpgsqlCommand cmd;
            NpgsqlDataAdapter sda;
            con.Open();

            string sql_param = ClassFolder.Query_Class.sql_editParamReportQuery(schema, id);
            string sql_query = ClassFolder.Query_Class.sql_editQryKPIQuery(schema, id);

            //cmd = new NpgsqlCommand(sql_query, con);
            //sda = new NpgsqlDataAdapter(cmd);
            //DataSet ds_param = new DataSet();
            //sda.Fill(ds_param);
            //sda.Dispose();

            cmd = new NpgsqlCommand(sql_query, con);
            sda = new NpgsqlDataAdapter(cmd);
            DataTable dt_qry = new DataTable();
            sda.Fill(dt_qry);
            //Connection_Class conclass = new Connection_Class();
            foreach (DataRow rdr in dt_qry.Rows)
            {
                //mydata.kpi_id = Convert.ToInt32(rdr["kpi_id"]);
                mydata.chart_type_id = Convert.ToInt32(rdr["chart_type_id"]);
                mydata.chart_type = rdr["chart_type"].ToString();
                mydata.kpi_name = rdr["kpi_name"].ToString();
                mydata.description = rdr["description"].ToString();
                mydata.query_script = rdr["query_script"].ToString();
                //mydata.status = rdr["status"].ToString();
                //mydata.query_script = rdr["query_script"].ToString(); ;

            }
            sda.Dispose();
            return mydata;
        }
    }
}