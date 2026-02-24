using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        _ = this.CreateMap<Ingredient, IngredientViewModel>()
            .ForMember(d => d.Recipes, o => o.Ignore())
            .ForMember(d => d.Measurements, o => o.MapFrom(s => s.Measurements!.Select(im => im.Measurement)));
    }
}
