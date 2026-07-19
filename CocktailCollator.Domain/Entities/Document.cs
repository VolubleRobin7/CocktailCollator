namespace CocktailCollator.Domain.Entities;

public class Document
{
    public Guid DocumentId { get; set; }
    public required string FilePath { get; set; }
    public required string OriginalFileName { get; set; }
}
