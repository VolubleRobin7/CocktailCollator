using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class RecipeDocumentConfiguration : IEntityTypeConfiguration<RecipeDocument>
{
    void IEntityTypeConfiguration<RecipeDocument>.Configure(EntityTypeBuilder<RecipeDocument> builder)
    {
        _ = builder.HasKey(rd => rd.DocumentId);

        _ = builder.HasOne(rd => rd.Document)
                   .WithOne()
                   .HasForeignKey<RecipeDocument>(rd => rd.DocumentId);

        _ = builder.HasOne(rd => rd.Recipe)
                   .WithMany(r => r.Images)
                   .HasForeignKey(rd => rd.RecipeId);
    }
}
