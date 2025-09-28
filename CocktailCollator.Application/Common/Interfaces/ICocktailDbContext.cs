namespace CocktailCollator.Application.Common.Interfaces;

public interface ICocktailDbContext
{
    void Add<TEntity>(TEntity entity) where TEntity : class;

    IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class;

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
