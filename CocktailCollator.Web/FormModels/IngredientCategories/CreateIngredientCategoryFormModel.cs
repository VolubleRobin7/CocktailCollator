using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.IngredientCategories;

public class CreateIngredientCategoryFormModel(IMapper mapper) : IFormModel<CreateIngredientCategoryInputPort>
{
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public CreateIngredientCategoryInputPort ExtractToInputPort()
        => mapper.Map<CreateIngredientCategoryInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
