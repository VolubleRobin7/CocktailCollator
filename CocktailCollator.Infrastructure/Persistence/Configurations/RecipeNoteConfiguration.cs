using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class RecipeNoteConfiguration : IEntityTypeConfiguration<RecipeNote>
{
    void IEntityTypeConfiguration<RecipeNote>.Configure(EntityTypeBuilder<RecipeNote> builder)
    {
        _ = builder.HasKey(rn => new { rn.UserId, rn.RecipeId });

        _ = builder
            .HasOne(rn => rn.Recipe)
            .WithMany(r => r.PersonalRecipeNotes)
            .HasForeignKey(rn => rn.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
