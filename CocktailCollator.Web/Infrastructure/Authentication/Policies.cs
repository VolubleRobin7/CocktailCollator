using Microsoft.AspNetCore.Authorization;

namespace CocktailCollator.Web.Infrastructure.Authentication;

public static class Policies
{
#pragma warning disable IDE1006
    public const string ManageUsers = "ManageUsers";
    public const string ViewUsers = "ViewUsers";
    public const string ManageRoles = "ManageRoles";
    public const string ViewRoles = "ViewRoles";
    public const string ChangeUserPasswords = "ChangeUserPasswords";
    public const string ManageRecipes = "ManageRecipes";
    public const string ViewIngredients = "ViewIngredients";
    public const string ManageIngredients = "ManageIngredients";
    public const string ViewMeasurements = "ViewMeasurements";
    public const string ManageMeasurements = "ManageMeasurements";
#pragma warning restore IDE1006

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.ManageUsers, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Users.Manage));
        options.AddPolicy(Policies.ViewUsers, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Users.View));
        options.AddPolicy(Policies.ManageRoles, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Roles.Manage));
        options.AddPolicy(Policies.ViewRoles, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Roles.View));
        options.AddPolicy(Policies.ChangeUserPasswords, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Users.ChangePassword));
        options.AddPolicy(Policies.ManageRecipes, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Recipes.Manage));
        options.AddPolicy(Policies.ViewIngredients, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Ingredients.View));
        options.AddPolicy(Policies.ManageIngredients, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Ingredients.Manage));
        options.AddPolicy(Policies.ViewMeasurements, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Measurements.View));
        options.AddPolicy(Policies.ManageMeasurements, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Measurements.Manage));
    }
}
