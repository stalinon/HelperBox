using HelperBox.Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    ///     Название первой таблицы
    /// </summary>
    private string FirstTableName { get; } = typeof(TFirstEntity).GetCustomAttribute<TableAttribute>()!.Name;

    /// <summary>
    ///     Название второй таблицы
    /// </summary>
    private string SecondTableName { get; } = typeof(TSecondEntity).GetCustomAttribute<TableAttribute>()!.Name;

    /// <summary>
    ///     Тип связи
    /// </summary>
    private ConnectionType ConnectionType { get; set; }

    #region Single

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    private Expression<Func<TFirstEntity, TSecondEntity?>> HasSelector { get; set; } = null!;

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    private Expression<Func<TSecondEntity, TFirstEntity?>> WithSelector { get; set; } = null!;

    #endregion

    #region Multiple

    /// <summary>
    ///     Выражение выбора поля для маппинга
    /// </summary>
    private Expression<Func<TFirstEntity, IEnumerable<TSecondEntity>?>> HasManySelector { get; set; } = null!;

    #endregion

    /// <summary>
    ///     Выражение выбора поля для foreign key
    /// </summary>
    private Expression<Func<TSecondEntity, object?>> ForeignKeySelector { get; set; } = null!;

    /// <summary>
    ///     Обязательно ли отношение
    /// </summary>
    private bool IsRequired { get; set; }

    /// <inheritdoc />
    public Type Table => typeof(TFirstEntity);

    /// <inheritdoc />
    public string Type { get; } = "Relationship";

    /// <inheritdoc />
    public string Name => ToString();

    /// <inheritdoc />
    public string Description
    {
        get
        {
            var desc = $"This {(IsRequired ? "required" : string.Empty)} {GetConnectionTypeName()} relationship is beetween two tables: \n";
            desc += $"+ `{FirstTableName}`";
            desc += $"+ `{SecondTableName}`";
            desc += $"Foreign key is the `{ForeignKeySelector.GetPropertyAccess().GetCustomAttribute<ColumnAttribute>()!.Name}` column.";

            return desc;
        }
    }

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
        => $"FK_{FirstTableName}_{SecondTableName}";

    private string GetConnectionTypeName() => ConnectionType switch
    {
        ConnectionType.ONE_TO_ONE => "one-to-one",
        ConnectionType.ONE_TO_MANY => "one-to-many",
        _ => throw new ArgumentOutOfRangeException()
    };

    #endregion
}
