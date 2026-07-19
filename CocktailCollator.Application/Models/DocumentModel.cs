namespace CocktailCollator.Application.Models;

public class DocumentModel // This should probably be replaced by IFormFile
{
    public required byte[] Data { get; set; }
    public required string FileName { get; set; }
}