using PUCCI.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PUCCI.Data
{
    public class PucciContext : DbContext
    {
        public PucciContext() : base ("PucciContext") 
        { 
        }

        public DbSet<Container> Containers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        }
    }
}
