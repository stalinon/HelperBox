using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Конфигурация таблицы
/// </summary>
public class TableConfiguration<TEntity>
    where TEntity : Entity, new()
{
    #region Fields

    private static readonly List<IConfigItem> _configItems = new();

    #endregion

    #region Methods

    /// <summary>
    ///     Конфигурировать БД
    /// </summary>
    internal static void Setup(ModelBuilder modelBuilder) => _configItems.ForEach(i => i.Setup(modelBuilder));

    /// <summary>
    ///     Добавить новый индекс
    /// </summary>
    public TableConfiguration<TEntity> AddIndex(
        Expression<Func<TEntity, object?>> propertySelector,
        bool isUnique = false)
    {
        var index = new TableIndex<TEntity>(propertySelector, isUnique);
        _configItems.Add(index);

        return this;
    }

    /// <summary>
    ///     Добавить новую связь один-к-одному
    /// </summary>
    public TableConfiguration<TEntity> AddConnection<TSecondEntity>(
        Expression<Func<TEntity, TSecondEntity?>> hasOneSelector,
        Expression<Func<TSecondEntity, TEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
        where TSecondEntity : Entity, new()
    {
        var connection = TableConnection<TEntity, TSecondEntity>.CreateOneToOneConnection(
            hasOneSelector, withOneSelector, foreignKeySelector, isRequired);
        _configItems.Add(connection);

        return this;
    }

    /// <summary>
    ///     Добавить новую связь один-ко-многим
    /// </summary>
    public TableConfiguration<TEntity> AddConnection<TSecondEntity>(
        Expression<Func<TEntity, IEnumerable<TSecondEntity>?>> hasManySelector,
        Expression<Func<TSecondEntity, TEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
        where TSecondEntity : Entity, new()
    {
        var connection = TableConnection<TEntity, TSecondEntity>.CreateOneToManyConnection(
            hasManySelector, withOneSelector, foreignKeySelector, isRequired);
        _configItems.Add(connection);

        return this;
    }

    #endregion
}