using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.Common.Pipes;

public class AuthenticationPipe<TInputPort, TOutputPort>(IPipeline<TInputPort, TOutputPort> innerPipe, AuthenticationStateProvider authenticationStateProvider)
    : IAuthenticationPipe<TInputPort, TOutputPort> where TInputPort : IInputPort<TOutputPort> where TOutputPort : IAuthenticatableOutputPort
{
    public IPipeline<TInputPort, TOutputPort> InnerPipe => innerPipe;

    public async Task ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            await innerPipe.ExecuteAsync(inputPort, outputPort, cancellationToken);
        else
            await outputPort.Unauthenticated(cancellationToken);
    }
}