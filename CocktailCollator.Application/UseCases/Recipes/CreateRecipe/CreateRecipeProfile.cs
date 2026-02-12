using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeProfile : Profile
{
    public CreateRecipeProfile()
    {
        _ = this.CreateMap<CreateRecipeInputPort, Recipe>();

        _ = this.CreateMap<CreateRecipeInputPortIngredient, Ingredient>();

        _ = this.CreateMap<CreateRecipeInputPortIngredient, RecipeIngredient>()
            .ForMember(d => d.Ingredient, o => o.MapFrom(src => src));

        _ = this.CreateMap<CreateRecipeInputPortStep, RecipeStep>();

        _ = this.CreateMap<CreateRecipeInputPortMeasurement, Measurement>();
    }
}
