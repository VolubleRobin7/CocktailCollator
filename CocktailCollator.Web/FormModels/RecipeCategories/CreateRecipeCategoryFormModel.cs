using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.RecipeCategories;

public class CreateRecipeCategoryFormModel : IFormModel<CreateRecipeCategoryInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateRecipeCategoryFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateRecipeCategoryInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRecipeCategoryInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
