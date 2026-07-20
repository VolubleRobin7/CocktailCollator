using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using Microsoft.AspNetCore.Http;
using CocktailCollator.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CocktailCollator.Infrastructure.Persistence;

public class CocktailDbContext(DbContextOptions<CocktailDbContext> options, IFileService fileService) : IdentityDbContext<CocktailUser, CocktailRole, Guid>(options), ICocktailDbContext
{
    private readonly List<DocumentInfo> _queuedDocumentsToAdd = [];
    private readonly List<Guid> _queuedDocumentsToRemove = [];

    void ICocktailDbContext.Add<TEntity>(TEntity entity)
        => this.Add(entity);

    IQueryable<TEntity> ICocktailDbContext.GetEntities<TEntity>()
        => this.Set<TEntity>();

    void ICocktailDbContext.Remove<TEntity>(TEntity entity)
        => this.Remove(entity);

    async Task ICocktailDbContext.SaveChangesAsync(CancellationToken cancellationToken)
    {
        // No need to use try/catch here, as the transaction will automatically roll back if an exception occurs.
        using var transaction = await this.Database.BeginTransactionAsync(cancellationToken);

        // Remove the records of the documents from the database, do not delete the files yet, in case the database save fails.
        var _FilesToRemove = new List<string>();
        if (this._queuedDocumentsToRemove.Count > 0)
        {
            foreach (var documentId in this._queuedDocumentsToRemove)
                _FilesToRemove.Add(await this.RemoveDocumentRecordAsync(documentId, cancellationToken));

            this._queuedDocumentsToRemove.Clear();
        }

        _ = await this.SaveChangesAsync(cancellationToken);

        // Add the new documents to the database and save them to the file system.
        if (this._queuedDocumentsToAdd.Count > 0)
        {
            foreach (var _DocumentInfo in this._queuedDocumentsToAdd)
                await AddDocumentAsync(_DocumentInfo, cancellationToken);

            this._queuedDocumentsToAdd.Clear();

            _ = await this.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        // Delete the files from the file system after the database save has succeeded.
        if (_FilesToRemove.Count > 0)
        {
            foreach (var _FilePath in _FilesToRemove)
                await fileService.DeleteFileAsync(_FilePath, cancellationToken);
        }
    }

    Guid ICocktailDbContext.QueueAddDocument<TEntity>(IFormFile file, TEntity relatedEntity, CancellationToken cancellationToken)
    {
        var _NewDocument = new Document
        {
            FilePath = "", // This will be filled in during SaveChangesAsync()
            OriginalFileName = file.FileName
        };
        _ = this.Add(_NewDocument);
        this._queuedDocumentsToAdd.Add(new(file, _NewDocument, () => this.GenerateUniqueDirectoryPath(relatedEntity)));

        return _NewDocument.DocumentId;
    }

    void ICocktailDbContext.QueueRemoveDocument(Guid documentId)
    {
        if (!this._queuedDocumentsToRemove.Exists(d => d == documentId))
            this._queuedDocumentsToRemove.Add(documentId);
    }

    private async Task AddDocumentAsync(DocumentInfo documentInfo, CancellationToken cancellationToken)
    {
        var _UniqueFileName = documentInfo.Entity.DocumentId.ToString() + Path.GetExtension(documentInfo.Entity.OriginalFileName);
        var _FilePath = Path.Combine(documentInfo.DirectoryPath(), _UniqueFileName);

        await fileService.SaveFileAsync(documentInfo.File, _FilePath, cancellationToken);

        documentInfo.Entity.FilePath = _FilePath;
    }

    private string GenerateUniqueDirectoryPath<TEntity>(TEntity relatedEntity)
    {
        var _RelatedEntityDatabaseEntry = this.Entry(relatedEntity);
        var _RelatedEntityTypeName = _RelatedEntityDatabaseEntry.Metadata.ClrType.Name;
        var _RelatedEntityId = _RelatedEntityDatabaseEntry.CurrentValues
            .GetValue<Guid>(_RelatedEntityDatabaseEntry.Metadata.FindPrimaryKey().Properties.Single())
            .ToString();

        if (_RelatedEntityId == Guid.Empty.ToString())
            throw new InvalidOperationException("The related entity must exist in the database before saving the document.");

        return Path.Combine(_RelatedEntityTypeName, _RelatedEntityId);
    }

    private async Task<string> RemoveDocumentRecordAsync(Guid documentId, CancellationToken cancellationToken)
    {
        var _Document = await this.Set<Document>().FirstAsync(d => d.DocumentId == documentId, cancellationToken)
            ?? throw new InvalidOperationException($"Document with ID {documentId} not found.");

        _ = this.Remove(_Document);
        return _Document.FilePath;
    }

    private record struct DocumentInfo(IFormFile File, Document Entity, Func<string> DirectoryPath);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        AddEntities(modelBuilder);
        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(CocktailDbContext).Assembly);
    }

    // Create Migration Command
    // dotnet ef migrations add NAME --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web
    //
    // Apply Migration Command (May not be needed, based on RunMigrationsOnStartup in appsettings)
    // dotnet ef database update --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web
    //
    // Revert Applied Migration Command (Will revert to having NAME as the latest applied migration)
    // dotnet ef database update NAME --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web
    //
    // Remove Previous Migrations Command (Can only do after reverting the applied migrations, or if it hasn't been applied yet)
    // dotnet ef migrations remove --project CocktailCollator.Infrastructure --startup-project CocktailCollator.Web

    private static void AddEntities(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Recipe>();
        _ = modelBuilder.Entity<Ingredient>();
        _ = modelBuilder.Entity<RecipeStep>();
        _ = modelBuilder.Entity<RecipeIngredient>();
        _ = modelBuilder.Entity<Measurement>();
        _ = modelBuilder.Entity<IngredientMeasurement>();
        _ = modelBuilder.Entity<IngredientCategory>();
        _ = modelBuilder.Entity<RecipeCategory>();
        _ = modelBuilder.Entity<Document>();
        _ = modelBuilder.Entity<RecipeDocument>();
    }
}
