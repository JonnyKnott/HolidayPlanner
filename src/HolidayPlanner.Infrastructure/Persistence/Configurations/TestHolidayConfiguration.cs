using HolidayPlanner.Domain.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HolidayPlanner.Infrastructure.Persistence.Configurations;

internal sealed class TestHolidayConfiguration : IEntityTypeConfiguration<TestHoliday>
{
    public void Configure(EntityTypeBuilder<TestHoliday> builder)
    {
        builder.ToTable("TestHolidays");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Destination)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.ModifiedOn)
            .IsRequired();
    }
}
