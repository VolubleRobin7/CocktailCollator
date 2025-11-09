namespace CocktailCollator.Web.Views.Components.Buttons;

public class ButtonTheme
{
    private readonly string _css;

    public readonly static ButtonTheme None = new("");
    public readonly static ButtonTheme Primary = new("btn-primary");
    public readonly static ButtonTheme Secondary = new("btn-secondary");
    public readonly static ButtonTheme Success = new("btn-success");
    public readonly static ButtonTheme Danger = new("btn-danger");
    public readonly static ButtonTheme Warning = new("btn-warning");
    public readonly static ButtonTheme Info = new("btn-info");
    public readonly static ButtonTheme Light = new("btn-light");
    public readonly static ButtonTheme Dark = new("btn-dark");

    public string CssClass
    {
        get => this._css;
    }

    private ButtonTheme(string css)
        => this._css = css;
}
