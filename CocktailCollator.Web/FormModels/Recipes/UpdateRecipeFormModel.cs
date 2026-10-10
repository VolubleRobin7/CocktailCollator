using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;
using CocktailCollator.Web.Common.Inputs;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.RecipeCategories;
using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web.FormModels.Recipes;

public class UpdateRecipeFormModel : IFormModel<UpdateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public DocumentInputPropertyList Images { get; set; } = [];
    public InputPropertyList<UpdateRecipeFormModelIngredient> Ingredients { get; set; }
        = new([(ingredient) => ingredient.Amount, (ingredient) => ingredient.Measurement, (ingredient) => ingredient.Name]);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<string> Note { get; set; }
        = new(() => string.Empty, (input) => true);
    public InputProperty<string?> PersonalNote { get; set; }
        = new(() => null, (input) => true);
    public InputProperty<Guid> RecipeId { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<RecipeCategoryViewModel?> RecipeCategory { get; set; }
        = new(() => null, (input) => true);
    public InputPropertyList<UpdateRecipeFormModelStep> Steps { get; set; }
        = new([(step) => step.Instruction, (step) => step.Order]);

    public Action? OnChange { get; set; }

    public UpdateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Images.OnChange = () => OnChange?.Invoke();
        this.Ingredients.OnChange = () => OnChange?.Invoke();
        this.Name.OnChange = () => OnChange?.Invoke();
        this.Note.OnChange = () => OnChange?.Invoke();
        this.PersonalNote.OnChange = () => OnChange?.Invoke();
        this.RecipeId.OnChange = () => OnChange?.Invoke();
        this.RecipeCategory.OnChange = () => OnChange?.Invoke();
        this.Steps.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRecipeInputPort ExtractToInputPort()
    {
        var _InputPort = this._mapper.Map<UpdateRecipeInputPort>(this);
        _InputPort.RecipeCategoryId = this.RecipeCategory.Input?.RecipeCategoryId;
        _InputPort.Note = this.Note.Input;
        _InputPort.PersonalNote = this.PersonalNote.Input;
        return _InputPort;
    }

    public bool IsValid()
        => this.Name.IsValid()
            && this.Steps.IsValid()
            && this.Ingredients.IsValid()
            && this.Images.IsValid();

    public void PopulateFrom(RecipeViewModel recipe)
    {
        this.RecipeId.Input = recipe.RecipeId;
        this.Name.Input = recipe.Name ?? string.Empty;
        this.RecipeCategory.Input = recipe.Category;
        this.Note.Input = recipe.Note;
        this.PersonalNote.ResetToDefault();

        this.Images.ResetToDefault();
        this.Images.AddRange(recipe.Images?.Select(i => i.AsExistingDocument()) ?? []);

        this.Ingredients.Clear();
        foreach (var ingredient in recipe.Ingredients ?? [])
        {
            var newIngredient = new UpdateRecipeFormModelIngredient();
            newIngredient.Amount.Input = ingredient.Amount ?? 0;
            newIngredient.ExistingIngredientId = ingredient.IngredientId;
            newIngredient.Name.Input = ingredient.Ingredient?.Name ?? "";
            newIngredient.Measurement.Input = ingredient.MeasurementId;
            newIngredient.MeasurementModel = ingredient.Measurement;
            this.Ingredients.Add(newIngredient);
        }

        this.Steps.Clear();
        foreach (var step in recipe.Steps ?? [])
        {
            var newStep = new UpdateRecipeFormModelStep();
            newStep.Instruction.Input = step.Instruction ?? "";
            newStep.Order.Input = step.Order ?? 0;
            this.Steps.Add(newStep);
        }
    }

    public void ResetToDefault()
    {
        this.RecipeId.ResetToDefault();
        this.Name.ResetToDefault();
        this.RecipeCategory.ResetToDefault();
        this.Ingredients.ResetToDefault();
        this.Steps.ResetToDefault();
        this.Images.ResetToDefault();
        this.Note.ResetToDefault();
        this.PersonalNote.ResetToDefault();
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
