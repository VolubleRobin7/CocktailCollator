using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

namespace CocktailCollator.Web.FormModels.RecipeCategories;

public class RecipeCategoryFormModelProfile : Profile
{
    public RecipeCategoryFormModelProfile()
    {
        _ = this.CreateMap<CreateRecipeCategoryFormModel, CreateRecipeCategoryInputPort>();
    }
}
