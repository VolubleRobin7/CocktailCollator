namespace CocktailCollator.Web.Views.Components.Navbars;

public class NavbarAddress
{
    public readonly static NavbarAddress Recipes = new("", "Recipes", false);
    public readonly static NavbarAddress Ingredients = new("ingredients", "Ingredients", true);
    public readonly static NavbarAddress Measurements = new("measurements", "Measurements", true);

    public readonly bool AuthRequired;
    public readonly string DisplayName;
    public readonly string RelativeUri;

    private NavbarAddress(string address, string name, bool authRequired)
    {
        this.AuthRequired = authRequired;
        this.DisplayName = name;
        this.RelativeUri = address;
    }
}
