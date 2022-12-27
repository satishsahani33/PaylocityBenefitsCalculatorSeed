using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataContext
{
    public class EmployeeDbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Dependent> Dependents { get; set; }
        public DbSet<PayCheck> PayChecks { get; set; }
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {

        }

        public EmployeeDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=benefitpaycheckdb;Trusted_Connection=True");
        }
    }
}
