using CocktailCollator.Web.Common.Generics;

namespace CocktailCollator.Web.ViewModels.Documents;

public class DocumentViewModel
{
    public required Guid DocumentId { get; set; }
    public required string Url { get; set; }

    public DocumentInputProperty AsInputProperty()
        => new()
        {
            ExistingDocumentId = this.DocumentId,
            ExistingDocumentUrl = this.Url,
        };
}
