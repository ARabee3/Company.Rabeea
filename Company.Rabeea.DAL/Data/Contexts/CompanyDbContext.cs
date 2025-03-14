using Company.Rabeea.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Company.Rabeea.DAL.Data.Contexts
{
    class CompanyDbContext : DbContext
    {
        public CompanyDbContext() : base()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = Rabee3; Database = CompanyMVCProject; TrustServerCertificate = True; Trusted_Connection = True");
        }
        public DbSet<Department> Departments { get; set; }
    }
}
