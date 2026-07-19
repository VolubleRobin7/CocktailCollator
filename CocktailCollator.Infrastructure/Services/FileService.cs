using CocktailCollator.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CocktailCollator.Infrastructure.Services;

public class FileService(IConfiguration configuration) : IFileService
{
    async Task IFileService.DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var _FileStorePath = configuration["FileStorePath"]; // I think I normally get this differently
        if (string.IsNullOrEmpty(_FileStorePath))
            throw new InvalidOperationException("FileStorePath is not configured.");

        var _TotalFilePath = Path.Combine(_FileStorePath, filePath);
        if (Path.Exists(_TotalFilePath))
            File.Delete(_TotalFilePath);

        var _FilePathDirectroy = Path.GetDirectoryName(_TotalFilePath);
        if (!string.IsNullOrEmpty(_FilePathDirectroy) && Directory.Exists(_FilePathDirectroy))
            Directory.Delete(_FilePathDirectroy, false);
    }

    async Task IFileService.SaveFileAsync(byte[] fileData, string filePath, CancellationToken cancellationToken)
    {
        var _FileStorePath = configuration["FileStorePath"]; // I think I normally get this differently
        if (string.IsNullOrEmpty(_FileStorePath))
            throw new InvalidOperationException("FileStorePath is not configured.");

        if (fileData is null || fileData.Length == 0)
            return;

        var _TotalFilePath = Path.Combine(_FileStorePath, filePath);

        var _TotalFilePathDirectory = Path.GetDirectoryName(_TotalFilePath);
        if (!string.IsNullOrEmpty(_TotalFilePathDirectory) && !Directory.Exists(_TotalFilePathDirectory))
            _ = Directory.CreateDirectory(_TotalFilePathDirectory);

        await File.WriteAllBytesAsync(_TotalFilePath, fileData, cancellationToken);
    }
}
