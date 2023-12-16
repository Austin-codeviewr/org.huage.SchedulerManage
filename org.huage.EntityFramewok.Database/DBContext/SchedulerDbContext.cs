using System.Reflection;
using Microsoft.EntityFrameworkCore;
using org.huage.EntityFramewok.Database.Table;

namespace org.huage.EntityFramewok.Database.DBContext;

public class SchedulerDbContext : DbContext
{
    public DbSet<Scheduler> Scheduler { get; set; }

    public SchedulerDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Scheduler>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}