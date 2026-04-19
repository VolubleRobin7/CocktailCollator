namespace CocktailCollator.Web.Views.Components.Navbars;

public class NavbarAddress
{
    public readonly static NavbarAddress Recipes = new("", "Recipes");
    public readonly static NavbarAddress Ingredients = new("ingredients", "Ingredients");
    public readonly static NavbarAddress Measurements = new("measurements", "Measurements");

    public readonly string DisplayName;
    public readonly string RelativeUri;

    private NavbarAddress(string address, string name)
    {
        this.DisplayName = name;
        this.RelativeUri = address;
    }
}
