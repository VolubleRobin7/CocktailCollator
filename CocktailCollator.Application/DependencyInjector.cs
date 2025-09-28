using CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailCollator.Application;
public static class DependencyInjector
{
    public static IServiceCollection InjectApplication(this IServiceCollection services)
        => services
            .AddUseCaseInteractors();

    private static IServiceCollection AddUseCaseInteractors(this IServiceCollection services)
        => services
            .AddScoped<CreateRecipeInteractor>()
            .AddScoped<CreateIngredientInteractor>();
}
