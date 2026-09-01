using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.RecipeNotes;

public class RecipeNoteViewModel : IStoreableViewModel<RecipeNoteViewModel>
{
    public required Guid RecipeNoteId { get; set; }
    public Guid RecipeId { get; set; }
    public Guid UserId { get; set; }
    public string? Note { get; set; }

    public void ApplyChanges(RecipeNoteViewModel source, IViewModelStore store)
    {
        this.RecipeId = source.RecipeId;
        this.UserId = source.UserId;
        this.Note = source.Note;
    }
}
