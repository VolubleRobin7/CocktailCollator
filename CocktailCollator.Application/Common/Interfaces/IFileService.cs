using Microsoft.AspNetCore.Http;

namespace CocktailCollator.Application.Common.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Saves a file to the specified file path.
    /// </summary>
    /// <param name="filePath">
    /// The path where the file should be saved. This includes the file name and extension.
    /// </param>
    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);

    /// <summary>
    /// Saves a file to the specified file path.
    /// </summary>
    /// <param name="file">
    /// The file to save.
    /// </param>
    /// <param name="filePath">
    /// The path where the file should be saved. This includes the file name and extension.
    /// </param>
    Task SaveFileAsync(IFormFile file, string filePath, CancellationToken cancellationToken);
}
