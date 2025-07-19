using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicScale
{
    internal class PackingInfoData
    {
        public struct PackingInfoDataList
        {
            public int id { get; set; }
            public string apn { get; set; }
            public string project { get; set; }
            public string color { get; set; }
            public string spec { get; set; }
            public string lag { get; set; }
            public int num { get; set; }
            public string upper { get; set; }
            public string lower { get; set; }
            public string standard { get; set; }
            public string customer { get; set; }
            public string mes_code { get; set; }
            public string nw { get; set; }
            public string gw { get; set; }
            public string eeee { get; set; }
            public string tag_product_name { get; set; }
            public int range { get; set; }
        }
    }
}
