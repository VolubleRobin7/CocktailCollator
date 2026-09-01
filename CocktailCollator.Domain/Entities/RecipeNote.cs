namespace CocktailCollator.Domain.Entities;

public class RecipeNote
{
    public Guid RecipeNoteId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid UserId { get; set; }
    public string? Note { get; set; }

    public Recipe? Recipe { get; set; }
}
