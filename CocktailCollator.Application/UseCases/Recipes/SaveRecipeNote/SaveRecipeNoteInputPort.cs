namespace CocktailCollator.Application.UseCases.Recipes.SaveRecipeNote;

public class SaveRecipeNoteInputPort
{
    public required Guid RecipeId { get; set; }
    public required Guid UserId { get; set; }
    public string? Note { get; set; }
}
