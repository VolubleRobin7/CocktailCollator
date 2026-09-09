using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeAuthoriser(IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> innerPipe, AuthenticationStateProvider authenticationStateProvider)
    : IAuthorisationPipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
