using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeAuthoriser(
    IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : IAuthorisationPipe<CreateRecipeInputPort, ICreateRecipeOutputPort>
{
    public IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(CreateRecipeInputPort inputPort, ICreateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _AuthResult = await authorisationService.AuthorizeAsync(_AuthState.User, "ManageRecipes");

        if (_AuthResult.Succeeded)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
