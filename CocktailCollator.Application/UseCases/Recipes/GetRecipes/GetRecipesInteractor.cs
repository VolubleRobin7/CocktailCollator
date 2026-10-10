using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public class GetRecipesInteractor(
    AuthenticationStateProvider authenticationStateProvider,
    ICocktailDbContext dbContext)
    : IInteractorPipe<IGetRecipesOutputPort>
{
    public async Task ExecuteAsync(IGetRecipesOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _CurrentUserId = await GetUserId(authenticationStateProvider);

        var _Query = dbContext.GetEntities<Recipe>()
            .Select(r => new
            {
                Recipe = r,
                r.Category,
                Ingredients = r.Ingredients!.Select(ri => new
                {
                    RecipeIngredient = ri,
                    ri.Ingredient,
                    ri.Measurement
                }),
                r.Steps,
                Images = r.Images!.Select(ri => new
                {
                    RecipeDocument = ri,
                    ri.Document
                }),
                RecipeNotes = r.PersonalRecipeNotes!.Where(rn => _CurrentUserId.HasValue && rn.UserId == _CurrentUserId.Value)
            });

        var _Recipes = _Query.AsEnumerable().Select(x =>
        {
            x.Recipe.PersonalRecipeNotes = [.. x.RecipeNotes];
            return x.Recipe;
        }).ToList();

        await outputPort.Success(_Recipes, cancellationToken);
    }

    private static async Task<Guid?> GetUserId(AuthenticationStateProvider authProvider)
    {
        var _AuthState = await authProvider.GetAuthenticationStateAsync();
        Guid? _UserId = null;
        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
        {
            var _UserIdClaim = _AuthState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(_UserIdClaim, out var _ParsedId))
                _UserId = _ParsedId;
        }
        return _UserId;
    }
}
