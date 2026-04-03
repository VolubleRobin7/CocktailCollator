using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using CocktailCollator.Web.ViewModels.IngredientCategories;

namespace CocktailCollator.Web.FormModels.Ingredients;

public class UpdateIngredientFormModel(IMapper mapper) : IFormModel<UpdateIngredientInputPort>
{
    public InputProperty<IngredientCategoryViewModel?> IngredientCategory { get; set; } 
        = new(() => null, (input) => true);
    public InputProperty<Guid> IngredientId { get; set; } 
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<string> Name { get; set; } 
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public UpdateIngredientInputPort ExtractToInputPort()
    {
        var _InputPort = mapper.Map<UpdateIngredientInputPort>(this);
        _InputPort.IngredientCategoryId = this.IngredientCategory.Input?.IngredientCategoryId;
        return _InputPort;
    }

    public bool IsValid()
        => this.IngredientId.IsValid() && this.Name.IsValid();

    public void ResetToDefault()
    {
        this.IngredientId.ResetToDefault();
        this.Name.ResetToDefault();
        this.IngredientCategory.ResetToDefault();
    }
}
