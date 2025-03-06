using Microsoft.EntityFrameworkCore;
using Accurate_ERP.Models;

namespace Accurate_ERP.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
{
	base.OnModelCreating(builder);
	
	builder.Entity<Employee>().ToTable("Employee");

	         
}
        public DbSet<Employee> Employees { get; set; }
    }
}
