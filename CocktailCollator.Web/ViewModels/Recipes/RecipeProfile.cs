using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        _ = this.CreateMap<Recipe, RecipeViewModel>()
            .ForMember(d => d.Ingredients, o => o.MapFrom(s => s.Ingredients!.Select(ri => ri.Ingredient)));
    }
}
