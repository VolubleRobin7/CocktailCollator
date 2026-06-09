using Microsoft.AspNetCore.Authorization;

namespace CocktailCollator.Web.Infrastructure.Authentication;

public static class Policies
{
#pragma warning disable IDE1006
    public const string ManageUsers = "ManageUsers";
    public const string ViewIngredients = "ViewIngredients";
    public const string ManageIngredients = "ManageIngredients";
    public const string ViewMeasurements = "ViewMeasurements";
    public const string ManageMeasurements = "ManageMeasurements";
#pragma warning restore IDE1006

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.ManageUsers, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Users.Manage));
        options.AddPolicy(Policies.ViewIngredients, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Ingredients.View));
        options.AddPolicy(Policies.ManageIngredients, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Ingredients.Manage));
        options.AddPolicy(Policies.ViewMeasurements, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Measurements.View));
        options.AddPolicy(Policies.ManageMeasurements, policy => policy.RequireClaim(ClaimTypes.Permission, ClaimValues.Permissions.Measurements.Manage));
    }
}
