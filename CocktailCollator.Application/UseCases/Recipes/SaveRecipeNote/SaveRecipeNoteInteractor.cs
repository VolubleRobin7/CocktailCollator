using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Recipes.SaveRecipeNote;

public class SaveRecipeNoteInteractor(ICocktailDbContext dbContext)
{
    public async Task InteractAsync(SaveRecipeNoteInputPort inputPort, ISaveRecipeNoteOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _RecipeNote = dbContext.GetEntities<RecipeNote>()
            .FirstOrDefault(rn => rn.RecipeId == inputPort.RecipeId && rn.UserId == inputPort.UserId);

        if (_RecipeNote is null)
        {
            _RecipeNote = new RecipeNote
            {
                RecipeId = inputPort.RecipeId,
                UserId = inputPort.UserId,
                Note = inputPort.Note
            };
            dbContext.Add(_RecipeNote);
        }
        else
        {
            _RecipeNote.Note = inputPort.Note;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_RecipeNote, cancellationToken);
    }
}
