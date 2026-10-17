using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel : IFormModel<CreateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public DocumentInputPropertyList Images { get; set; } = [];
    public InputPropertyList<RecipeFormModelIngredient> Ingredients { get; set; }
        = new([(ingredient) => ingredient.Amount, (ingredient) => ingredient.Measurement, (ingredient) => ingredient.Name]);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputPropertyList<RecipeFormModelStep> Steps { get; set; }
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
