using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.IngredientCategories;

public class CreateIngredientCategoryFormModel : IFormModel<CreateIngredientCategoryInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateIngredientCategoryFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateIngredientCategoryInputPort ExtractToInputPort()
        => this._mapper.Map<CreateIngredientCategoryInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
