using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeAuthoriser(
    IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : IAuthorisationPipe<DeleteRecipeInputPort, IDeleteRecipeOutputPort>
{
    public IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(DeleteRecipeInputPort inputPort, IDeleteRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _AuthResult = await authorisationService.AuthorizeAsync(_AuthState.User, "ManageRecipes");

        if (_AuthResult.Succeeded)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
