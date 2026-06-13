using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.FormModels.IngredientCategories;
using CocktailCollator.Web.FormModels.Ingredients;
using CocktailCollator.Web.FormModels.Measurements;
using CocktailCollator.Web.FormModels.RecipeCategories;
using CocktailCollator.Web.FormModels.Recipes;
using CocktailCollator.Web.FormModels.Roles;
using CocktailCollator.Web.FormModels.Users;
using CocktailCollator.Web.Infrastructure.Authentication;
using CocktailCollator.Web.ViewModels.IngredientCategories;
using CocktailCollator.Web.ViewModels.Ingredients;
using CocktailCollator.Web.ViewModels.Measurements;
using CocktailCollator.Web.ViewModels.RecipeCategories;
using CocktailCollator.Web.ViewModels.Recipes;
using CocktailCollator.Web.ViewModels.Roles;
using CocktailCollator.Web.ViewModels.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Web;

public static class DependencyInjector
{
    public static IServiceCollection InjectWeb(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddAuth(configuration)
            .AddFormModels()
            .AddViewModels();

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var enforcePasswordPolicies = configuration.GetValue<bool>("EnforcePasswordPolicies");

        _ = services.AddIdentity<CocktailUser, CocktailRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = enforcePasswordPolicies;
                options.Password.RequireNonAlphanumeric = enforcePasswordPolicies;
                options.Password.RequiredLength = enforcePasswordPolicies ? 8 : 0;
                options.Password.RequireUppercase = enforcePasswordPolicies;
                options.Password.RequireLowercase = enforcePasswordPolicies;
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
            .AddAuthorization(Policies.AddPolicies);
    }

    private static IServiceCollection AddFormModels(this IServiceCollection services)
        => services
            .AddScoped<ChangePasswordFormModel>()
            .AddScoped<CreateIngredientCategoryFormModel>()
            .AddScoped<CreateMeasurementFormModel>()
            .AddScoped<CreateRecipeCategoryFormModel>()
            .AddScoped<CreateRecipeFormModel>()
            .AddScoped<CreateRoleFormModel>()
            .AddScoped<CreateUserFormModel>()
            .AddScoped<UpdateIngredientFormModel>()
            .AddScoped<UpdateRecipeFormModel>()
            .AddScoped<UpdateRoleFormModel>()
            .AddScoped<UpdateRolesFormModel>();

    private static IServiceCollection AddViewModels(this IServiceCollection services)
        => services
            .AddScoped<RecipesViewModel>()
            .AddScoped<IngredientsViewModel>()
            .AddScoped<MeasurementsViewModel>()
            .AddScoped<IngredientCategoriesViewModel>()
            .AddScoped<RecipeCategoriesViewModel>()
            .AddScoped<RolesViewModel>()
            .AddScoped<UsersViewModel>();
}
