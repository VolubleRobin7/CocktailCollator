using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.Common.Pipes;

public abstract class AuthorisationPipeBase<TInputPort, TOutputPort>(
    IPipeline<TInputPort, TOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorizationService,
    string policyName)
    : IAuthorisationPipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IAuthorisableOutputPort
{
    public IPipeline<TInputPort, TOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var _AuthResult = await authorizationService.AuthorizeAsync(_AuthState.User, policyName);

        if (_AuthResult.Succeeded)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthorised(cancellationToken);
    }
}
