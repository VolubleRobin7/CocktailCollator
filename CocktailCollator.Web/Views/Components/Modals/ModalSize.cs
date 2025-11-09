namespace CocktailCollator.Web.Views.Components.Modals;

public class ModalSize
{
    private readonly string _css;

    public readonly static ModalSize Small = new("modal-sm");
    public readonly static ModalSize Medium = new("");
    public readonly static ModalSize Large = new("modal-lg");
    public readonly static ModalSize ExtraLarge = new("modal-xl");
    public readonly static ModalSize Fullscreen = new("modal-fullscreen");
    public readonly static ModalSize FullscreenBelowSmall = new("modal-fullscreen-sm-down");
    public readonly static ModalSize FullscreenBelowMedium = new("modal-fullscreen-md-down");
    public readonly static ModalSize FullscreenBelowLarge = new("modal-fullscreen-lg-down");
    public readonly static ModalSize FullscreenBelowExtraLarge = new("modal-fullscreen-xl-down");
    public readonly static ModalSize FullscreenBelowExtraExtraLarge = new("modal-fullscreen-xxl-down");

    public string CssClass
    {
        get => this._css;
    }

    private ModalSize(string css)
        => this._css = css;
}
