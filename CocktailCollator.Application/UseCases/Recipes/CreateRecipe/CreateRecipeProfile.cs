using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeProfile : Profile
{
    public CreateRecipeProfile()
    {
        this.CreateMap<CreateRecipeInputPort, Recipe>();

        this.CreateMap<CreateRecipeInputPortIngredient, Ingredient>();

        this.CreateMap<CreateRecipeInputPortIngredient, RecipeIngredient>()
            .ForMember(d => d.Ingredient, o => o.MapFrom(src => src));
    }
}
