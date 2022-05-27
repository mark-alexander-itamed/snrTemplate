using BugReporter.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BugReporter.Api
{
    public class BugReporterDbContext : DbContext
    {
        public BugReporterDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugComment> BugComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugReporterDbContext).Assembly);
        }
    }
}