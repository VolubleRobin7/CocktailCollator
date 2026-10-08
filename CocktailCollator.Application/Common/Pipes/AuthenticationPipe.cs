using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.Common.Pipes;

public class AuthenticationPipe<TInputPort, TOutputPort>(AuthenticationStateProvider authenticationStateProvider)
    : IAuthenticationPipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IAuthenticatableOutputPort
{
    public async Task<bool> ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _AuthState = await authenticationStateProvider.GetAuthenticationStateAsync();

        if (_AuthState.User.Identity?.IsAuthenticated ?? false)
            return true;
        else
        {
            await outputPort.Unauthenticated(cancellationToken);
            return false;
        }
    }
}