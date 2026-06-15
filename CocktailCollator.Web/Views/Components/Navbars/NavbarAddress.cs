using CocktailCollator.Web.Infrastructure.Authentication;

namespace CocktailCollator.Web.Views.Components.Navbars;

public class NavbarAddress
{
    public readonly static NavbarAddress Recipes = new("", "Recipes", null);
    public readonly static NavbarAddress Ingredients = new("ingredients", "Ingredients", Policies.ViewIngredients);
    public readonly static NavbarAddress Measurements = new("measurements", "Measurements", Policies.ViewMeasurements);

    public readonly string DisplayName;
    public readonly string? Policy;
    public readonly string RelativeUri;

    private NavbarAddress(string address, string name, string? policy = null)
    {
        this.DisplayName = name;
        this.Policy = policy;
        this.RelativeUri = address;
    }
}
