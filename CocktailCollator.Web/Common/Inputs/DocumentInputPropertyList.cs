using Microsoft.AspNetCore.Components.Forms;

namespace CocktailCollator.Web.Common.Inputs;

public class DocumentInputPropertyList : InputPropertyListBase<IBrowserFile?, DocumentInputProperty>
{

    #region Fields

    private readonly bool _isRequired = false;
    private readonly bool _isOnlyValidIfUploaded = false;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new <see cref="DocumentInputPropertyList"/> with the specified requirements and an optional validation function.
    /// </summary>
    /// <param name="isRequired">What to set for the <see cref="DocumentInputPropertyList"/> isRequired construction parameter.</param>
    /// <param name="isOnlyValidIfUploaded">What to set for the <see cref="DocumentInputPropertyList"/> isOnlyValidIfUploaded construction parameter.</param>
    /// <param name="collectionValidationFunc">Function to validate the entire collection with a custom <see cref="ValidationResult"/>.</param>
    /// <remarks>
    /// Defaults to the collection being valid if all items are valid.
    /// Note that by default this does skip reading the error messages from the internal inputs.
    /// </remarks>
    public DocumentInputPropertyList(
        bool isRequired = false,
        bool isOnlyValidIfUploaded = true,
        Func<IEnumerable<DocumentInputProperty>, ValidationResult>? collectionValidationFunc = null)
        : base(collectionValidationFunc)
    {
        this._isRequired = isRequired;
        this._isOnlyValidIfUploaded = isOnlyValidIfUploaded;
    }

    #endregion

    #region Public List Manipulation Methods

    /// <summary>
    /// Creates an <see cref="DocumentInputProperty"/> for the given <see cref="ExistingDocument"/>, adds it to the collection, and wires change notifications.
    /// </summary>
    /// <param name="document">The document to add.</param>
    public void Add(ExistingDocument document)
    {
        var _Property = this.CreateInputProperty(document);
        this.HookItem(_Property);
        this._items.Add(_Property);
        this.OnItemChanged();
    }

    /// <summary>
    /// Adds a range of documents to the collection, creating a <see cref="DocumentInputProperty"/> for 
    /// each of the given documents, and wires change notifications.
    /// </summary>
    /// <param name="documents">The documents to add.</param>
    public void AddRange(IEnumerable<ExistingDocument> documents)
    {
        foreach (var _Document in documents)
        {
            var _Property = this.CreateInputProperty(_Document);
            this.HookItem(_Property);
            this._items.Add(_Property);
        }
        this.OnItemChanged();
    }

    /// <summary>
    /// Clears all items from the collection that do not have existing documents and unhooks change notifications.
    /// </summary>
    /// <remarks>
    /// This will clear any <see cref="DocumentInputProperty"/>'s that have null <see cref="ExistingDocument"/>, 
    /// therefore only clearing new inputs and bypassing overriding inputs.
    /// </remarks>
    /// <param name="resetExisting">Reset overriden documents back to their original existing documents.</param>
    public void ClearNew(bool resetExisting = false)
    {
        if (this._items.Count == 0)
            return;

        foreach (var _Property in this._items.Where(property => property.Existing is null).ToList()) // Must be a list to avoid modifying the collection while iterating
            _ = this._items.Remove(_Property);

        if (resetExisting)
        {
            foreach (var _Property in this._items.Where(property => property.Existing is not null && property.Input is not null))
                _Property.Input = null;
        }

        this.OnItemChanged();
    }

    #endregion

    #region Public Input Interaction Methods

    /// <summary>
    /// Uploads all <see cref="IBrowserFile"/>'s in the collection that are not already existing documents with nothing to override them.
    /// </summary>
    /// <returns>True if all uploads succeed.</returns>
    public async Task<bool> UploadAllAsync()
        => (await Task.WhenAll(this._items.Where(property => !property.IsExisting()).Select(property => property.UploadFileAsync())))
            .All(result => result);

    #endregion

    #region Protected Helper Methods

    /// <inheritdoc/>
    protected override DocumentInputProperty CreateInputProperty(IBrowserFile? file)
        => new(isRequired: this._isRequired, isOnlyValidIfUploaded: this._isOnlyValidIfUploaded) { Input = file };

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Creates a new <see cref="DocumentInputProperty"/> for the given <see cref="ExistingDocument"/>.
    /// </summary>
    /// <param name="document">The document for which to create an document input property.</param>
    /// <returns>The new <see cref="DocumentInputProperty"/> containing the file.</returns>
    private DocumentInputProperty CreateInputProperty(ExistingDocument document)
        => new(isRequired: this._isRequired, isOnlyValidIfUploaded: this._isOnlyValidIfUploaded) { Existing = document };

    #endregion

}
