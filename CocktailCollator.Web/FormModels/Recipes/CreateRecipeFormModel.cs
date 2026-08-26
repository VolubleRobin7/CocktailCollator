using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Inputs;
using CocktailCollator.Web.ViewModels.Measurements;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel : IFormModel<CreateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public DocumentInputPropertyList Images { get; set; } = [];
    public InputPropertyList<CreateRecipeFormModelIngredient> Ingredients { get; set; }
        = new([(ingredient) => ingredient.Amount, (ingredient) => ingredient.Measurement, (ingredient) => ingredient.Name]);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputPropertyList<CreateRecipeFormModelStep> Steps { get; set; }
        = new([(step) => step.Instruction, (step) => step.Order]);

    public Action? OnChange { get; set; }

    public CreateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Images.OnChange = () => OnChange?.Invoke();
        this.Ingredients.OnChange = () => OnChange?.Invoke();
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
    public bool UsingExistingIngredient { get; set; } = true;
}

public class CreateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
