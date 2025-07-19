using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicScale
{
    internal class EFDBContent:DbContext
    {
        private string Connection { get; set; } = "D";
        public EFDBContent(string connection)
        {

        }
        public DbSet<PackingInfoData> PackingInfoData { get; set; }
    }

    public class PackingInfo
    {
        public int id { get; set; }
        public string apn { get; set; }
        public string project { get; set; }
        public string color { get; set; } 
        public string spec { get; set; }
        public string lag { get; set; }
        public string num { get; set; }
        public string upper { get; set; }
        public string lower { get; set; }
        public string standard { get; set; }
        public string customer { get; set; }
        public string mes_code { get; set; }
        public string nw { get; set; }
        public string gw { get; set; }
        public string eeee { get; set; }
        public string tag_product_name { get; set; }
        public string range { get; set; }
        public string peer_nw { get; set; }
        public string peer_interval { get; set; }
        public string packing_material_nw { get; set; }
        public string tray_nw { get; set; }
        public string tray_ex_num { get; set; }
        public string tray_include_num { get; set; }
    }

}
