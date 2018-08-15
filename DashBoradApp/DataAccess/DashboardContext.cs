using DashBoradApp.DataAccess.Configurations;
using DashBoradApp.DataAccess.Entitites;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DashBoradApp.DataAccess
{
    public class DashboardContext : DbContext
    {
        public DashboardContext() : base("DashboardOrder")
        {

        }

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailsConfiguration());
        }


    }
}