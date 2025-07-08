using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassApplication.Models
{
    public class MenuMap_Class
    {
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public string menu_parent_id { get; set; }
        public bool role_menu_create { get; set; }
        public bool role_menu_read { get; set; }
        public bool role_menu_update { get; set; }
        public bool role_menu_delete { get; set; }
    }
}