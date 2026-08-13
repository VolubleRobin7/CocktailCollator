namespace CocktailCollator.Web.Common.Generics;

public interface IInputInteraction
{
    public string ErrorMessage { get; }
    public Action? OnChange { get; set; }

    public bool IsValid();
    public void ResetToDefault();
}
