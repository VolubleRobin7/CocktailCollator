namespace CocktailCollator.Domain.Entities;

public class RecipeDocument
{
    public Guid RecipeId { get; set; }
    public Guid DocumentId { get; set; }

    public Recipe? Recipe { get; set; }
    public Document? Document { get; set; }
}
