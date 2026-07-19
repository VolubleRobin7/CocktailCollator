using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Infrastructure.Options;
using CocktailCollator.Infrastructure.Persistence;
using CocktailCollator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CocktailCollator.Infrastructure;

public static class DependencyInjector
{
    public static IServiceCollection InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .Configure<FileStorageOptions>(configuration)
            .AddScoped<ICocktailDbContext, CocktailDbContext>()
            .AddScoped<IFileService, FileService>();
}
