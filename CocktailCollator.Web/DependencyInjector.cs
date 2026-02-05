using CocktailCollator.Web.FormModels.Recipes;
using CocktailCollator.Web.ViewModels.Ingredients;
using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web;

public static class DependencyInjector
{
    public static IServiceCollection InjectWeb(this IServiceCollection services)
        => services
            .AddFormModels()
            .AddViewModels();

    private static IServiceCollection AddFormModels(this IServiceCollection services)
        => services
            .AddScoped<CreateRecipeFormModel>();

    private static IServiceCollection AddViewModels(this IServiceCollection services)
        => services
            .AddScoped<RecipesViewModel>()
            .AddScoped<IngredientsViewModel>();
}
