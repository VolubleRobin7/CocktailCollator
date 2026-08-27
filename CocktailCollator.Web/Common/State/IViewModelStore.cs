namespace CocktailCollator.Web.Common.State;

public interface IViewModelStore
{
    /// <summary>
    /// Gets an existing ViewModel from the store if it exists, otherwise registers the provided instance and returns it.
    /// </summary>
    /// <remarks>
    /// This method will <b>not</b> update the stored entity with the instance provided, only getting the existing.
    /// </remarks>
    TViewModel GetOrRegister<TViewModel>(Guid id, TViewModel instance) where TViewModel : class;

    /// <summary>
    /// Gets an existing ViewModel from the store if it exists, otherwise returns null.
    /// </summary>
    TViewModel? Get<TViewModel>(Guid id) where TViewModel : class;

    /// <summary>
    /// Removes a ViewModel from the store.
    /// </summary>
    void Remove<TViewModel>(Guid id) where TViewModel : class;

    /// <summary>
    /// Gets and calls <see cref="IStoreableViewModel{TViewModel}.ApplyChanges(TViewModel, IViewModelStore)"/> 
    /// on the existing ViewModel from the store if it exists, otherwise registers the provided instance and returns it.
    /// </summary>
    TViewModel UpdateOrRegister<TViewModel>(Guid id, TViewModel instance) where TViewModel : class;
}
