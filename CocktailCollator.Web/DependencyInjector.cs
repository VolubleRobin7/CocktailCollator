using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.FormModels.IngredientCategories;
using CocktailCollator.Web.FormModels.Ingredients;
using CocktailCollator.Web.FormModels.Measurements;
using CocktailCollator.Web.FormModels.Recipes;
using CocktailCollator.Web.FormModels.Users;
using CocktailCollator.Web.Infrastructure.Authentication;
using CocktailCollator.Web.ViewModels.IngredientCategories;
using CocktailCollator.Web.ViewModels.Ingredients;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.Recipes;
using CocktailCollator.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Web;

public static class DependencyInjector
{
    public static IServiceCollection InjectWeb(this IServiceCollection services)
        => services
            .AddAuth()
            .AddFormModels()
            .AddViewModels();

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        _ = services.AddIdentity<CocktailUser, CocktailRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<CocktailDbContext>()
            .AddDefaultTokenProviders();

        return services
            .ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/access-denied";
                options.ExpireTimeSpan = TimeSpan.FromDays(14); // 14 days is default
            })
            .AddCascadingAuthenticationState()
            .AddScoped<AuthenticationStateProvider, CocktailAuthenticationStateProvider>()
            .AddAuthorization(AddPolicies);
    }

    private static IServiceCollection AddFormModels(this IServiceCollection services)
        => services
            .AddScoped<ChangePasswordFormModel>()
            .AddScoped<CreateIngredientCategoryFormModel>()
            .AddScoped<CreateMeasurementFormModel>()
            .AddScoped<CreateRecipeFormModel>()
            .AddScoped<CreateUserFormModel>()
            .AddScoped<UpdateIngredientFormModel>()
            .AddScoped<UpdateRecipeFormModel>()
            .AddScoped<UpdateRolesFormModel>();

    private static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Permissions.Users.Manage, policy => policy.RequireClaim("Permission", Permissions.Users.Manage));
        options.AddPolicy(Permissions.Ingredients.View, policy => policy.RequireClaim("Permission", Permissions.Ingredients.View));
        options.AddPolicy(Permissions.Ingredients.Manage, policy => policy.RequireClaim("Permission", Permissions.Ingredients.Manage));
        options.AddPolicy(Permissions.Measurements.View, policy => policy.RequireClaim("Permission", Permissions.Measurements.View));
        options.AddPolicy(Permissions.Measurements.Manage, policy => policy.RequireClaim("Permission", Permissions.Measurements.Manage));
    }

    private static IServiceCollection AddViewModels(this IServiceCollection services)
        => services
            .AddScoped<RecipesViewModel>()
            .AddScoped<IngredientsViewModel>()
            .AddScoped<MeasurementsViewModel>()
            .AddScoped<IngredientCategoriesViewModel>()
            .AddScoped<UsersViewModel>();
}
