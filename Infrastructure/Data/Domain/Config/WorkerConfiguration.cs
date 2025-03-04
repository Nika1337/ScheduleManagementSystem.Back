

using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Domain.Config;
internal class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder
            .HasKey(w => w.Id);

        builder
            .Property(w => w.FirstName)
            .HasMaxLength(250);

        builder
            .Property(w => w.LastName)
            .HasMaxLength(250);
    }
}
