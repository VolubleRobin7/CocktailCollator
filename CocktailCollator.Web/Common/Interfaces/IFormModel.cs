namespace CocktailCollator.Web.Common.Interfaces;

public interface IFormModel<TInputPort> where TInputPort : class
{
    TInputPort ExtractToInputPort();
    bool IsValid();
    void ResetToDefault();
}
