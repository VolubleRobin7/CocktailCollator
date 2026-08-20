namespace CocktailCollator.Web.Common.State;

/// <summary>
/// Defines a ViewModel that can update its own state from another instance of the same type.
/// </summary>
/// <remarks>
/// Used by <see cref="ViewModelStore"/> to update cached instances with fresh data.
/// </remarks>
public interface IStoreableViewModel<TViewModel> where TViewModel : class
{
    void ApplyChanges(TViewModel source, IViewModelStore store);
}
