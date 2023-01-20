using HelperBox.Database.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace HelperBox.Database.Services.Impl;

/// <inheritdoc cref="IRepository{TEntity}" />
public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : Entity
{
    protected virtual DatabaseContext DbContext { get; }

    protected readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Название таблицы
    /// </summary>
    protected static readonly string? TableName = typeof(TEntity).GetCustomAttributes(true)
        .OfType<TableAttribute>()
        .FirstOrDefault()?.Name;

    /// <inheritdoc cref="Repository{T}"/>
    protected Repository(DatabaseContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    /// <inheritdoc />
    public virtual IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>>? filter = null) => filter == null
        ? _dbSet.AsQueryable()
        : _dbSet.AsQueryable().Where(filter);

    /// <inheritdoc />
    public virtual async Task<TEntity[]> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? sort = null,
        SortType sortDirection = SortType.Ascending,
        CancellationToken cancellationToken = default)
    {
        var query = CreateQuery(filter);
        if (sort != null)
        {
            query = sortDirection switch
            {
                SortType.Ascending => query.OrderBy(sort),
                SortType.Descending => query.OrderByDescending(sort),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return await query.ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) =>
        await CreateQuery(filter).FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public virtual void Add(TEntity entity) => _dbSet.Add(entity);

    /// <inheritdoc />
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    /// <inheritdoc />
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
        await _dbSet.AddRangeAsync(entities, cancellationToken);

    /// <inheritdoc />
    public virtual void Remove(TEntity entity) => _dbSet.Remove(entity);

    /// <inheritdoc />
    public virtual void RemoveRange(TEntity[] entities) => _dbSet.RemoveRange(entities);

    /// <inheritdoc />
    public virtual IQueryable<TEntity> NoTracking() =>
        _dbSet.AsNoTracking();

    /// <inheritdoc />
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await DbContext.SaveChangesAsync(cancellationToken);
}
