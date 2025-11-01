using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.RecipeSteps;

public class RecipeStepProfile : Profile
{
    public RecipeStepProfile()
    {
        _ = this.CreateMap<RecipeStep, RecipeStepViewModel>();
    }
}
