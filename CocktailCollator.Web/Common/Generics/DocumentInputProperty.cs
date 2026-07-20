using Microsoft.AspNetCore.Components.Forms;

namespace CocktailCollator.Web.Common.Generics;

public class DocumentInputProperty(bool isRequired = false)
    : InputProperty<(IBrowserFile? File, string? ExceptionMessage)>(() => (null, null), (input) => IsDocumentValid(input.File, isRequired, input.ExceptionMessage))
{
    private const int MAX_ALLOWED_SIZE_MB = 5;
    private const int MAX_ALLOWED_SIZE = MAX_ALLOWED_SIZE_MB * 1024 * 1024;

    public byte[] Data { get; private set; } = [];
    public string FileName { get; private set; } = string.Empty;

    // Intentionally hiding the base Input property to prevent external edits.
    /// <summary>
    /// The IBrowserFile that was uploaded.
    /// </summary>
    /// <remarks>
    /// This property should generally not be accessed, instead use <see cref="UploadFileAsync(IBrowserFile?)"/> to input a new file.
    /// </remarks>
    public new IBrowserFile? Input => base.Input.File;

    private static ValidationResult IsDocumentValid(IBrowserFile? file, bool isRequired, string? exceptionMessage)
    {
        if (file is null)
            return isRequired ? new ValidationResult(false, "No file has been uploaded.") : new ValidationResult(true);

        if (file.Size == 0)
            return new ValidationResult(false, "Uploaded file is empty.");

        if (file.Size > MAX_ALLOWED_SIZE)
            return new ValidationResult(false, $"File exceeds the maximum allowed size of {MAX_ALLOWED_SIZE_MB} MB.");

        if (!string.IsNullOrEmpty(exceptionMessage))
            return new ValidationResult(false, $"File upload failed: {exceptionMessage}");

        return new ValidationResult(true);
    }

    /// <summary>
    /// Uploads a file and parses the data if successful.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>
    /// True if the upload was successful and the file is valid, false otherwise.
    /// </returns>
    public async Task<bool> UploadFileAsync(IBrowserFile? file)
    {
        base.Input = (file, null);

        if (base.Input.File is not null && base.Input.File.Size <= MAX_ALLOWED_SIZE)
        {
            try
            {
                using var _Stream = new MemoryStream();
                await base.Input.File.OpenReadStream(MAX_ALLOWED_SIZE).CopyToAsync(_Stream);

                this.Data = _Stream.ToArray();
                this.FileName = base.Input.File.Name;
            }
            catch (Exception exception)
            {
                base.Input = (null, $"File upload failed: {exception.Message}");
            }
        }

        this.OnChange?.Invoke();
        // Set the correct ErrorMessage
        return this.IsValid();
    }
}
