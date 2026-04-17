namespace CocktailCollator.Web.Common.Generics;

public class InputProperty<TEntity>
{
    private readonly Func<TEntity> _defaultEntityFunc;
    private TEntity _entity;
    private readonly Func<TEntity, bool> _validationFunc;

    /// <summary>
    /// The actual value of the input.
    /// </summary>
    public TEntity Input
    {
        get { return this._entity; }
        set 
        { 
            this._entity = value; 
            this.OnChange?.Invoke();
        }
    }

    /// <summary>
    /// An Action that is invoked whenever <see cref="Input"/> is changed.
    /// </summary>
    public Action? OnChange { get; set; }

    /// <summary>
    /// Create a new input, with an initial value and the function required to check its validity.
    /// </summary>
    /// <param name="initialEntityFunc">The function to run to retrieve the initial value for the input.</param>
    /// <param name="validationFunc">The function to run that will check if the input is valid.</param>
    public InputProperty(Func<TEntity> initialEntityFunc, Func<TEntity, bool> validationFunc)
    {
        this._defaultEntityFunc = initialEntityFunc;
        this._entity = initialEntityFunc.Invoke();
        this._validationFunc = validationFunc;
    }

    /// <summary>
    /// Uses the validation function provided on construction to check whether the input is valid.
    /// </summary>
    /// <returns>True if valid.</returns>
    public bool IsValid()
        => this._validationFunc.Invoke(this._entity);

    /// <summary>
    /// Resets the Input to the original value defined during construction.
    /// </summary>
    public void ResetToDefault()
    {
        this._entity = this._defaultEntityFunc.Invoke();
        this.OnChange?.Invoke();
    }
}
