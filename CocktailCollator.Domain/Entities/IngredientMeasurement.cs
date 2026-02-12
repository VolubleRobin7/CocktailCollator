namespace CocktailCollator.Domain.Entities;

public class IngredientMeasurement
{
    public Guid IngredientId { get; set; }
    public Guid MeasurementId { get; set; }

    public Ingredient? Ingredient { get; set; }
    public Measurement? Measurement { get; set; }
}
