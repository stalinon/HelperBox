using HelperBox.Database.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Связи таблицы
/// </summary>
internal class TableConnection<TFirstEntity, TSecondEntity> : IConfigItem
    where TFirstEntity : Entity, new()
    where TSecondEntity : Entity, new()
{
    #region Properties

    /// <summary>
    ///     Тип связи
    /// </summary>
    internal ConnectionType ConnectionType { get; private set; }

    #region Single

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    internal Expression<Func<TFirstEntity, TSecondEntity?>> HasSelector { get; private set; } = null!;

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    internal Expression<Func<TSecondEntity, TFirstEntity?>> WithSelector { get; private set; } = null!;

    #endregion

    #region Multiple

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    internal Expression<Func<TFirstEntity, IEnumerable<TSecondEntity>?>> HasManySelector { get; private set; } = null!;

    #endregion

    /// <summary>
    ///     Выражение выбора поля для foreign key
    /// </summary>
    internal Expression<Func<TSecondEntity, object?>> ForeignKeySelector { get; private set; } = null!;

    /// <summary>
    ///     Обязательно ли отношение
    /// </summary>
    internal bool IsRequired { get; private set; }

    #endregion

    #region Constructor

    /// <inheritdoc cref="TableConnection{TFirstEntity, TSecondEntity}" />
    private TableConnection()
    { }

    #endregion

    #region Methods

    internal static TableConnection<TFirstEntity, TSecondEntity> CreateOneToOneConnection(
        Expression<Func<TFirstEntity, TSecondEntity?>> hasOneSelector,
        Expression<Func<TSecondEntity, TFirstEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
    {
        var connection = new TableConnection<TFirstEntity, TSecondEntity>()
        {
            ConnectionType = ConnectionType.ONE_TO_ONE,
            HasSelector = hasOneSelector,
            WithSelector = withOneSelector,
            ForeignKeySelector = foreignKeySelector,
            IsRequired = isRequired
        };

        return connection;
    }

    internal static TableConnection<TFirstEntity, TSecondEntity> CreateOneToManyConnection(
        Expression<Func<TFirstEntity, IEnumerable<TSecondEntity>?>> hasManySelector,
        Expression<Func<TSecondEntity, TFirstEntity?>> withOneSelector,
        Expression<Func<TSecondEntity, object?>> foreignKeySelector,
        bool isRequired = false)
    {
        var connection = new TableConnection<TFirstEntity, TSecondEntity>()
        {
            ConnectionType = ConnectionType.ONE_TO_MANY,
            HasManySelector = hasManySelector,
            WithSelector = withOneSelector,
            ForeignKeySelector = foreignKeySelector,
            IsRequired = isRequired
        };

        return connection;
    }

    /// <summary>
    ///     Настроить связь в БД
    /// </summary>
    public void Setup(ModelBuilder modelBuilder)
    {
        switch (ConnectionType)
        {
            case ConnectionType.ONE_TO_ONE when IsRequired:
                modelBuilder.Entity<TFirstEntity>()
                            .HasOne(HasSelector)
                            .WithOne(WithSelector)
                            .IsRequired()
                            .HasConstraintName(ToString());
                break;
            case ConnectionType.ONE_TO_MANY when IsRequired:
                modelBuilder.Entity<TFirstEntity>()
                            .HasMany(HasManySelector)
                            .WithOne(WithSelector)
                            .IsRequired()
                            .HasConstraintName(ToString());
                break;
            case ConnectionType.ONE_TO_ONE when !IsRequired:
                modelBuilder.Entity<TFirstEntity>()
                            .HasOne(HasSelector)
                            .WithOne(WithSelector)
                            .HasConstraintName(ToString());
                break;
            case ConnectionType.ONE_TO_MANY when !IsRequired:
                modelBuilder.Entity<TFirstEntity>()
                            .HasMany(HasManySelector)
                            .WithOne(WithSelector)
                            .HasConstraintName(ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ConnectionType));
        }
    }

    public override string ToString() 
        => $"FK_{typeof(TFirstEntity).GetCustomAttribute<TableAttribute>()!.Name}_{typeof(TSecondEntity).GetCustomAttribute<TableAttribute>()!.Name}";

    #endregion
}
