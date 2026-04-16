namespace CocktailCollator.Web.Common.Interfaces;

public interface IFormModel<TInputPort> where TInputPort : class
{
    //Action? OnFormChange { get; set; }

    TInputPort ExtractToInputPort();
    bool IsValid();
    void ResetToDefault();
}
