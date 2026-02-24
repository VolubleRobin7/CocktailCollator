using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeProfile : Profile
{
    public CreateRecipeProfile()
    {
        _ = this.CreateMap<CreateRecipeInputPort, Recipe>();

        _ = this.CreateMap<CreateRecipeInputPortIngredient, Ingredient>();

        _ = this.CreateMap<CreateRecipeInputPortRecipeIngredient, RecipeIngredient>()
            .AfterMap((src, dest) =>
            {
                if (dest.Ingredient is not null)
                    dest.Ingredient.Measurements = [new() { MeasurementId = dest.MeasurementId }];
            });

        _ = this.CreateMap<CreateRecipeInputPortStep, RecipeStep>();
    }
}
