using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeAuthoriser(
    IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : IAuthorisationPipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _AuthResult = await authorisationService.AuthorizeAsync(_AuthState.User, "ManageRecipes");

        if (_AuthResult.Succeeded)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
