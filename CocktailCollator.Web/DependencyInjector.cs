using CocktailCollator.Web.ViewModels.Recipes;

namespace CocktailCollator.Web;

public static class DependencyInjector
{
    public static IServiceCollection InjectWeb(this IServiceCollection services)
        => services
            .AddViewModels();

    private static IServiceCollection AddViewModels(this IServiceCollection services)
        => services
            .AddScoped<RecipesViewModel>();
}
