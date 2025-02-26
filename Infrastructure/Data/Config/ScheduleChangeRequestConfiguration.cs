
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class PendingScheduleChangeConfiguration : IEntityTypeConfiguration<PendingScheduleChange>
{
    public void Configure(EntityTypeBuilder<PendingScheduleChange> builder)
    {
        builder
            .HasKey(sch => sch.Id);

        builder
            .HasIndex(sch => sch.RequestDateTime);
    }
}
