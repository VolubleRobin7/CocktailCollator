using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeProfile : Profile
{
    public CreateRecipeProfile()
    {
        this.CreateMap<CreateRecipeInputPort, Recipe>()
            .ForMember(d => d.Ingredients, o => o.Ignore());

        this.CreateMap<CreateRecipeInputPortIngredient, Ingredient>();

        this.CreateMap<Ingredient, RecipeIngredient>()
            .ForMember(d => d.Ingredient, o => o.MapFrom(s => s));
    }
}
