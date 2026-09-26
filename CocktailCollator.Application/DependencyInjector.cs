using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.UseCasePipelines;
using CocktailCollator.UseCasePipelines.OutputPorts;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailCollator.Application;

public static class DependencyInjector
{
    public static IServiceCollection InjectApplication(this IServiceCollection services)
        => services
            .AddUseCasePipelines(
            [
                new(typeof(IAuthenticatableOutputPort), typeof(IAuthenticationPipe<,>), typeof(AuthenticationPipe<,>)),
                new(typeof(IAuthorisableOutputPort), typeof(IAuthorisationPipe<,>)),
                new(typeof(IExistenceOutputPort), typeof(IExistencePipe<,>))
            ], typeof(DependencyInjector).Assembly);
}

