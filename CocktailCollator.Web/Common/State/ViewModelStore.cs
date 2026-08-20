namespace CocktailCollator.Web.Common.State;

public class ViewModelStore : IViewModelStore
{
    // A dictionary where the key is the ViewModel type, and the value is a dictionary mapping the Guid to the instance.
    private readonly Dictionary<Type, Dictionary<Guid, object>> _store = [];

    public TViewModel GetOrRegister<TViewModel>(Guid id, TViewModel instance) where TViewModel : class
        => this.EntityInCache(id, instance, false);

    public TViewModel? Get<TViewModel>(Guid id) where TViewModel : class
        => _store.TryGetValue(typeof(TViewModel), out var typeCache) && typeCache.TryGetValue(id, out var existingInstance)
            ? (TViewModel)existingInstance
            : null;

    public void Remove<TViewModel>(Guid id) where TViewModel : class
    {
        if (_store.TryGetValue(typeof(TViewModel), out var typeCache))
            _ = typeCache.Remove(id);
    }

    public TViewModel UpdateOrRegister<TViewModel>(Guid id, TViewModel instance) where TViewModel : class
        => this.EntityInCache(id, instance, true);

    private TViewModel EntityInCache<TViewModel>(Guid id, TViewModel instance, bool update) where TViewModel : class
    {
        // Try get the type-specific cache; if it doesn't exist, create it
        if (!_store.TryGetValue(typeof(TViewModel), out var typeCache))
            _store[typeof(TViewModel)] = typeCache ??= [];

        // Try get the existing instance from the type-specific cache
        if (typeCache.TryGetValue(id, out var existingInstance))
        {
            var _Existing = (TViewModel)existingInstance;

            // If the view model knows how to update itself, apply the fresh data to the cached instance
            if (update && _Existing is IStoreableViewModel<TViewModel> storeable)
                storeable.ApplyChanges(instance, this);

            return _Existing;
        }
        else
        {
            typeCache[id] = instance;

            // If it's a new instance, we still call ApplyChanges on itself to ensure its nested properties get deduplicated
            if (update && instance is IStoreableViewModel<TViewModel> newStoreable)
                newStoreable.ApplyChanges(instance, this);

            return instance;
        }
    }
}
