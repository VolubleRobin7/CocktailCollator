namespace CocktailCollator.Web.Common.Interfaces;

public interface IFormModel<TInputPort> where TInputPort : class
{
    Action? OnChange { get; set; }

    TInputPort ExtractToInputPort();
    bool IsValid();
    void ResetToDefault();
}
