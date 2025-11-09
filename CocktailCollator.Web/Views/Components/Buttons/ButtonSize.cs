namespace CocktailCollator.Web.Views.Components.Buttons;

public class ButtonSize
{
    private readonly string _css;

    public readonly static ButtonSize Small = new("btn-sm");
    public readonly static ButtonSize Medium = new("");
    public readonly static ButtonSize Large = new("btn-lg");

    public string CssClass
    {
        get => this._css;
    }

    private ButtonSize(string css)
        => this._css = css;
}
