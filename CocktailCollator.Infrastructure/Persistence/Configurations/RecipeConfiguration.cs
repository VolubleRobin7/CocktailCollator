using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    void IEntityTypeConfiguration<Recipe>.Configure(EntityTypeBuilder<Recipe> builder)
    {
        _ = builder.Property(r => r.Name)
            .HasMaxLength(100);
    }
}
