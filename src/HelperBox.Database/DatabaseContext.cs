using HelperBox.Database.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database;

/// <summary>
///     Базовый класс контекста базы данных
/// </summary>
public class DatabaseContext : DbContext
{
    private readonly ConfigurationService _configurationService;

    /// <inheritdoc cref="DatabaseContext" />
    public DatabaseContext(ConfigurationService configurationService, DbContextOptions options) : base(options) 
        => _configurationService = configurationService;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _configurationService.Setup(modelBuilder);
    }

    internal static Type[] GetEntityTypes() => typeof(DatabaseContext)
            .GetProperties()
            .Where(p =>
                p.PropertyType.IsGenericType
                && p.PropertyType.ContainsGenericParameters
                && p.PropertyType.GenericTypeArguments.Length == 1
                && p.PropertyType.GenericTypeArguments.First().BaseType == typeof(Entity))
            .Select(p => p.PropertyType.GenericTypeArguments.Single())
            .ToArray();
}