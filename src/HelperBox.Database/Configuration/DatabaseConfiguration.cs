using HelperBox.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Конфигурация таблицы
/// </summary>
public class DatabaseConfiguration
{
    #region Fields

    private readonly List<IConfigItem> _configItems = new();

    #endregion

    #region Methods

    /// <summary>
    ///     Конфигурировать БД
    /// </summary>
    public void Setup(ModelBuilder modelBuilder) => _configItems.ForEach(i => i.Setup(modelBuilder));

    /// <summary>
    ///     Добавить новый индекс
    /// </summary>
    public DatabaseConfiguration AddIndex<TEntity>(
        Expression<Func<TEntity, object?>> propertySelector,
        bool isUnique = false)
        where TEntity: Entity, new()
    {
        var index = new TableIndex<TEntity>(propertySelector, isUnique);
        _configItems.Add(index);

        return this;
    }

    /// <summary>
    ///     Добавить новую связь один-к-одному
    /// </summary>
    public DatabaseConfiguration AddConnection<TEntity, TSecondEntity>(
        Expression<Func<TEntity, TSecondEntity?>> hasOneSelector,
        Expression<Func<TSecondEntity, TEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
        where TEntity : Entity, new()
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
    public DatabaseConfiguration AddConnection<TEntity, TSecondEntity>(
        Expression<Func<TEntity, IEnumerable<TSecondEntity>?>> hasManySelector,
        Expression<Func<TSecondEntity, TEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
        where TEntity : Entity, new()
        where TSecondEntity : Entity, new()
    {
        var connection = TableConnection<TEntity, TSecondEntity>.CreateOneToManyConnection(
            hasManySelector, withOneSelector, foreignKeySelector, isRequired);
        _configItems.Add(connection);

        return this;
    }

    /// <summary>
    ///     Генерировать документацию
    /// </summary>
    internal TableDocumentation[] GenerateDocumentation()
    {
        var list = new List<TableDocumentation>();

        foreach (var configItem in _configItems)
        {
            var table = configItem.Table;
            var columns = table.GetProperties();

            var desc = string.Empty;
            desc += $"### {configItem.Type} `{configItem.Name}`\n";
            desc += $"{configItem.Description}";

            list.Add(new(table, columns, desc));
        }

        return list.ToArray();
    }

    #endregion
}