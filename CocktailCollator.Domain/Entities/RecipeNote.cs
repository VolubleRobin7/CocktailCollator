namespace CocktailCollator.Domain.Entities;

public class RecipeNote
{
    public Guid UserId { get; set; }
    public Guid RecipeId { get; set; }
    public string Note { get; set; } = "";

    public Recipe? Recipe { get; set; }
}
