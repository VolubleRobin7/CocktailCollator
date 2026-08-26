namespace CocktailCollator.Web.Common.Inputs;

public interface IFormModel<TInputPort> where TInputPort : class
{
    Action? OnChange { get; set; }

    TInputPort ExtractToInputPort();
    bool IsValid();
    void ResetToDefault();
}
