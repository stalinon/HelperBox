using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Индекс таблицы
/// </summary>
public class TableIndex<TEntity>
    where TEntity : BaseEntity, new()
{
    /// <summary>
    ///     Индексированные колонки таблицы
    /// </summary>
    public PropertyInfo[] Columns { get; } = Array.Empty<PropertyInfo>();

    /// <summary>
    ///     Выражение выбора свойств
    /// </summary>
    public Expression<Func<TEntity, object?>> PropertySelector { get; }

    /// <summary>
    ///     Признак уникальности индекса
    /// </summary>
    public bool IsUnique { get; }

    /// <inheritdoc cref="TableIndex{TEntity}" />
    /// <param name="propertySelector"> Выбор свойств (колонок таблицы) </param>
    /// <param name="isUnique"> Явялется ли индекс уникальным </param>
    public TableIndex(Expression<Func<TEntity, object?>> propertySelector, bool isUnique)
    {
        PropertySelector = propertySelector;
        Columns = propertySelector.GetPropertyAccessList().ToArray();
        IsUnique = isUnique;
    }

    /// <inheritdoc />
    public override string ToString()
        => $"IX_{StringColumns + (IsUnique ? "_unique" : string.Empty)}";

    private string StringColumns
        => string.Join("_", Columns.Select(c => c.GetCustomAttribute<ColumnAttribute>()!.Name));
}