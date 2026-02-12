using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class MeasurementConfiguration : IEntityTypeConfiguration<Measurement>
{
    void IEntityTypeConfiguration<Measurement>.Configure(EntityTypeBuilder<Measurement> builder)
    {
        _ = builder.Property(m => m.Name)
            .HasMaxLength(50);
    }
}
