using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.GetRecipes;

public class GetRecipesAuthoriser(IPipeline<GetRecipesInputPort, IGetRecipesOutputPort> innerPipe, AuthenticationStateProvider authenticationStateProvider)
    : IAuthorisationPipe<GetRecipesInputPort, IGetRecipesOutputPort>
{
    public IPipeline<GetRecipesInputPort, IGetRecipesOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(GetRecipesInputPort inputPort, IGetRecipesOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
