
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class ScheduleChangeRequestConfiguration : IEntityTypeConfiguration<ScheduleChangeRequest>
{
    public void Configure(EntityTypeBuilder<ScheduleChangeRequest> builder)
    {
        builder
            .HasKey(sch => sch.Id);

        builder
            .HasIndex(sch => sch.RequestDateTime);
    }
}
