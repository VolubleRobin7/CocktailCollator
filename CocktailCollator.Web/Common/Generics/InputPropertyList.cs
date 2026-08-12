using System.Collections;

namespace CocktailCollator.Web.Common.Generics;

public class InputPropertyList<TEntity> : IReadOnlyList<InputProperty<TEntity>>
{

    // TODO: Test implementing IList<> instead of this.

    // TODO: Test implementing generic type TEntity instead of InputProperty<TEntity>.
    //       See if this removes alot of the weird duplication issues.
    //       To handle consumers wanting to directly access their validations and resets,
    //       overload the IsValid and ResetToDefault methods to accept a TEntity and find
    //       the matching InputProperty<TEntity> to call the methods on.
    //       This could possibly cause issues with components that want a single InputProperty.

    #region Fields

    private const string DEFAULT_ERROR_MESSAGE = "Please review your input list.";

    private readonly Func<IEnumerable<InputProperty<TEntity>>, ValidationResult> _collectionValidationFunc;
    private readonly Func<IEnumerable<TEntity>> _defaultEntitiesFunc;
    private readonly List<InputProperty<TEntity>> _items = [];
    private readonly Func<TEntity, ValidationResult> _itemValidationFunc;
    private readonly Action<TEntity, Action?> _onAddOnChange;

    #endregion

    #region Input Interaction Properties

    /// <summary>
    /// The current error message for the collection based on item and collection validation.
    /// </summary>
    public string ErrorMessage { get; private protected set; } = DEFAULT_ERROR_MESSAGE;

    /// <summary>
    /// An Action that is invoked whenever the collection changes or any item's <see cref="InputProperty{TEntity}.Input"/> changes.
    /// </summary>
    public Action? OnChange { get; set; }

    #endregion

    #region List Manipulation Properties

    /// <inheritdoc/>
    public int Count => this._items.Count;

    /// <inheritdoc/>
    public InputProperty<TEntity> this[int index] => this._items[index];

    #endregion

    #region Constructors

    // TODO: I want to be able construct by passing in a list of internal inputs.
    //       This will likely require creating a IInputProperty (or IInputInteraction?) interface.

    /// <summary>
    /// Creates a new <see cref="InputPropertyList{TEntity}"/> with optional initial entities, item validation, and collection validation.
    /// </summary>
    /// <param name="initialEntitiesFunc">Function returning the default initial entities for the collection.</param>
    /// <param name="itemValidationFunc">Function to validate each individual item.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection. Defaults to checking that all items are valid.</param>
    /// <param name="onAddOnChange">Action to assign internal change handlers.</param>
    public InputPropertyList(
        Func<IEnumerable<TEntity>>? initialEntitiesFunc = null,
        Func<TEntity, bool>? itemValidationFunc = null,
        Func<IEnumerable<InputProperty<TEntity>>, bool>? collectionValidationFunc = null,
        Action<TEntity, Action?>? onAddOnChange = null)
    {
        this._defaultEntitiesFunc = initialEntitiesFunc ?? (() => []);

        this._itemValidationFunc = itemValidationFunc is null
            ? (_) => new ValidationResult(true)
            : (entity) => new ValidationResult(itemValidationFunc(entity));

        this._collectionValidationFunc = collectionValidationFunc is null
            ? (items) => new ValidationResult(items.All(item => item.IsValid()))
            : (items) => new ValidationResult(collectionValidationFunc(items));

        this._onAddOnChange = onAddOnChange is not null
            ? onAddOnChange
            : (_, _) => { };

        this.InitializeDefaultItems();
    }

    /// <summary>
    /// Creates a new <see cref="InputPropertyList{TEntity}"/> with optional initial entities, custom item <see cref="ValidationResult"/> validation, and custom collection <see cref="ValidationResult"/> validation.
    /// </summary>
    /// <param name="initialEntitiesFunc">Function returning the default initial entities for the collection.</param>
    /// <param name="itemValidationFunc">Function to validate each individual item with a custom <see cref="ValidationResult"/>.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection with a custom <see cref="ValidationResult"/>. Defaults to checking that all items are valid.</param>
    /// <param name="onAddOnChange">Action to assign internal change handlers.</param>
    public InputPropertyList(
        Func<IEnumerable<TEntity>>? initialEntitiesFunc,
        Func<TEntity, ValidationResult>? itemValidationFunc,
        Func<IEnumerable<InputProperty<TEntity>>, ValidationResult>? collectionValidationFunc = null,
        Action<TEntity, Action?>? onAddOnChange = null)
    {
        this._defaultEntitiesFunc = initialEntitiesFunc ?? (() => []);

        this._itemValidationFunc = itemValidationFunc ?? ((_) => new ValidationResult(true));

        this._collectionValidationFunc = collectionValidationFunc ?? ((items) => new ValidationResult(items.All(item => item.IsValid())));

        this._onAddOnChange = onAddOnChange is not null
            ? onAddOnChange
            : (_, _) => { };

        this.InitializeDefaultItems();
    }

    #endregion

    #region Public List Manipulation Methods

