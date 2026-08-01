using Microsoft.AspNetCore.Http;

namespace CocktailCollator.Application.Common.Interfaces;

public interface ICocktailDbContext
{
    // Database Methods
    void Add<TEntity>(TEntity entity) where TEntity : class;

    IQueryable<TEntity> GetEntities<TEntity>() where TEntity : class;

    void Remove<TEntity>(TEntity entity) where TEntity : class;

    Task SaveChangesAsync(CancellationToken cancellationToken);

    // File Storage Methods
    Guid QueueAddDocument<TEntity>(IFormFile fileData, TEntity relatedEntity) where TEntity : class;

    void QueueRemoveDocument(Guid documentId);
}
