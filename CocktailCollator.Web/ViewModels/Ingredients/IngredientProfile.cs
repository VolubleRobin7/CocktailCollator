using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Ingredients;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        _ = this.CreateMap<Ingredient, IngredientViewModel>()
            .ForMember(d => d.Recipes, o => o.Ignore())
            .ForMember(d => d.Measurements, o =>
            {
                // Ensures that all the measurements are loaded before mapping to the view model, to ensure that missing data is not hidden.
                o.PreCondition(s => s.Measurements is not null && s.Measurements.All(im => im.Measurement is not null));
                o.MapFrom(s => s.Measurements!.Select(im => im.Measurement));
            });
    }
}
