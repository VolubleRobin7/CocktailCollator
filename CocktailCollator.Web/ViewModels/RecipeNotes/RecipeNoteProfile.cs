using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.RecipeNotes;

public class RecipeNoteProfile : Profile
{
    public RecipeNoteProfile()
    {
        _ = this.CreateMap<RecipeNote, RecipeNoteViewModel>();
    }
}
