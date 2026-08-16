using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using CocktailCollator.Web.ViewModels.Measurements;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel : IFormModel<CreateRecipeInputPort>
{
    private readonly IMapper _mapper;

    // I'm now more thinking that these should be more like List<InputProp<object...
    // That would allow more precise control, could set better validation, and reset per item
    // Or possibly even InputProp<List<InputProp<object... but that could be overkill
    // Could require the creation of a new class that wraps List, something like InputPropertyList<T>
    // If I do that, I would have to handle how each InputProperty gets created on add to list,
    // as the consumer would likely then have to handle the constructor parameters.
    // That would not be ideal. Maybe alter the add method to just take in the object?
    public DocumentInputPropertyList Images { get; set; } = [];
    public InputProperty<ObservableCollection<CreateRecipeFormModelIngredient>> Ingredients { get; set; }
        = new(() => [], ValidateIngredients);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputPropertyList<CreateRecipeFormModelStep> Steps { get; set; }
        = new([(step) => step.Instruction, (step) => step.Order]);

    public Action? OnChange { get; set; }

    public CreateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Images.OnChange = () => OnChange?.Invoke();
        this.Ingredients.Input.CollectionChanged += (_, args) =>
        {
            if (args.NewItems is not null)
            {
                foreach (CreateRecipeFormModelIngredient ingredient in args.NewItems)
                {
                    ingredient.Amount.OnChange = () => OnChange?.Invoke();
                    ingredient.Measurement.OnChange = () => OnChange?.Invoke();
                    ingredient.Name.OnChange = () => OnChange?.Invoke();
                }
            }
        };
        this.Name.OnChange = () => OnChange?.Invoke();
        this.Steps.OnChange = () => OnChange?.Invoke();
    }

    public CreateRecipeInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid()
            && this.Steps.IsValid()
            && this.Ingredients.IsValid()
            && this.Images.IsValid();

    public void ResetToDefault()
    {
        this.Name.ResetToDefault();
        this.Steps.ResetToDefault();
        this.Ingredients.ResetToDefault();
        this.Images.ResetToDefault();
    }

    private static bool ValidateIngredients(ObservableCollection<CreateRecipeFormModelIngredient> ingredients)
    {
        return ingredients.All(ingredient => ingredient.Name.IsValid()
            && ingredient.Amount.IsValid()
            && ingredient.Measurement.IsValid());
    }
}

public class CreateRecipeFormModelIngredient
{
    public InputProperty<decimal> Amount { get; set; }
        = new(() => 1m, (input) => true);
    public Guid ExistingIngredientId { get; set; }
    public InputProperty<Guid> Measurement { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public MeasurementViewModel? MeasurementModel { get; set; }
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<bool> UsingExistingIngredient { get; set; }
        = new(() => true, (_) => true);
}

public class CreateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
