using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class IngredientMeasurementConfiguration : IEntityTypeConfiguration<IngredientMeasurement>
{
    void IEntityTypeConfiguration<IngredientMeasurement>.Configure(EntityTypeBuilder<IngredientMeasurement> builder)
    {
        _ = builder.HasKey(im => new { im.IngredientId, im.MeasurementId });

        _ = builder.HasOne(im => im.Ingredient)
               .WithMany(i => i.Measurements)
               .HasForeignKey(im => im.IngredientId);

        _ = builder.HasOne(im => im.Measurement)
               .WithMany(m => m.Ingredients)
               .HasForeignKey(im => im.MeasurementId);
    }
}
