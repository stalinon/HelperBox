using HelperBox.Database.Services;
using HelperBox.Database.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HelperBox.Database;

/// <summary>
///     Базовый класс контекста базы данных
/// </summary>
public class DatabaseContext : DbContext
{
    private readonly ConfigurationService _configurationService;
    private readonly IDocGenerator? _docGenerator;

    /// <inheritdoc cref="DatabaseContext" />
    public DatabaseContext(
        ConfigurationService configurationService, 
        DbContextOptions options, 
        IServiceProvider serviceProvider) : base(options)
    {
        _configurationService = configurationService;
        _docGenerator = serviceProvider.GetRequiredService<IDocGenerator>();
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _configurationService.Setup(modelBuilder);

        _docGenerator?.GenerateDocumentationAsync().RunSynchronously();
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