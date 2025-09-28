namespace CocktailCollator.Domain.Entities;

public class RecipeStep
{
    public Guid RecipeStepId { get; set; }
    public required string Instruction { get; set; }
    public required int Order { get; set; }

    public Guid RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}
