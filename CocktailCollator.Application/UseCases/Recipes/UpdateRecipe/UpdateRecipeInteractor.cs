using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeInteractor(ICocktailDbContext dbContext)
    : IInteractorPipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public async Task ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Recipe = dbContext.GetEntities<Recipe>().First(r => r.RecipeId == inputPort.RecipeId);

        if (inputPort.PipelineContext.HasAllPermissions)
        {
            // Details
            _Recipe.Name = inputPort.Name;
            _Recipe.RecipeCategoryId = inputPort.RecipeCategoryId;
            _Recipe.Note = inputPort.Note;

            // Steps
            _Recipe.Steps = [.. inputPort.Steps.Select(s => new RecipeStep
            {
                Instruction = s.Instruction,
                Order = s.Order
            })];

            // Ingredients
            // This is a very inefficient way to update the recipe ingredients
            var _RecipeIngredients = dbContext.GetEntities<RecipeIngredient>().Where(ri => ri.RecipeId == inputPort.RecipeId);
            foreach (var recipeIngredient in _RecipeIngredients)
                dbContext.Remove(recipeIngredient);

            var newRecipeIngredients = new List<RecipeIngredient>();
            foreach (var recipeIngredient in inputPort.Ingredients)
            {
                if (recipeIngredient.Ingredient is not null)
                {
                    var newRecipeIngredient = new RecipeIngredient
                    {
                        Ingredient = new Ingredient
                        {
                            Name = recipeIngredient.Ingredient.Name,
                            Measurements = [new() { MeasurementId = recipeIngredient.MeasurementId }],
                        },
                        MeasurementId = recipeIngredient.MeasurementId,
                        Amount = recipeIngredient.Amount
                    };
                    newRecipeIngredients.Add(newRecipeIngredient);
                }
                else
                {
                    var newRecipeIngredient = new RecipeIngredient
                    {
                        RecipeId = _Recipe.RecipeId,
                        IngredientId = recipeIngredient.IngredientId,
                        MeasurementId = recipeIngredient.MeasurementId,
                        Amount = recipeIngredient.Amount
                    };
                    newRecipeIngredients.Add(newRecipeIngredient);
                }
            }
            _Recipe.Ingredients = newRecipeIngredients;

            // Images
            var _DocumentsToKeep = inputPort.Images
                .Where(i => i.ExistingDocumentId.HasValue)
                .Select(i => i.ExistingDocumentId!.Value);

            var _DocumentsToRemove = dbContext.GetEntities<RecipeDocument>()
                .Where(rd => rd.RecipeId == inputPort.RecipeId && !_DocumentsToKeep.Contains(rd.DocumentId));

            foreach (var _RecipeDocument in _DocumentsToRemove)
            {
                dbContext.Remove(_RecipeDocument);
                dbContext.QueueRemoveDocument(_RecipeDocument.DocumentId);
            }

            var _NewDocuments = inputPort.Images
                .Where(i => i.NewDocument is not null)
                .Select(i => i.NewDocument!);

            foreach (var _NewDocument in _NewDocuments)
            {
                var _NewDocumentId = dbContext.QueueAddDocument(_NewDocument, _Recipe);
                _Recipe.Images ??= [];
                _Recipe.Images.Add(new RecipeDocument
                {
                    RecipeId = _Recipe.RecipeId,
                    DocumentId = _NewDocumentId,
                });
            }
        }

        // Personal Note
        if (inputPort.PipelineContext.UserId != Guid.Empty)
        {
            var _ExistingNote = dbContext.GetEntities<RecipeNote>()
                    .FirstOrDefault(rn => rn.RecipeId == _Recipe.RecipeId && rn.UserId == inputPort.PipelineContext.UserId);

            if (inputPort.PersonalNote is not null)
            {
                if (string.IsNullOrWhiteSpace(inputPort.PersonalNote))
                {
                    if (_ExistingNote is not null)
                        dbContext.Remove(_ExistingNote);

                    _Recipe.PersonalRecipeNotes = [];
                }
                else
                {
                    if (_ExistingNote is not null)
                    {
                        _ExistingNote.Note = inputPort.PersonalNote;
                        _Recipe.PersonalRecipeNotes = [_ExistingNote];
                    }
                    else
                    {
                        var _NewNote = new RecipeNote
                        {
                            UserId = inputPort.PipelineContext.UserId,
                            RecipeId = _Recipe.RecipeId,
                            Note = inputPort.PersonalNote
                        };
                        dbContext.Add(_NewNote);
                        _Recipe.PersonalRecipeNotes = [_NewNote];
                    }
                }
            }
            else
            {
                // Even if the personal note is not being changed (null), we still want to load the
                // current note for the user so that it can be returned in the output port.
                if (_ExistingNote is not null)
                    _Recipe.PersonalRecipeNotes = [_ExistingNote];
            }
        }

        // Finalisation
        await dbContext.SaveChangesAsync(cancellationToken);
        if (inputPort.PipelineContext.HasAllPermissions)
            await outputPort.Success(_Recipe, cancellationToken);
        else
            await outputPort.PersonalNoteUpdated(_Recipe, cancellationToken);
    }
}
