using Microsoft.Extensions.DependencyInjection;

namespace HelperBox.Database.Services.Impl;

/// <inheritdoc cref="IRepositoryContext" />
internal sealed class RepositoryContext : IRepositoryContext
{
    private readonly DatabaseContext _databaseContext;
    private readonly IServiceProvider _serviceProvider; 
    private bool _isDisposed;

    /// <inheritdoc cref="RepositoryContext" />
    public RepositoryContext(DatabaseContext databaseContext, IServiceProvider serviceProvider)
    {
        _databaseContext = databaseContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IRepository<TEntity> Repository<TEntity>() 
        where TEntity : Entity
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();

        return repository;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _databaseContext.SaveChangesAsync(cancellationToken);

        await(_databaseContext.Database.CurrentTransaction?.CommitAsync(cancellationToken) ?? Task.CompletedTask);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _databaseContext.Database.CurrentTransaction?.Commit();
        _databaseContext.Dispose();
        _isDisposed = true;
    }
}
