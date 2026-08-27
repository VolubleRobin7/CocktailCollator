namespace CocktailCollator.Web.Common.Inputs;

public class InputPropertyList<TEntity> : InputPropertyListBase<TEntity, InputProperty<TEntity>>
{

    #region Fields

    private readonly Func<IEnumerable<TEntity>> _defaultEntitiesFunc;
    private readonly Func<TEntity, ValidationResult> _itemValidationFunc;
    private readonly Action<TEntity, Action?> _onAddOnChange;

    #endregion

    #region List Manipulation Properties

    /// <inheritdoc/>
    public override TEntity this[int index]
    {
        get => base[index];
        set
        {
            base[index] = value;
            this._onAddOnChange?.Invoke(value, this._items[index].OnChange);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="InputPropertyList{TEntity}"/> with optional initial entities, custom item <see cref="ValidationResult"/> validation, and custom collection <see cref="ValidationResult"/> validation.
    /// </summary>
    /// <param name="internalInputsFunc">An array of functions returning an <see cref="IInputInteraction"/> for each entity.</param>
    /// <param name="initialEntitiesFunc">Function returning the default initial entities for the collection.</param>
    /// <param name="itemValidationFunc">Function to validate each individual item with a custom <see cref="ValidationResult"/>.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection with a custom <see cref="ValidationResult"/>.</param>
    /// <remarks>
    /// Defaults to an empty list, each item being valid if all their internal inputs are valid, and collection valid if all items are valid.
    /// Note that by default this does skip reading the error messages from the internal inputs.
    /// </remarks>
    public InputPropertyList(
        Func<TEntity, IInputInteraction>[] internalInputsFunc,
        Func<IEnumerable<TEntity>>? initialEntitiesFunc = null,
        Func<TEntity, ValidationResult>? itemValidationFunc = null,
        Func<IEnumerable<InputProperty<TEntity>>, ValidationResult>? collectionValidationFunc = null)
        : base(collectionValidationFunc)
    {
        this._defaultEntitiesFunc = initialEntitiesFunc ?? (() => []);

        this._itemValidationFunc = itemValidationFunc
            ?? ((item) => new ValidationResult(internalInputsFunc.All(inputFunc => inputFunc(item).IsValid())));

        this._onAddOnChange = (item, onChange) =>
        {
            foreach (var inputFunc in internalInputsFunc)
                inputFunc(item).OnChange = () => onChange?.Invoke();
        };

        this.InitializeDefaultItems();
    }

    /// <summary>
    /// Creates a new <see cref="InputPropertyList{TEntity}"/> with optional initial entities, item validation, and collection validation.
    /// </summary>
    /// <param name="initialEntitiesFunc">Function returning the default initial entities for the collection.</param>
    /// <param name="itemValidationFunc">Function to validate each individual item.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection.</param>
    /// <param name="onAddOnChange">Action to assign internal change handlers.</param>
    /// <remarks>
    /// Defaults to an empty list, each item always valid, and collection valid if all items are valid.
    /// Note that by default this does skip reading the error messages from the internal inputs.
    /// </remarks>
    public InputPropertyList(
        Func<IEnumerable<TEntity>>? initialEntitiesFunc = null,
        Func<TEntity, bool>? itemValidationFunc = null,
        Func<IEnumerable<InputProperty<TEntity>>, bool>? collectionValidationFunc = null,
        Action<TEntity, Action?>? onAddOnChange = null)
        : base(collectionValidationFunc is null ? null : (items) => new ValidationResult(collectionValidationFunc(items)))
    {
        this._defaultEntitiesFunc = initialEntitiesFunc ?? (() => []);

        this._itemValidationFunc = itemValidationFunc is null
            ? (_) => new ValidationResult(true)
            : (entity) => new ValidationResult(itemValidationFunc(entity));

        this._onAddOnChange = onAddOnChange ?? ((_, _) => { });

        this.InitializeDefaultItems();
    }

    /// <summary>
    /// Creates a new <see cref="InputPropertyList{TEntity}"/> with optional initial entities, custom item <see cref="ValidationResult"/> validation, and custom collection <see cref="ValidationResult"/> validation.
    /// </summary>
    /// <param name="initialEntitiesFunc">Function returning the default initial entities for the collection.</param>
    /// <param name="itemValidationFunc">Function to validate each individual item with a custom <see cref="ValidationResult"/>.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection with a custom <see cref="ValidationResult"/>.</param>
    /// <param name="onAddOnChange">Action to assign internal change handlers.</param>
    /// <remarks>
    /// Defaults to an empty list, each item always valid, and collection valid if all items are valid.
    /// Note that by default this does skip reading the error messages from the internal inputs.
    /// </remarks>
    public InputPropertyList(
        Func<IEnumerable<TEntity>>? initialEntitiesFunc,
        Func<TEntity, ValidationResult>? itemValidationFunc,
        Func<IEnumerable<InputProperty<TEntity>>, ValidationResult>? collectionValidationFunc = null,
        Action<TEntity, Action?>? onAddOnChange = null)
        : base(collectionValidationFunc)
    {
        this._defaultEntitiesFunc = initialEntitiesFunc ?? (() => []);

        this._itemValidationFunc = itemValidationFunc ?? ((_) => new ValidationResult(true));

        this._onAddOnChange = onAddOnChange ?? ((_, _) => { });

        this.InitializeDefaultItems();
    }

    #endregion

    #region Public Input Interaction Methods

    /// <summary>
    /// Resets the collection to the original default entities defined during construction.
    /// </summary>
    public override void ResetToDefault()
    {
        foreach (var item in this._items)
            this.UnhookItem(item);

        this.InitializeDefaultItems();
        this.OnItemChanged();
    }

    #endregion

    #region Protected Helper Methods

    /// <inheritdoc/>
    protected override InputProperty<TEntity> CreateInputProperty(TEntity entity)
        => new(() => entity, this._itemValidationFunc);

    /// <inheritdoc/>
    protected override void HookItem(InputProperty<TEntity> property)
    {
        base.HookItem(property);
        this._onAddOnChange?.Invoke(property.Input, property.OnChange);
    }

    #endregion

    #region Private Helper Methods

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

    #endregion

}
