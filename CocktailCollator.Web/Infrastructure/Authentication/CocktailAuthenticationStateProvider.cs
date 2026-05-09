using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace CocktailCollator.Web.Infrastructure.Authentication;

public class CocktailAuthenticationStateProvider(
    ILoggerFactory loggerFactory)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(3);

    protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // this needs to be dealt with
        return Task.FromResult(true);
    }
}
