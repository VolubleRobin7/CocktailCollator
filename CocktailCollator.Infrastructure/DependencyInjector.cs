using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailCollator.Infrastructure;

public static class DependencyInjector
{
    public static IServiceCollection InjectInfrastructure(this IServiceCollection services)
        => services
            .AddScoped<ICocktailDbContext, CocktailDbContext>();
}
