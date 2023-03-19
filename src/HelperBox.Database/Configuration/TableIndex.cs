using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Индекс таблицы
/// </summary>
internal class TableIndex<TEntity> : IConfigItem
    where TEntity : Entity, new()
{
    #region Properties

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

    #endregion

    #region Constructor

    /// <inheritdoc cref="TableIndex{TEntity}" />
    /// <param name="propertySelector"> Выбор свойств (колонок таблицы) </param>
    /// <param name="isUnique"> Явяется ли индекс уникальным </param>
    internal TableIndex(Expression<Func<TEntity, object?>> propertySelector, bool isUnique)
    {
        PropertySelector = propertySelector;
        Columns = propertySelector.GetPropertyAccessList().ToArray();
        IsUnique = isUnique;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Настроить индекс в БД
    /// </summary>
    public void Setup(ModelBuilder modelBuilder)
        => modelBuilder.Entity<TEntity>()
                       .HasIndex(PropertySelector)
                       .HasDatabaseName(ToString())
                       .IsUnique(IsUnique);

    /// <inheritdoc />
    public override string ToString()
    {
        return $"IX_{GetStringRepresentationOfNameAndColumns() + (IsUnique ? "_unique" : string.Empty)}";

        string GetStringRepresentationOfNameAndColumns() 
            => string.Join(
                "_",
                Columns.Select(c => c.GetCustomAttribute<ColumnAttribute>()!.Name)
                       .Prepend(typeof(TEntity).GetCustomAttribute<TableAttribute>()!.Name));

    }

    #endregion
}