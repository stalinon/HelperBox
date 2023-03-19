using HelperBox.Database.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HelperBox.Database;

/// <summary>
///     Design-time фабрика для <see cref="DatabaseContext" />
/// </summary>
internal sealed class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private readonly ConfigurationService _configurationService;
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc cref="DatabaseContextFactory" />
    public DatabaseContextFactory(ConfigurationService configurationService, IServiceProvider serviceProvider)
    {
        _configurationService = configurationService;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        optionsBuilder.UseNpgsql(
            Environment.GetEnvironmentVariable(ServiceCollectionExtensions.CONNECTION_STRING_HB),
            options => options.MigrationsHistoryTable("__EFMigrationsHistory")
        );

        return new DatabaseContext(_configurationService, optionsBuilder.Options, _serviceProvider);
    }
}

