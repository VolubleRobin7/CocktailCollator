using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
    void IEntityTypeConfiguration<RecipeStep>.Configure(EntityTypeBuilder<RecipeStep> builder)
    {
        _ = builder
            .HasOne(rs => rs.Recipe)
            .WithMany(r => r.Steps)
            .HasForeignKey(rs => rs.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
