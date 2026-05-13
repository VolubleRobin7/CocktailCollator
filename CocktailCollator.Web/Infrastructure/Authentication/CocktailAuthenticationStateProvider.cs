using CocktailCollator.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Web.Infrastructure.Authentication;

public class CocktailAuthenticationStateProvider(
    ILoggerFactory loggerFactory,
    IServiceScopeFactory scopeFactory)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(1);

    // this check needs to be confirmed, just forcing it to return false is not good enough, needs to be a true false result
    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        var user = authenticationState.User;

        if (user is null || user.Identity is null || !user.Identity.IsAuthenticated)
            return false;

        await using var scope = scopeFactory.CreateAsyncScope();
        var signInManager = scope.ServiceProvider
            .GetRequiredService<SignInManager<CocktailUser>>();

        var validatedUser = await signInManager.ValidateSecurityStampAsync(user);
        return validatedUser is not null;
    }
}
