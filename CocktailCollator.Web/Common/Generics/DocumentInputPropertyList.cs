using Microsoft.AspNetCore.Components.Forms;
using System.Collections;

namespace CocktailCollator.Web.Common.Generics;

public class DocumentInputPropertyList : IList<IBrowserFile?>, IInputInteraction
{

    #region Fields

    private const string DEFAULT_ERROR_MESSAGE = "Please review your input list.";

    private readonly Func<IEnumerable<DocumentInputProperty>, ValidationResult> _collectionValidationFunc;
    private readonly bool _isRequired = false;
    private readonly bool _isOnlyValidIfUploaded = false;
    private readonly List<DocumentInputProperty> _items = [];

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
    public IBrowserFile? this[int index]
    {
        get => this._items[index].Input;
        set => this._items[index].Input = value;
    }

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
    {
        this._isRequired = isRequired;
        this._isOnlyValidIfUploaded = isOnlyValidIfUploaded;

        this._collectionValidationFunc = collectionValidationFunc
            ?? ((items) => new ValidationResult(items.All(item => item.IsValid())));
    }

    #endregion

    #region Public List Manipulation Methods

    /// <summary>
    /// Creates an <see cref="DocumentInputProperty"/> for the given <see cref="IBrowserFile"/>, adds it to the collection, and wires change notifications.
    /// </summary>
    /// <param name="file">The file to add.</param>
    public void Add(IBrowserFile? file)
    {
        var _Property = this.CreateInputProperty(file);
        this.HookItem(_Property);
        this._items.Add(_Property);
        this.OnItemChanged();
    }

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
    /// Adds a range of files to the collection, creating a <see cref="DocumentInputProperty"/> for 
    /// each of the given files, and wires change notifications.
    /// </summary>
    /// <param name="files">The files to add.</param>
    public void AddRange(IEnumerable<IBrowserFile?> files)
    {
        foreach (var _File in files)
        {
            var _Property = this.CreateInputProperty(_File);
            this.HookItem(_Property);
            this._items.Add(_Property);
        }
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

    /// <<summary>
    /// Clears all items from the collection and unhooks change notifications.
    /// </summary>>
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

    /// <inheritdoc/>
    public void CopyTo(IBrowserFile?[] array, int arrayIndex)
    {
        for (int i = 0; i < this._items.Count; i++)
            array[arrayIndex + i] = this._items[i].Input;
    }

    /// <summary>
    /// Determines whether the collection contains an item whose <see cref="DocumentInputProperty.Input"/> matches the specified file.
    /// </summary>
    public bool Contains(IBrowserFile? file)
        => this.IndexOf(file) >= 0;

    /// <inheritdoc/>
    public IEnumerator<IBrowserFile?> GetEnumerator()
        => this._items.Select(item => item.Input).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    /// <summary>
    /// Returns the zero-based index of the first item whose <see cref="DocumentInputProperty.Input"/> matches the specified file.
    /// </summary>
    /// <returns>
    /// The zero-based index of the first occurrence of the item within the collection, if found; otherwise, -1.
    /// </returns>
    public int IndexOf(IBrowserFile? entity)
    {
        var _Comparer = EqualityComparer<IBrowserFile?>.Default;
        for (int i = 0; i < this._items.Count; i++)
        {
            if (_Comparer.Equals(this._items[i].Input, entity))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Creates an <see cref="DocumentInputProperty"/> for the given <see cref="IBrowserFile"/>, inserts it at the specified index, and wires change notifications.
    /// </summary>
    /// <param name="index">The zero-based index at which the file should be inserted.</param>
    /// <param name="file">The file to insert.</param>
    public void Insert(int index, IBrowserFile? file)
    {
        var _Property = this.CreateInputProperty(file);
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
        this.OnItemChanged();
    }

    /// <summary>
    /// Moves the first occurrence of a file up the list by the given amount, if it exists in the collection.
    /// Will clamp to the bounds of the collection (0 -> Count).
    /// </summary>
    /// <param name="file">The file to match and move.</param>
    /// <param name="moveAmount">The number of positions to move the file.</param>
    /// <remarks>
    /// A positive <paramref name="moveAmount"/> moves the file away from index 0, 
    /// while a negative <paramref name="moveAmount"/> moves it towards index 0.
    /// </remarks>
    public void Move(IBrowserFile? file, int moveAmount = 1)
    {
        var _Index = this.IndexOf(file);
        if (_Index >= 0)
            this.Move(_Index, _Index + moveAmount);
    }

    /// <summary>
    /// Removes the first occurrence of an item whose <see cref="DocumentInputProperty.Input"/> matches the specified 
    /// entity and unhooks change notifications.
    /// </summary>
    /// <remarks>
    /// It is possible for there to be multiple <see cref="DocumentInputProperty"/>'s in this list 
    /// with the same file, especially since it may be null. This will then likely delete the first 
    /// occurence of the file, rather than the one specified.
    /// </remarks>
    /// <param name="file">The file to match and remove.</param>
    /// <returns>True if an item was found and removed; otherwise, false.</returns>
    public bool Remove(IBrowserFile? file)
    {
        var _Index = this.IndexOf(file);
        if (_Index >= 0)
        {
            this.RemoveAt(_Index);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the first occurrence of the specified <see cref="DocumentInputProperty.Input"/> and unhooks change notifications.
    /// </summary>
    /// <param name="property">The input property to match and remove.</param>
    /// <returns>True if an item was found and removed; otherwise, false.</returns>
    public bool Remove(DocumentInputProperty property)
    {
        var _Result = this._items.Remove(property);
        this.UnhookItem(property);
        this.OnItemChanged();
        return _Result;
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
    /// The items in the list as <see cref="DocumentInputProperty"/>'s.
    /// </summary>
    /// <returns>A read-only list of <see cref="DocumentInputProperty"/> items.</returns>
    public IReadOnlyList<DocumentInputProperty> AsInputs()
        => this._items.AsReadOnly();

    /// <summary>
    /// Returns the <see cref="DocumentInputProperty"/> for the first item that matches the specified file.
    /// </summary>
    /// <param name="file">The file to look for.</param>
    /// <returns>The matching <see cref="DocumentInputProperty"/>, or null if not found.</returns>
    public DocumentInputProperty? InputFor(IBrowserFile? file)
        => this.InputFor(this.IndexOf(file));

    /// <summary>
    /// Returns the <see cref="DocumentInputProperty"/> for the first item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to retrieve.</param>
    /// <returns>The <see cref="DocumentInputProperty"/> at the specified index, or null if the index is out of bounds.</returns>
    public DocumentInputProperty? InputFor(int index)
        => index >= 0 && index < this._items.Count
            ? this._items[index]
            : null;

    /// <summary>
    /// Uses the collection validation function provided on construction to check whether the collection is valid.
    /// </summary>
    /// <returns>True if the collection validation succeeds.</returns>
    public bool IsValid()
    {
        var collectionResult = this._collectionValidationFunc.Invoke(this._items);
        if (!collectionResult.IsValid)
        {
            this.ErrorMessage = collectionResult.ErrorMessage ?? DEFAULT_ERROR_MESSAGE;
            return false;
        }

        this.ErrorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Resets the collection to the original default entities defined during construction.
    /// </summary>
    public void ResetToDefault()
        => this.Clear();

    /// <summary>
    /// Uploads all <see cref="IBrowserFile"/>'s in the collection that are not already existing documents with nothing to override them.
    /// </summary>
    /// <returns>True if all uploads succeed.</returns>
    public async Task<bool> UploadAllAsync()
        => (await Task.WhenAll(this._items.Where(property => !property.IsExisting()).Select(property => property.UploadFileAsync())))
            .All(result => result);

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Creates a new <see cref="DocumentInputProperty"/> for the given <see cref="IBrowserFile"/>.
    /// </summary>
    /// <param name="file">The file for which to create an document input property.</param>
    /// <returns>The new <see cref="DocumentInputProperty"/> containing the file.</returns>
    private DocumentInputProperty CreateInputProperty(IBrowserFile? file)
        => new(isRequired: this._isRequired, isOnlyValidIfUploaded: this._isOnlyValidIfUploaded) { Input = file };

    /// <summary>
    /// Creates a new <see cref="DocumentInputProperty"/> for the given <see cref="ExistingDocument"/>.
    /// </summary>
    /// <param name="document">The document for which to create an document input property.</param>
    /// <returns>The new <see cref="DocumentInputProperty"/> containing the file.</returns>
    private DocumentInputProperty CreateInputProperty(ExistingDocument document)
        => new(isRequired: this._isRequired, isOnlyValidIfUploaded: this._isOnlyValidIfUploaded) { Existing = document };

    /// <summary>
    /// Wires the change notification of the given <see cref="DocumentInputProperty"/> to the 
    /// collection's <see cref="OnChange"/> event.
    /// </summary>
    /// <param name="property">The input to hook in.</param>
    private void HookItem(DocumentInputProperty property)
        => property.OnChange += this.OnItemChanged;

    /// <summary>
    /// Invokes the collection's <see cref="OnChange"/> event to notify subscribers that an item has changed.
    /// </summary>
    private void OnItemChanged()
        => this.OnChange?.Invoke();

    /// <summary>
    /// Unhooks the change notification of the given <see cref="DocumentInputProperty"/> from the 
    /// collection's <see cref="OnChange"/> event.
    /// </summary>
    /// <param name="property">The document input property to unhook.</param>
    private void UnhookItem(DocumentInputProperty property)
        => property.OnChange -= this.OnItemChanged;

    #endregion

}
