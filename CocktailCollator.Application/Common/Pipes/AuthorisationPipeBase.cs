using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.Common.Pipes;

public abstract class AuthorisationPipeBase<TInputPort, TOutputPort>(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorizationService,
    string policyName)
    : IAuthorisationPipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IAuthorisableOutputPort
{
    public virtual async Task<bool> ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _AuthResult = await authorizationService.AuthorizeAsync(_AuthState.User, policyName);

        if (_AuthResult.Succeeded)
            return true;
        else
        {
            await outputPort.Unauthorised(cancellationToken);
            return false;
        }
    }
}
