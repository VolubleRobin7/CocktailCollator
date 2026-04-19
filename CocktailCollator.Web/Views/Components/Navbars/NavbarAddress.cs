namespace CocktailCollator.Web.Views.Components.Navbars;

public class NavbarAddress
{
    public readonly static NavbarAddress Recipes = new("");
    public readonly static NavbarAddress Ingredients = new("ingredients");
    public readonly static NavbarAddress Measurements = new("measurements");

    public readonly string RelativeUri;

    private NavbarAddress(string address)
    {
        this.RelativeUri = address;
    }
}
