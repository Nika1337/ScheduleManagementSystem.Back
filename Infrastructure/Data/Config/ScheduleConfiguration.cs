

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder
            .HasKey(sch => sch.Id);

        builder
            .HasOne(sch => sch.JobToPerform)
            .WithMany();

        builder
            .HasOne(sch => sch.PendingScheduleChange)
            .WithOne(sch => sch.ScheduleToChange);

        builder
            .HasIndex(sch => sch.ScheduleOfWorkerId);

        builder
            .HasIndex(sch => sch.ScheduledAtDate);

        builder
            .HasIndex(sch => new { sch.ScheduleOfWorkerId, sch.ScheduledAtDate });
    }
}
