using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;
using CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;
using CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;
using CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;
using CocktailCollator.Application.UseCases.Ingredients.GetIngredients;
using CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.GetRecipeCategories;
using CocktailCollator.UseCasePipelines;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailCollator.Application;

public static class DependencyInjector
{
    public static IServiceCollection InjectApplication(this IServiceCollection services)
        => services
            .AddUseCasePipelines(typeof(DependencyInjector).Assembly)
            .AddUseCaseInteractors();

    private static IServiceCollection AddUseCaseInteractors(this IServiceCollection services)
        => services
            .AddScoped<CreateIngredientInteractor>()
            .AddScoped<GetIngredientsInteractor>()
            .AddScoped<DeleteIngredientInteractor>()
            .AddScoped<UpdateIngredientInteractor>()
            .AddScoped<CreateMeasurementInteractor>()
            .AddScoped<GetMeasurementsInteractor>()
            .AddScoped<DeleteMeasurementInteractor>()
            .AddScoped<CreateIngredientCategoryInteractor>()
            .AddScoped<GetIngredientCategoriesInteractor>()
            .AddScoped<DeleteIngredientCategoryInteractor>()
            .AddScoped<CreateRecipeCategoryInteractor>()
            .AddScoped<GetRecipeCategoriesInteractor>()
            .AddScoped<DeleteRecipeCategoryInteractor>();
}
