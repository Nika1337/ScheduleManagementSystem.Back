
using Domain.Models;
using Infrastructure.Data.Domain.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Domain;

internal class DomainContext(DbContextOptions<DomainContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<PendingScheduleChange> PendingScheduleChanges { get; set; }
    public DbSet<Worker> Workers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new JobConfiguration());
        builder.ApplyConfiguration(new ScheduleConfiguration());
        builder.ApplyConfiguration(new PendingScheduleChangeConfiguration());
        builder.ApplyConfiguration(new WorkerConfiguration());
    }
}
