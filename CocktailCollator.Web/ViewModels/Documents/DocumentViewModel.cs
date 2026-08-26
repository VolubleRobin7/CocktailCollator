using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.ViewModels.Documents;

public class DocumentViewModel
{
    public required Guid DocumentId { get; set; }
    public required string FileName { get; set; }
    public required string Url { get; set; }

    public DocumentInputProperty AsInputProperty()
        => new()
        {
            Existing = new()
            {
                Id = this.DocumentId,
                FileName = this.FileName,
                Url = this.Url,
            }
        };

    public ExistingDocument AsExistingDocument()
        => new()
        {
            Id = this.DocumentId,
            FileName = this.FileName,
            Url = this.Url,
        };
}
