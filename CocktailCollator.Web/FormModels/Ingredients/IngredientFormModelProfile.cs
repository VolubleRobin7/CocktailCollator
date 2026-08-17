using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

namespace CocktailCollator.Web.FormModels.Ingredients;

public class IngredientFormModelProfile : Profile
{
    public IngredientFormModelProfile()
    {
        _ = this.CreateMap<UpdateIngredientFormModel, UpdateIngredientInputPort>()
            .ForMember(d => d.IngredientCategoryId, o => o.Ignore());
    }
}
