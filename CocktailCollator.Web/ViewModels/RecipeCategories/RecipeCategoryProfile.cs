using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.RecipeCategories;

public class RecipeCategoryProfile : Profile
{
    public RecipeCategoryProfile()
    {
        _ = this.CreateMap<RecipeCategory, RecipeCategoryViewModel>();
    }
}
