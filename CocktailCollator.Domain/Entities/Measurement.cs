namespace CocktailCollator.Domain.Entities;

public class Measurement
{
    public Guid MeasurementId { get; set; }
    public required string Name { get; set; }

    public ICollection<IngredientMeasurement>? Ingredients { get; set; }
    public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
}
