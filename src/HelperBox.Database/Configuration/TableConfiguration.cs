using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Конфигурация таблицы
/// </summary>
public class TableConfiguration<TEntity>
    where TEntity : BaseEntity, new()
{
    private readonly List<TableIndex<TEntity>> _indices = new();

    /// <summary>
    ///     Индексы таблицы
    /// </summary>
    public TableIndex<TEntity>[] Indices => _indices.ToArray();

    /// <summary>
    ///     Добавить новый индекс
    /// </summary>
    public TableConfiguration<TEntity> AddIndex(Expression<Func<TEntity, object?>> propertySelector, bool isUnique = false)
    {
        var index = new TableIndex<TEntity>(propertySelector, isUnique);
        _indices.Add(index);

        return this;
    }

    private TableConfiguration<TEntity> SetupIndices(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TEntity>();
        _indices.ForEach(i => entity.HasIndex(i.PropertySelector).HasDatabaseName(i.ToString()).IsUnique(i.IsUnique));

        return this;
    }
}