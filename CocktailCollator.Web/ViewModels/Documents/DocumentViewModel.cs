using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.Documents;

public class DocumentViewModel : IStoreableViewModel<DocumentViewModel>
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

    public void ApplyChanges(DocumentViewModel source, IViewModelStore store)
    {
        this.FileName = source.FileName;
        this.Url = source.Url;
    }
}
