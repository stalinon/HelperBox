using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Конфигурация таблицы
/// </summary>
public class TableConfiguration<TEntity>
    where TEntity : Entity, new()
{
    private static readonly List<TableIndex<TEntity>> _indices = new();

    /// <summary>
    ///     Индексы таблицы
    /// </summary>
    internal TableIndex<TEntity>[] Indices => _indices.ToArray();

    /// <summary>
    ///     Конфигурировать БД
    /// </summary>
    internal static void Setup(ModelBuilder modelBuilder)
    {
        SetupIndices(modelBuilder);
    }

    /// <summary>
    ///     Добавить новый индекс
    /// </summary>
    public TableConfiguration<TEntity> AddIndex(
        Expression<Func<TEntity, object?>> propertySelector,
        bool isUnique = false)
    {
        var index = new TableIndex<TEntity>(propertySelector, isUnique);
        _indices.Add(index);

        return this;
    }

    private static void SetupIndices(ModelBuilder modelBuilder) 
        => _indices.ForEach(i => i.Setup(modelBuilder));
}