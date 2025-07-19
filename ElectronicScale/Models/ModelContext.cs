using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicScale.Models
{
    internal class ModelContext : DbContext 
    {
       private static mssql mssql = new Configuration().mssql;
        private  string ConnectionStrings { get; set; } = $"Server={mssql.host};Database={mssql.database};User Id={mssql.user};Password={mssql.password};TrustServerCertificate=True;";
        public DbSet<PackingInfo>? PackingInfo { get; set; }
        public DbSet<PackingScale>? PackingScale { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStrings);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackingInfo>(entity =>
            {
                entity.Property(e => e.upper)
                .HasConversion(typeof(string));
                entity.Property(e => e.lower)
                .HasConversion(typeof(string));
                entity.Property(e => e.nw)
                .HasConversion(typeof(string));
                entity.Property(e => e.gw)
                .HasConversion(typeof(string));
                entity.Property(e => e.peer_nw)
                .HasConversion(typeof(string));
                entity.Property(e => e.peer_interval)
                .HasConversion(typeof(string));
                entity.Property(e => e.packing_material_nw)
                .HasConversion(typeof(string));
                entity.Property(e => e.tray_nw)
                .HasConversion(typeof(string));
                entity.Property(e => e.standard)
                .HasConversion(typeof(string));
            });
        }
    }
    [Table("packing_info")]
    public class PackingInfo
    {
        public int id { get; set; } //ID
        public string? apn { get; set; } //业务APN
        public string? project { get; set; } //专案
        public string? color { get; set; } //品名/颜色
        public string? spec { get; set; } //规格
        public string? lag { get; set; } //外购件
        public int? num { get; set; } //标准件装量
        public decimal? upper { get; set; }  //秤重上限
        public decimal? lower { get; set; } //秤重下限
        public decimal? standard { get; set; } //秤重标准
        public string? customer { get; set; } //出货客户
        public string? mes_code { get; set; } //MES料号代码
        public decimal? nw { get; set; } //净重(标签显示)
        public decimal? gw { get; set; } //毛重(标签显示)
        public string? eeee { get; set; } //工程代码
        public string? tag_product_name { get; set; }  //标签上显示物料名称
        public string? range { get; set; } //备注
        public decimal? peer_nw { get; set; } //单片净重
        public decimal? peer_interval { get; set; } //单片正负误差
        public decimal? packing_material_nw { get; set; } //包装材料重量
        public decimal? tray_nw { get; set; } //Tray盘重量
        public int? tray_ex_num { get; set; } //附加Tray盘数量
        public int? tray_include_num { get; set; } //单Tray盘容量
    }


    [Table("packing_scale")]
    public class PackingScale
    {
        public int id { get; set; }
        public string? apn { get; set; }
        public string? code { get; set; }
        public string? sn { get; set; }
        public double? weight { get; set; }
        public DateTime? create_time { get; set; }
    }
}
