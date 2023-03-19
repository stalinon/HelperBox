﻿using HelperBox.Database.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HelperBox.Database;

/// <summary>
///     Design-time фабрика для <see cref="DatabaseContext" />
/// </summary>
internal sealed class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private readonly IConfigurationService _configurationService;

    /// <inheritdoc cref="DatabaseContextFactory" />
    public DatabaseContextFactory(IConfigurationService configurationService) 
        => _configurationService = configurationService;

    /// <inheritdoc/>
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

        optionsBuilder.UseNpgsql(
            Environment.GetEnvironmentVariable(ServiceCollectionExtensions.CONNECTION_STRING_HB),
            options => options.MigrationsHistoryTable("__EFMigrationsHistory")
        );

        return new DatabaseContext(_configurationService, optionsBuilder.Options);
    }
}

