using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class IngredientCategoryConfiguration : IEntityTypeConfiguration<IngredientCategory>
{
    void IEntityTypeConfiguration<IngredientCategory>.Configure(EntityTypeBuilder<IngredientCategory> builder)
    {
        _ = builder.Property(ic => ic.Name)
            .HasMaxLength(100);
    }
}
