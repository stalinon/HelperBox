using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Индекс таблицы
/// </summary>
internal class TableIndex<TEntity>
    where TEntity : Entity, new()
{
    /// <summary>
    ///     Индексированные колонки таблицы
    /// </summary>
    private PropertyInfo[] Columns { get; } = Array.Empty<PropertyInfo>();

    /// <summary>
    ///     Выражение выбора свойств
    /// </summary>
    private Expression<Func<TEntity, object?>> PropertySelector { get; }

    /// <summary>
    ///     Признак уникальности индекса
    /// </summary>
    private bool IsUnique { get; }

    /// <inheritdoc cref="TableIndex{TEntity}" />
    /// <param name="propertySelector"> Выбор свойств (колонок таблицы) </param>
    /// <param name="isUnique"> Явяется ли индекс уникальным </param>
    internal TableIndex(Expression<Func<TEntity, object?>> propertySelector, bool isUnique)
    {
        PropertySelector = propertySelector;
        Columns = propertySelector.GetPropertyAccessList().ToArray();
        IsUnique = isUnique;
    }

    /// <summary>
    ///     Настроить индекс в БД
    /// </summary>
    internal void Setup(ModelBuilder modelBuilder)
        => modelBuilder.Entity<TEntity>()
                       .HasIndex(PropertySelector)
                       .HasDatabaseName(ToString())
                       .IsUnique(IsUnique);

    /// <inheritdoc />
    public override string ToString()
        => $"IX_{StringColumns + (IsUnique ? "_unique" : string.Empty)}";

    private string StringColumns
        => string.Join("_", Columns.Select(c => c.GetCustomAttribute<ColumnAttribute>()!.Name));
}