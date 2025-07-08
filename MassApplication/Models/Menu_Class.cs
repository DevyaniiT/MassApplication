using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class Menu_Class
    {
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public string menu_url { get; set; }
        public string menu_icon { get; set; }
        public string menu_route { get; set; }
        public int menu_parent_id { get; set; }
        public string menu_group { get; set; }
        public string menu_allow_read { get; set; }
        public string menu_allow_create { get; set; }
        public string menu_allow_update { get; set; }
        public string menu_allow_delete { get; set; }

        /// <summary>
        /// Returns list of menu_mst table data
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <returns></returns>
        public List<Menu_Class> getAllItems(string schema)
        {
            try
            {
                List<Menu_Class> mydata = new List<Menu_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_AllMenuItems(schema);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Menu_Class menuclass = new Menu_Class();
                    menuclass.menu_id = (int)rdr["menu_id"];
                    menuclass.menu_name = rdr["menu_name"].ToString();
                    //menuclass.menu_url = rdr["menu_url"].ToString();
                    //menuclass.menu_icon = rdr["menu_icon"].ToString();
                    //menuclass.menu_route = rdr["menu_route"].ToString();
                    //menuclass.menu_parent_id = (int)rdr["menu_parent_id"];
                    //menuclass.menu_group = rdr["group"].ToString();

                    mydata.Add(menuclass);
                }

                rdr.Dispose();
                con.Close();
                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns list of menu_mst table data according to the role id
        /// </summary>
        /// <param name="schema"></param>
        /// Holds the schema of database
        /// <param name="role_id"></param>
        /// Holds the id of role
        /// <returns></returns>
        public List<Menu_Class> getItems(string schema, string role_id)
        {
            try
            {
                List<Menu_Class> mydata = new List<Menu_Class>();
                string constr = ClassFolder.Database_Class.GetConnectionString();
                NpgsqlConnection con = new NpgsqlConnection(constr);

                con.Open();
                string sql = ClassFolder.Query_Class.sql_RoleAccessQry(schema, role_id);
                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Menu_Class menuclass = new Menu_Class();
                    menuclass.menu_id = (int)rdr["menu_id"];
                    menuclass.menu_name = rdr["menu_name"].ToString();
                    menuclass.menu_url = rdr["menu_url"].ToString();
                    menuclass.menu_icon = rdr["menu_icon"].ToString();
                    menuclass.menu_route = rdr["menu_route"].ToString();
                    menuclass.menu_parent_id = (int)rdr["menu_parent_id"];
                    menuclass.menu_group = rdr["group"].ToString();
                    menuclass.menu_allow_read = rdr["allow_read"].ToString().ToLower();
                    menuclass.menu_allow_create = rdr["allow_create"].ToString().ToLower();
                    menuclass.menu_allow_update = rdr["allow_update"].ToString().ToLower();
                    menuclass.menu_allow_delete = rdr["allow_delete"].ToString().ToLower();

                    mydata.Add(menuclass);
                }
                
                rdr.Dispose();
                con.Close();

                //MASSODBC.MASSODBC obj = new MASSODBC.MASSODBC();
                //obj.DatabaseName = ClassFolder.CrystalConnection_Class.database;
                //obj.OdbcName = ClassFolder.CrystalConnection_Class.odbc_name;
                //obj.UserId = ClassFolder.CrystalConnection_Class.user;
                //obj.Password = ClassFolder.CrystalConnection_Class.password;
                //DataTable dt = new DataTable();
                //dt = obj.OpenTable(sql, "query");

                //foreach (DataRow dr in dt.Rows)
                //{
                //    Menu_Class menuClass = new Menu_Class();
                //    menuClass.menu_id = (int)dr["menu_id"];
                //    menuClass.menu_name = dr["menu_name"].ToString();
                //    menuClass.menu_url = dr["menu_url"].ToString();
                //    menuClass.menu_icon = dr["menu_icon"].ToString();
                //    menuClass.menu_route = dr["menu_route"].ToString();
                //    menuClass.menu_parent_id = (int)dr["menu_parent_id"];

                //    mydata.Add(menuClass);
                //}
                //dt.Dispose();

                return mydata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}