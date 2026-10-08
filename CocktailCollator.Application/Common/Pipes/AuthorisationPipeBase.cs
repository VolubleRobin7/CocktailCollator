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
    public virtual Task<bool> ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
        => AuthorisationPipeBaseHelper.AuthoriseAsync(authenticationStateProvider, authorizationService, policyName, outputPort, cancellationToken);
}

public abstract class AuthorisationPipeBase<TOutputPort>(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorizationService,
    string policyName)
    : IAuthorisationPipe<EmptyInputPort<TOutputPort>, TOutputPort>
    where TOutputPort : IAuthorisableOutputPort
{
    public virtual Task<bool> ExecuteAsync(TOutputPort outputPort, CancellationToken cancellationToken)
        => AuthorisationPipeBaseHelper.AuthoriseAsync(authenticationStateProvider, authorizationService, policyName, outputPort, cancellationToken);

    // Explicit interface implementation to satisfy the pipeline engine's 2-generic-parameter contract.
    // Consumers will use ExecuteAsync() above.
    Task<bool> IPipe<EmptyInputPort<TOutputPort>, TOutputPort>.ExecuteAsync(
        EmptyInputPort<TOutputPort> inputPort,
        TOutputPort outputPort,
        CancellationToken cancellationToken)
        => this.ExecuteAsync(outputPort, cancellationToken);
}

file static class AuthorisationPipeBaseHelper
{
    public static async Task<bool> AuthoriseAsync(
        AuthenticationStateProvider authenticationStateProvider,
        IAuthorizationService authorizationService,
        string policyName,
        IAuthorisableOutputPort outputPort,
        CancellationToken cancellationToken)
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