    /// <summary>
    /// Creates an <see cref="InputProperty{TEntity}"/> for the given entity, adds it to the collection, and wires change notifications.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The created <see cref="InputProperty{TEntity}"/>.</returns>
    public InputProperty<TEntity> Add(TEntity entity)
    {
        var _Property = this.CreateInputProperty(entity);
        this.Add(_Property);
        return _Property;
    }

    /// <summary>
    /// Adds an existing <see cref="InputProperty{TEntity}"/> to the collection and wires change notifications.
    /// </summary>
    /// <param name="property">The input property to add.</param>
    public void Add(InputProperty<TEntity> property)
    {
        this.HookItem(property);
        this._items.Add(property);
        this.OnItemChanged();
    }

    /// <summary>
    /// Adds a range of entities to the collection, creating a <see cref="InputProperty{TEntity}"/> for 
    /// each of the given entities, and wires change notifications.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    public void AddRange(IEnumerable<TEntity> entities)
    {
        foreach (var _Entity in entities)
        {
            var _Property = this.CreateInputProperty(_Entity);
            this.HookItem(_Property);
            this._items.Add(_Property);
        }
        this.OnItemChanged();
    }

    /// <summary>
    /// Adds a range of existing <see cref="InputProperty{TEntity}"/> items to the collection and wires change notifications.
    /// </summary>
    /// <param name="properties">The input properties to add.</param>
    public void AddRange(IEnumerable<InputProperty<TEntity>> properties)
    {
        foreach (var _Property in properties)
        {
            this.HookItem(_Property);
            this._items.Add(_Property);
        }
        this.OnItemChanged();
    }

    /// <summary>
    /// Clears all items from the collection and unhooks change notifications.
    /// </summary>
    public void Clear()
    {
        if (this._items.Count == 0)
            return;

        foreach (var _Property in this._items)
            this.UnhookItem(_Property);

        this._items.Clear();
        this.OnItemChanged();
    }

    /// <summary>
    /// Determines whether the collection contains the specified <see cref="InputProperty{TEntity}"/>.
    /// </summary>
    public bool Contains(InputProperty<TEntity> property)
        => this._items.Contains(property);

    /// <summary>
    /// Determines whether the collection contains an item whose <see cref="InputProperty{TEntity}.Input"/> matches the specified entity.
    /// </summary>
    public bool Contains(TEntity entity)
        => this.IndexOf(entity) >= 0;

    /// <inheritdoc/>
    public IEnumerator<InputProperty<TEntity>> GetEnumerator()
        => this._items.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    /// <summary>
    /// Returns the zero-based index of the first occurrence of the specified <see cref="InputProperty{TEntity}"/>.
    /// </summary>
    /// <returns>
    /// The zero-based index of the first occurrence of the item within the collection, if found; otherwise, -1.
    /// </returns>
    public int IndexOf(InputProperty<TEntity> property)
        => this._items.IndexOf(property);

