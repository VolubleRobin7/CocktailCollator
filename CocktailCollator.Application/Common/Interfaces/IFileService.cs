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
    /// <param name="fileData">
    /// The content of the file.
    /// </param>
    /// <param name="filePath">
    /// The path where the file should be saved. This includes the file name and extension.
    /// </param>
    Task SaveFileAsync(byte[] fileData, string filePath, CancellationToken cancellationToken);
}
