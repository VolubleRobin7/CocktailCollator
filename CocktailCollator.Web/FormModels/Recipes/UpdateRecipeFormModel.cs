using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Web.Common.Inputs;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.RecipeCategories;

namespace CocktailCollator.Web.FormModels.Recipes;

public class UpdateRecipeFormModel : IFormModel<UpdateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public DocumentInputPropertyList Images { get; set; } = [];
    public InputPropertyList<UpdateRecipeFormModelIngredient> Ingredients { get; set; }
        = new([(ingredient) => ingredient.Amount, (ingredient) => ingredient.Measurement, (ingredient) => ingredient.Name]);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<Guid> RecipeId { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<RecipeCategoryViewModel?> RecipeCategory { get; set; }
        = new(() => null, (input) => true);
    public InputPropertyList<UpdateRecipeFormModelStep> Steps { get; set; }
        = new([(step) => step.Instruction, (step) => step.Order]);
    public InputProperty<string?> GlobalNote { get; set; }
        = new(() => null, (input) => true);

    public Action? OnChange { get; set; }

    public UpdateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Images.OnChange = () => OnChange?.Invoke();
        this.Ingredients.OnChange = () => OnChange?.Invoke();
        this.Name.OnChange = () => OnChange?.Invoke();
        this.RecipeId.OnChange = () => OnChange?.Invoke();
        this.RecipeCategory.OnChange = () => OnChange?.Invoke();
        this.Steps.OnChange = () => OnChange?.Invoke();
        this.GlobalNote.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRecipeInputPort ExtractToInputPort()
    {
        var _InputPort = this._mapper.Map<UpdateRecipeInputPort>(this);
        _InputPort.RecipeCategoryId = this.RecipeCategory.Input?.RecipeCategoryId;
        return _InputPort;
    }

    public bool IsValid()
        => this.Name.IsValid()
            && this.Steps.IsValid()
            && this.Ingredients.IsValid()
            && this.Images.IsValid();

    public void ResetToDefault()
    {
        this.RecipeId.ResetToDefault();
        this.Name.ResetToDefault();
        this.RecipeCategory.ResetToDefault();
        this.Ingredients.ResetToDefault();
        this.Steps.ResetToDefault();
        this.Images.ResetToDefault();
        this.GlobalNote.ResetToDefault();
    }
}

public class UpdateRecipeFormModelIngredient
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

public class UpdateRecipeFormModelStep
{
    public InputProperty<string> Instruction { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<int> Order { get; set; }
        = new(() => 0, (input) => true);
}
