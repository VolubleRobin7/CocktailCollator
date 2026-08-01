using Microsoft.AspNetCore.Http;

namespace CocktailCollator.Application.Models;

public class DocumentModel
{
    public Guid? ExistingDocumentId { get; set; }
    public IFormFile? NewDocument { get; set; }
}