    /// <summary>
    /// Returns the zero-based index of the first item whose <see cref="InputProperty{TEntity}.Input"/> matches the specified entity.
    /// </summary>
    /// <returns>
    /// The zero-based index of the first occurrence of the item within the collection, if found; otherwise, -1.
    /// </returns>
    public int IndexOf(TEntity entity)
    {
        var _Comparer = EqualityComparer<TEntity>.Default;
        for (int i = 0; i < this._items.Count; i++)
        {
            if (_Comparer.Equals(this._items[i].Input, entity))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Creates an <see cref="InputProperty{TEntity}"/> for the given entity, inserts it at the specified index, and wires change notifications.
    /// </summary>
    /// <param name="index">The zero-based index at which the entity should be inserted.</param>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The created <see cref="InputProperty{TEntity}"/>.</returns>
    public InputProperty<TEntity> Insert(int index, TEntity entity)
    {
        var _Property = this.CreateInputProperty(entity);
        this.Insert(index, _Property);
        return _Property;
    }

    /// <summary>
    /// Inserts an existing <see cref="InputProperty{TEntity}"/> at the specified index, and wires change notifications.
    /// </summary>
    /// <param name="index">The zero-based index at which the property should be inserted.</param>
    /// <param name="property">The input property to insert.</param>
    public void Insert(int index, InputProperty<TEntity> property)
    {
        this.HookItem(property);
        this._items.Insert(index, property);
        this.OnItemChanged();
    }

    // TODO: Method to allow movement up and down the list. (MoveLower, MoveHigher)
    //       This would also be useful in helping to reduce the number of events raised.
    /// <summary>
    /// Moves an item from one index to another within the collection.
    /// Will clamp to the bounds of the collection (0 -> Count).
    /// </summary>
    /// <param name="oldIndex">The zero-based index of the item to move.</param>
    /// <param name="newIndex">The zero-based destination index.</param>
    public void Move(int oldIndex, int newIndex)
    {
        if (this._items.Count <= 1)
            return;

        oldIndex = Math.Clamp(oldIndex, 0, this._items.Count - 1);
        newIndex = Math.Clamp(newIndex, 0, this._items.Count - 1);

        if (oldIndex == newIndex)
            return;

        var _Property = this._items[oldIndex];
        this._items.RemoveAt(oldIndex);
        this._items.Insert(newIndex, _Property);
        this.OnChange?.Invoke();
    }

    /// <summary>
    /// Removes the first occurrence of an item whose <see cref="InputProperty{TEntity}.Input"/> matches the specified 
    /// entity and unhooks change notifications.
    /// </summary>
    /// <remarks>
    /// While it is theoretically possible for there to be multiple <see cref="InputProperty{TEntity}"/>'s in this list 
    /// with the same entity, and therefore potentially gamble at which will be deleted, it is such a low chance that 
    /// this should not be considered a concern.
    /// </remarks>
    /// <param name="entity">The entity value to match and remove.</param>
    /// <returns>True if an item was found and removed; otherwise, false.</returns>
    public bool Remove(TEntity entity)
    {
        var _Index = this.IndexOf(entity);
        if (_Index >= 0)
        {
            this.RemoveAt(_Index);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the specified <see cref="InputProperty{TEntity}"/> from the collection and unhooks change notifications.
    /// </summary>
    /// <param name="property">The input property to remove.</param>
    /// <returns>True if the item was found and removed; otherwise, false.</returns>
    public bool Remove(InputProperty<TEntity> property)
    {
        if (this._items.Remove(property))
        {
            this.UnhookItem(property);
            this.OnItemChanged();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes the item at the specified index and unhooks change notifications.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    public void RemoveAt(int index)
    {
        var _Property = this._items[index];
        this._items.RemoveAt(index);
        this.UnhookItem(_Property);
        this.OnItemChanged();
    }

    #endregion

    #region Public Input Interaction Methods

    /// <summary>
    /// Uses the validation functions provided on construction to check whether all individual items and the collection as a whole are valid.
    /// </summary>
    /// <returns>True if all items and the collection validation succeed.</returns>
    public virtual bool IsValid()
    {
        bool allItemsValid = true;
        string? firstItemError = null;

        foreach (var item in this._items)
        {
            if (!item.IsValid())
            {
                allItemsValid = false;
                if (firstItemError is null && !string.IsNullOrEmpty(item.ErrorMessage))
                    firstItemError = item.ErrorMessage;
            }
        }

        var collectionResult = this._collectionValidationFunc.Invoke(this._items);
        if (!collectionResult.IsValid)
        {
            this.ErrorMessage = collectionResult.ErrorMessage ?? DEFAULT_ERROR_MESSAGE;
            return false;
        }

        if (!allItemsValid)
        {
            this.ErrorMessage = firstItemError ?? DEFAULT_ERROR_MESSAGE;
            return false;
        }

        this.ErrorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Resets the collection to the original default entities defined during construction.
    /// </summary>
    public virtual void ResetToDefault()
    {
        foreach (var item in this._items)
            this.UnhookItem(item);

        this.InitializeDefaultItems();
        this.OnItemChanged();
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Creates a new <see cref="InputProperty{TEntity}"/> for the given entity using the item validation 
    /// function provided during construction.
    /// </summary>
    /// <param name="entity">The entity for which to create an input property.</param>
    /// <returns>The new <see cref="InputProperty{TEntity}"/> containing the entity.</returns>
    private InputProperty<TEntity> CreateInputProperty(TEntity entity)
        => new(() => entity, this._itemValidationFunc);

    /// <summary>
    /// Wires the change notification of the given <see cref="InputProperty{TEntity}"/> to the 
    /// collection's <see cref="OnChange"/> event and invokes any additional actions provided during construction.
    /// </summary>
    /// <param name="property">The input to hook in.</param>
    private void HookItem(InputProperty<TEntity> property)
    {
        property.OnChange += this.OnItemChanged;
        this._onAddOnChange?.Invoke(property.Input, property.OnChange);
    }

    /// <summary>
    /// Clears the current items and initializes the collection with the default entities provided during construction, 
    /// creating <see cref="InputProperty{TEntity}"/> instances for each and wiring change notifications.
    /// </summary>
    private void InitializeDefaultItems()
    {
        this._items.Clear();
        var _DefaultEntities = this._defaultEntitiesFunc.Invoke();
        if (_DefaultEntities is not null)
        {
            foreach (var _Entity in _DefaultEntities)
            {
                var _Property = this.CreateInputProperty(_Entity);
                this.HookItem(_Property);
                this._items.Add(_Property);
            }
        }
    }

    /// <summary>
    /// Invokes the collection's <see cref="OnChange"/> event to notify subscribers that an item has changed.
    /// </summary>
    private void OnItemChanged()
        => this.OnChange?.Invoke();

    /// <summary>
    /// Unhooks the change notification of the given <see cref="InputProperty{TEntity}"/> from the 
    /// collection's <see cref="OnChange"/> event.
    /// </summary>
    /// <param name="property">The input property to unhook.</param>
    private void UnhookItem(InputProperty<TEntity> property)
        => property.OnChange -= this.OnItemChanged;

    #endregion

}
