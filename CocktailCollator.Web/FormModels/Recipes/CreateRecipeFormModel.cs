using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.FormModels.Recipes;

public class CreateRecipeFormModel : IFormModel<CreateRecipeInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateRecipeFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateRecipeInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRecipeInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
