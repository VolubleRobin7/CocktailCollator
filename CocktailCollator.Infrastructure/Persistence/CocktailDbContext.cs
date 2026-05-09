using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CocktailCollator.Infrastructure.Persistence;

public class CocktailDbContext(DbContextOptions<CocktailDbContext> options) : IdentityDbContext<CocktailUser, CocktailRole, Guid>(options), ICocktailDbContext
{
    void ICocktailDbContext.Add<TEntity>(TEntity entity)
        => this.Add(entity);

    IQueryable<TEntity> ICocktailDbContext.GetEntities<TEntity>()
        => this.Set<TEntity>();

    void ICocktailDbContext.Remove<TEntity>(TEntity entity)
        => this.Remove(entity);

    Task ICocktailDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        => this.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        AddEntities(modelBuilder);
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(CocktailDbContext).Assembly);
    }

    // Create Migration Command
    // dotnet ef migrations add NAME --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web
    //
    // Remove Previous Migration Command (Can only do before being applied I think)
    // dotnet ef migrations remove --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web
    //
    // Apply Migration Command
    // dotnet ef database update --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web

    private static void AddEntities(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Recipe>();
        _ = modelBuilder.Entity<Ingredient>();
        _ = modelBuilder.Entity<RecipeStep>();
        _ = modelBuilder.Entity<RecipeIngredient>();
        _ = modelBuilder.Entity<Measurement>();
        _ = modelBuilder.Entity<IngredientMeasurement>();
        _ = modelBuilder.Entity<IngredientCategory>();
    }
}
