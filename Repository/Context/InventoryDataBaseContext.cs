using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Repository.Context
{
    public partial class InventoryDataBaseContext : DbContext
    {
        public InventoryDataBaseContext()
        {
        }

        public InventoryDataBaseContext(DbContextOptions<InventoryDataBaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<InventoryModel> InventoryModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=51.104.231.132;Database=bookkeeping;User ID=bookkeepingusr;Password=ImdbGtd3dsr@dsrtd4;Trusted_Connection=false;");

                //get connection string from appsetting.json file and connect the database
                string c = Directory.GetCurrentDirectory();
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
                string connectionString = configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<InventoryModel>(entity =>
            {
                entity.ToTable("InventoryTable");

                entity.Property(e => e.ProductId).HasDefaultValueSql("(int)");

                entity.Property(e => e.Name).HasColumnType("nvarchar(100)");

                entity.Property(e => e.Category).HasColumnType("nvarchar(50)");
                entity.Property(e => e.Color).HasColumnType("nvarchar(10)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.UnitPrice).HasColumnType("int");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Code)
                 .IsRequired()
                 .HasMaxLength(200);
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description).HasColumnType("nvarchar(100)");


            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
