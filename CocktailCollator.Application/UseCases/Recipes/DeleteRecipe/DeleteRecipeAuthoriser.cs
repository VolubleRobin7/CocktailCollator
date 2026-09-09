using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeAuthoriser(IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> innerPipe, AuthenticationStateProvider authenticationStateProvider)
    : IAuthorisationPipe<DeleteRecipeInputPort, IDeleteRecipeOutputPort>
{
    public IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(DeleteRecipeInputPort inputPort, IDeleteRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
