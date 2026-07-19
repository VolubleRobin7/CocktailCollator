using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace CocktailCollator.Infrastructure.Services;

public class FileService(IOptions<FileStorageOptions> fileStorageOptions) : IFileService
{
    async Task IFileService.DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var _TotalFilePath = Path.Combine(fileStorageOptions.Value.FileStorePath, filePath);
        if (Path.Exists(_TotalFilePath))
            File.Delete(_TotalFilePath);

        var _FilePathDirectroy = Path.GetDirectoryName(_TotalFilePath);
        if (!string.IsNullOrEmpty(_FilePathDirectroy) && Directory.Exists(_FilePathDirectroy))
            Directory.Delete(_FilePathDirectroy, false);
    }

    async Task IFileService.SaveFileAsync(byte[] fileData, string filePath, CancellationToken cancellationToken)
    {
        if (fileData is null || fileData.Length == 0)
            return;

        var _TotalFilePath = Path.Combine(fileStorageOptions.Value.FileStorePath, filePath);

        var _TotalFilePathDirectory = Path.GetDirectoryName(_TotalFilePath);
        if (!string.IsNullOrEmpty(_TotalFilePathDirectory) && !Directory.Exists(_TotalFilePathDirectory))
            _ = Directory.CreateDirectory(_TotalFilePathDirectory);

        await File.WriteAllBytesAsync(_TotalFilePath, fileData, cancellationToken);
    }
}
