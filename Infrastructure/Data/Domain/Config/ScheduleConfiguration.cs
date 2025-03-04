

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Domain.Config;

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
            .WithOne(sch => sch.ScheduleToChange)
            .HasForeignKey<PendingScheduleChange>("ScheduleId");

        builder
            .HasIndex("ScheduleOfWorkerId");

        builder
            .HasIndex(sch => sch.ScheduledAtDate);

        builder
            .HasIndex("ScheduleOfWorkerId", nameof(Schedule.ScheduledAtDate));
    }
}
