using HelperBox.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database.Services.Impl;

/// <summary>
///     Сервис конфигурации базы данных
/// </summary>
public abstract class ConfigurationService
{
    /// <summary>
    ///     Конфигурация базы данных
    /// </summary>
    protected DatabaseConfiguration DatabaseConfiguration { get; }

    /// <inheritdoc cref="ConfigurationService" />
    public ConfigurationService(DatabaseConfiguration databaseConfiguration) => DatabaseConfiguration = databaseConfiguration;

    /// <summary>
    ///     Конфигурировать базу данных
    /// </summary>
    /// <remarks>
    ///     Использовать <see cref="DatabaseConfiguration" />
    /// </remarks>
    public abstract void Setup(ModelBuilder modelBuilder);
}
