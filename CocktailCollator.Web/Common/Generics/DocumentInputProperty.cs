using Microsoft.AspNetCore.Components.Forms;

namespace CocktailCollator.Web.Common.Generics;

public class DocumentInputProperty(bool isRequired = false, bool isOnlyValidIfUploaded = true)
    : InputProperty<IBrowserFile?>(() => null, (input) => false)
{
    private const int MAX_ALLOWED_SIZE_MB = 5;
    private const int MAX_ALLOWED_SIZE = MAX_ALLOWED_SIZE_MB * 1024 * 1024;

    private string? _exceptionMessage;

    public ExistingDocument? Existing { get; set; }
    public IFormFile? Output { get; private set; }

    /// <summary>
    /// Confirms whether the input document is valid and uploaded successfully.
    /// </summary>
    /// <returns>True if valid.</returns>
    public override bool IsValid()
    {
        var _ValidationResult = this.IsDocumentValid();
        this.ErrorMessage = _ValidationResult.IsValid ? "" : _ValidationResult.ErrorMessage ?? "";
        return _ValidationResult.IsValid;
    }

    /// <summary>
    /// Clear the Input and Output.
    /// </summary>
    public override void ResetToDefault()
    {
        this.Output = null;
        this._exceptionMessage = null;
        base.ResetToDefault();
    }

    private ValidationResult IsDocumentValid()
    {
        if (this.Input is null)
            return isRequired ? new ValidationResult(false, "No file has been uploaded.") : new ValidationResult(true);

        if (this.Input.Size == 0)
            return new ValidationResult(false, "Uploaded file is empty.");

        if (this.Input.Size > MAX_ALLOWED_SIZE)
            return new ValidationResult(false, $"File exceeds the maximum allowed size of {MAX_ALLOWED_SIZE_MB} MB.");

        if (isOnlyValidIfUploaded && !string.IsNullOrEmpty(this._exceptionMessage))
            return new ValidationResult(false, $"File upload failed: {this._exceptionMessage}");

        return new ValidationResult(true);
    }

    /// <summary>
    /// Whether this input document is an existing document in the filesystem and that nothing is input to override it.
    /// </summary>
    /// <returns>
    /// True if existing and unedited, false if either not existing or going to be overridden.
    /// </returns>
    public bool IsExisting()
        => this.Existing is not null && this.Input is null;

    /// <summary>
    /// Uploads the Input file and parses the data into Output if successful.
    /// </summary>
    /// <returns>
    /// True if the upload was successful and the file is valid, false otherwise.
    /// </returns>
    public async Task<bool> UploadFileAsync()
    {
        this.Output = null;

        if (this.Input is not null && this.Input.Size <= MAX_ALLOWED_SIZE)
        {
            try
            {
                var _Stream = new MemoryStream();
                await this.Input.OpenReadStream(MAX_ALLOWED_SIZE).CopyToAsync(_Stream);

                _Stream.Position = 0;
                this.Output = new FormFile(_Stream, 0, _Stream.Length, this.Input.Name, this.Input.Name);

                this._exceptionMessage = null;
            }
            catch (Exception exception)
            {
                this._exceptionMessage = exception.Message;
            }
        }

        this.OnChange?.Invoke();
        // Set the correct ErrorMessage
        return this.IsValid();
    }
}

// This is theoretically just a copy of DocumentViewModel, but separated so that a generic does not rely on a specific VM.
public class ExistingDocument
{
    public string? FileName { get; set; }
    public required Guid Id { get; set; }
    public string? Url { get; set; } // This should be how to access the existing file.
}
