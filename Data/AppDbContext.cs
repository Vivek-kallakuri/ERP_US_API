using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timesheet>()
                .Property(t => t.Status)
                .HasDefaultValue("Pending");
        }

    }
}

