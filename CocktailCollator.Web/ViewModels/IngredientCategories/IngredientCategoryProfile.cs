using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.IngredientCategories;

public class IngredientCategoryProfile : Profile
{
    public IngredientCategoryProfile()
    {
        _ = this.CreateMap<IngredientCategory, IngredientCategoryViewModel>();
    }
}
