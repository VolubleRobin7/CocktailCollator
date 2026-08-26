using System.Collections;

namespace CocktailCollator.Web.Common.Inputs;

public abstract class InputPropertyListBase<TEntity, TProperty>
    : IList<TEntity>, IReadOnlyList<TEntity>, IInputInteraction
    where TProperty : InputProperty<TEntity>
{

    #region Fields

    protected const string DEFAULT_ERROR_MESSAGE = "Please review your input list.";

    protected readonly Func<IEnumerable<TProperty>, ValidationResult> _collectionValidationFunc;
    protected readonly List<TProperty> _items = [];

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
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public virtual TEntity this[int index]
    {
        get => this._items[index].Input;
        set => this._items[index].Input = value;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InputPropertyListBase{TEntity, TProperty}"/> class with an optional collection validation function.
    /// </summary>
    /// <param name="collectionValidationFunc">Function to validate the entire collection with a custom <see cref="ValidationResult"/>.</param>
    protected InputPropertyListBase(Func<IEnumerable<TProperty>, ValidationResult>? collectionValidationFunc = null)
    {
        this._collectionValidationFunc = collectionValidationFunc
            ?? ((items) => new ValidationResult(items.All(item => item.IsValid())));
    }

    #endregion

    #region Public List Manipulation Methods

    /// <summary>
    /// Creates a <typeparamref name="TProperty"/> for the given entity, adds it to the collection, and wires change notifications.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public virtual void Add(TEntity entity)
    {
        var _Property = this.CreateInputProperty(entity);
        this.HookItem(_Property);
        this._items.Add(_Property);
        this.OnItemChanged();
    }

    /// <summary>
    /// Adds a range of entities to the collection, creating a <typeparamref name="TProperty"/> for 
    /// each of the given entities, and wires change notifications.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    public virtual void AddRange(IEnumerable<TEntity> entities)
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
    /// Clears all items from the collection and unhooks change notifications.
    /// </summary>
    public virtual void Clear()
    {
        if (this._items.Count == 0)
            return;

        foreach (var _Property in this._items)
            this.UnhookItem(_Property);

        this._items.Clear();
        this.OnItemChanged();
    }

    /// <inheritdoc/>
    public virtual void CopyTo(TEntity[] array, int arrayIndex)
    {
        for (int i = 0; i < this._items.Count; i++)
            array[arrayIndex + i] = this._items[i].Input;
    }

    /// <summary>
    /// Determines whether the collection contains an item whose <typeparamref name="TProperty"/>.Input matches the specified entity.
    /// </summary>
    public virtual bool Contains(TEntity entity)
        => this.IndexOf(entity) >= 0;

    /// <inheritdoc/>
    public virtual IEnumerator<TEntity> GetEnumerator()
        => this._items.Select(item => item.Input).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    /// <summary>
    /// Returns the zero-based index of the first item whose <typeparamref name="TProperty"/>.Input matches the specified entity.
    /// </summary>
    /// <returns>
    /// The zero-based index of the first occurrence of the item within the collection, if found; otherwise, -1.
    /// </returns>
    public virtual int IndexOf(TEntity entity)
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
    /// Creates a <typeparamref name="TProperty"/> for the given entity, inserts it at the specified index, and wires change notifications.
    /// </summary>
    /// <param name="index">The zero-based index at which the entity should be inserted.</param>
    /// <param name="entity">The entity to insert.</param> 
    public virtual void Insert(int index, TEntity entity)
    {
        var _Property = this.CreateInputProperty(entity);
        this.HookItem(_Property);
        this._items.Insert(index, _Property);
        this.OnItemChanged();
    }

    /// <summary>
    /// Moves an item from one index to another within the collection.
    /// Will clamp to the bounds of the collection (0 -> Count).
    /// </summary>
    /// <param name="oldIndex">The zero-based index of the item to move.</param>
    /// <param name="newIndex">The zero-based destination index.</param>
    public virtual void Move(int oldIndex, int newIndex)
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
        this.OnItemChanged();
    }

    /// <summary>
    /// Moves the first occurrence of an entity up the list by the given amount, if it exists in the collection.
    /// Will clamp to the bounds of the collection (0 -> Count).
    /// </summary>
    /// <param name="entity">The entity to match and move.</param>
    /// <param name="moveAmount">The number of positions to move the entity.</param>
    /// <remarks>
    /// A positive <paramref name="moveAmount"/> moves the entity away from index 0, 
    /// while a negative <paramref name="moveAmount"/> moves it towards index 0.
    /// </remarks>
    public virtual void Move(TEntity entity, int moveAmount = 1)
    {
        var _Index = this.IndexOf(entity);
        if (_Index >= 0)
            this.Move(_Index, _Index + moveAmount);
    }

    /// <summary>
    /// Removes the first occurrence of an item whose <typeparamref name="TProperty"/>.Input matches the specified 
    /// entity and unhooks change notifications.
    /// </summary>
    /// <remarks>
    /// It is possible for there to be multiple <typeparamref name="TProperty"/>'s in this list with the same reference, 
    /// especially if <typeparamref name="TEntity"/> is nullable. Therefore it is potentially a gamble at which will 
    /// be deleted. It is recommended to use <see cref="Remove(TProperty)"/> instead.
    /// </remarks>
    /// <param name="entity">The entity value to match and remove.</param>
    /// <returns>True if an item was found and removed; otherwise, false.</returns>
    public virtual bool Remove(TEntity entity)
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
    /// Removes the first occurrence of the specified <typeparamref name="TProperty"/> and unhooks change notifications.
    /// </summary>
    /// <param name="property">The input property to match and remove.</param>
    /// <returns>True if an item was found and removed; otherwise, false.</returns>
    public virtual bool Remove(TProperty property)
    {
        var _Result = this._items.Remove(property);
        if (_Result)
        {
            this.UnhookItem(property);
            this.OnItemChanged();
        }
        return _Result;
    }

    /// <summary>
    /// Removes the item at the specified index and unhooks change notifications.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    public virtual void RemoveAt(int index)
    {
        var _Property = this._items[index];
        this._items.RemoveAt(index);
        this.UnhookItem(_Property);
        this.OnItemChanged();
    }

    #endregion

    #region Public Input Interaction Methods

    /// <summary>
    /// The items in the list as <typeparamref name="TProperty"/>'s.
    /// </summary>
    /// <returns>A read-only list of <typeparamref name="TProperty"/> items.</returns>
    public virtual IReadOnlyList<TProperty> AsInputs()
        => this._items.AsReadOnly();

    /// <summary>
    /// Returns the <typeparamref name="TProperty"/> for the first item that matches the specified entity.
    /// </summary>
    /// <param name="entity">The entity to look for.</param>
    /// <returns>The matching <typeparamref name="TProperty"/>, or null if not found.</returns>
    public virtual TProperty? InputFor(TEntity entity)
        => this.InputFor(this.IndexOf(entity));

    /// <summary>
    /// Returns the <typeparamref name="TProperty"/> for the first item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to retrieve.</param>
    /// <returns>The <typeparamref name="TProperty"/> at the specified index, or null if the index is out of bounds.</returns>
    public virtual TProperty? InputFor(int index)
        => index >= 0 && index < this._items.Count
            ? this._items[index]
            : null;

    /// <summary>
    /// Uses the validation functions provided on construction to check whether the collection is valid.
    /// </summary>
    /// <returns>True if the collection validation succeeds.</returns>
    public virtual bool IsValid()
    {
        var _CollectionResult = this._collectionValidationFunc.Invoke(this._items);
        if (!_CollectionResult.IsValid)
        {
            this.ErrorMessage = _CollectionResult.ErrorMessage ?? DEFAULT_ERROR_MESSAGE;
            return false;
        }

        this.ErrorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Resets the collection to the original default entities defined during construction.
    /// </summary>
    public virtual void ResetToDefault()
        => this.Clear();

    #endregion

    #region Protected Helper Methods

    /// <summary>
    /// Creates a new <typeparamref name="TProperty"/> for the given entity.
    /// </summary>
    /// <param name="entity">The entity for which to create an input property.</param>
    /// <returns>The new <typeparamref name="TProperty"/> containing the entity.</returns>
    protected abstract TProperty CreateInputProperty(TEntity entity);

    /// <summary>
    /// Wires the change notification of the given <typeparamref name="TProperty"/> to the 
    /// collection's <see cref="OnChange"/> event.
    /// </summary>
    /// <param name="property">The input to hook in.</param>
    protected virtual void HookItem(TProperty property)
        => property.OnChange += this.OnItemChanged;

    /// <summary>
    /// Invokes the collection's <see cref="OnChange"/> event to notify subscribers that an item has changed.
    /// </summary>
    protected virtual void OnItemChanged()
        => this.OnChange?.Invoke();

    /// <summary>
    /// Unhooks the change notification of the given <typeparamref name="TProperty"/> from the 
    /// collection's <see cref="OnChange"/> event.
    /// </summary>
    /// <param name="property">The input property to unhook.</param>
    protected virtual void UnhookItem(TProperty property)
        => property.OnChange -= this.OnItemChanged;

    #endregion

}
