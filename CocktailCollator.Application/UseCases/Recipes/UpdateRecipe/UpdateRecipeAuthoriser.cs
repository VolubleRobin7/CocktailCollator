using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : IAuthorisationPipe<UpdateRecipeInputPort, IUpdateRecipeOutputPort>
{
    public async Task<bool> ExecuteAsync(UpdateRecipeInputPort inputPort, IUpdateRecipeOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _UserIdClaim = _AuthState.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        _ = Guid.TryParse(_UserIdClaim, out var _CurrentUserId);

        var _AuthResult = await authorisationService.AuthorizeAsync(_AuthState.User, Policies.ManageRecipes);

        inputPort.PipelineContext = new UpdateRecipePipelineContext(_CurrentUserId, _AuthResult.Succeeded);

        if (!_AuthResult.Succeeded && inputPort.PersonalNote is null)
        {
            await outputPort.Unauthorised(cancellationToken);
            return false;
        }

        return true;
    }
}
