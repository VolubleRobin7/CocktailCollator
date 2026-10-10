using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Recipes;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        _ = this.CreateMap<Recipe, RecipeViewModel>()
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images!.Select(i => i.Document)))
            .ForMember(d => d.PersonalNote, o => o.MapFrom(s => s.PersonalRecipeNotes != null && s.PersonalRecipeNotes.Any() ? s.PersonalRecipeNotes.First().Note : string.Empty));
    }
}
