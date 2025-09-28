using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    void IEntityTypeConfiguration<RecipeIngredient>.Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        _ = builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        _ = builder.HasOne(ri => ri.Recipe)
               .WithMany(r => r.Ingredients)
               .HasForeignKey(ri => ri.RecipeId);

        _ = builder.HasOne(ri => ri.Ingredient)
               .WithMany(i => i.Recipes)
               .HasForeignKey(ri => ri.IngredientId);
    }
}
