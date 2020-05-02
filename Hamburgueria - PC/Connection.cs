using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;

namespace Hamburgueria
{
    internal class Connection : DbContext
    {
        public DbSet<Tables.Client> Clients { get; set; }
        public DbSet<Tables.Product> Products { get; set; }
        public DbSet<Tables.ProductSale> ProductSales { get; set; }
        public DbSet<Tables.Sale> Sales { get; set; }
        public DbSet<Tables.SaleDelivery> SaleDeliveries { get; set; }
        public DbSet<Tables.SaleTable> SaleTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
