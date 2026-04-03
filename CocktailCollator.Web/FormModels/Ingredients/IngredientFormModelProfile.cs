using AutoMapper;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

namespace CocktailCollator.Web.FormModels.Ingredients;

public class IngredientFormModelProfile : Profile
{
    public IngredientFormModelProfile()
    {
        _ = this.CreateMap<UpdateIngredientFormModel, UpdateIngredientInputPort>()
            .ForMember(d => d.IngredientId, o => o.MapFrom(s => s.IngredientId.Input))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.IngredientCategoryId, o => o.Ignore());
    }
}
