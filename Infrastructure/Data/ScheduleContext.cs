
using Domain.Models;
using Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

internal class ScheduleContext(DbContextOptions<ScheduleContext> options) : DbContext(options)
{
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<PendingScheduleChange> PendingScheduleChanges { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new JobConfiguration());
        builder.ApplyConfiguration(new ScheduleConfiguration());
        builder.ApplyConfiguration(new PendingScheduleChangeConfiguration());
    }
}
