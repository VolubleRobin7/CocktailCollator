using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.RecipeIngredients;

public class RecipeIngredientProfile : Profile
{
    public RecipeIngredientProfile()
    {
        _ = this.CreateMap<RecipeIngredient, RecipeIngredientViewModel>();
    }
}
