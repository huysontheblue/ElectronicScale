using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ElectronicScale
{
    internal class ConfigSettings
    {
    }

    internal class mssql
    {
        public string? host { get; set; } = "10.2.100.213";
        public int? port { get; set; } = 1433;
        public string? database { get; set; } = "PMPMES";
        public string? user { get; set; } = "sa";
        public string? password { get; set; } = "PMPvn@0808";
    }

    internal class mysql
    {
        public string? host { get; set; } = "127.0.0.1";
        public int? port { get; set; } = 3306;
        public string? database { get; set; } = "test";
        public string? user { get; set; } = "root";
        public string? password { get; set; } = "root";
    }
    internal class scale
    {
        public string? port { get; set; } = "COM3";
        public int? baudrate { get; set; } = 9600;
        public decimal? compensation { get; set; } = 0.169M;
    }
    internal class alarm
    {
        public string? port { get; set; } = "COM5";
        public int? baudrate { get; set; } = 38400;
        public bool? voicestatus { get; set; } = true;
        public int? alarmupper { get; set; } = 0;
        public int? alarmlower { get; set; } = 0;
    } 
    internal class fileinfo
    {
        public string? path { get; set; } = "";
        public string? file { get; set; } = "xlsx";
    }
    internal class printer
    {
        //public string? printer { get; set; } = "";
        public string? ip { get; set; } 
        public int? quality { get; set; } 
    }
    //internal class system
    //{
    //    public string? version { get; set; } = "0.1";
    //    public string? updateurl { get; set; } = "http://192.168.161.202:8011/checkupdate/getVersion";
    //}

    internal class Configuration
    {
        public mssql mssql { get; set; }
        public mysql MysqlSettings { get; set; }
        public scale ScaleSettings { get; set; }
        public alarm AlarmSettings { get; set; }
        public fileinfo FileinfoSettings { get; set; }
        public printer PrinterSettings { get; set; }
        //public system SystemSettings { get; set; }


        public Configuration()
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            string configPath = Path.Join(Environment.CurrentDirectory,"config.json");
            configBuilder.AddJsonFile(configPath, true, true);
            IConfiguration configuration = configBuilder.Build();
            configuration.Bind(this);
        }
    }


}
