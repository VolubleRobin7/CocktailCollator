using CocktailCollator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CocktailCollator.Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    void IEntityTypeConfiguration<Document>.Configure(EntityTypeBuilder<Document> builder)
    {
        _ = builder.Property(f => f.FilePath)
            .HasMaxLength(2048);

        _ = builder.Property(f => f.OriginalFileName)
            .HasMaxLength(255);
    }
}
