using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeAuthoriser(IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> innerPipe, AuthenticationStateProvider authenticationStateProvider)
    : IAuthorisationPipe<CreateRecipeInputPort, ICreateRecipeOutputPort>
{
    public IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(CreateRecipeInputPort inputPort, ICreateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
