using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    void IEntityTypeConfiguration<Ingredient>.Configure(EntityTypeBuilder<Ingredient> builder)
    {
        _ = builder.Property(i => i.Name)
            .HasMaxLength(100);

        _ = builder.HasOne(i => i.Category)
            .WithMany(c => c.Ingredients)
            .HasForeignKey(i => i.IngredientCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
