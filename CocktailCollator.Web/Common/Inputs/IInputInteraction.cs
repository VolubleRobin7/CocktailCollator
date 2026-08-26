namespace CocktailCollator.Web.Common.Inputs;

public interface IInputInteraction
{
    public string ErrorMessage { get; }
    public Action? OnChange { get; set; }

    public bool IsValid();
    public void ResetToDefault();
}
