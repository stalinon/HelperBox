using HelperBox.Database.Services;
using HelperBox.Database.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HelperBox.Database;

/// <summary>
///     Расширения для <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    internal const string CONNECTION_STRING_HB = nameof(CONNECTION_STRING_HB);

    /// <summary>
    ///     Конфигурировать базу данных
    /// </summary>
    /// <typeparam name="TConfigurationService"> Сервис конфигурации БД </typeparam>
    /// <typeparam name="TDbContext"> Контекст БД </typeparam>
    public static IServiceCollection ConfigureDatabase<TConfigurationService, TDbContext>(
        this IServiceCollection services,
        string connectionString)
        where TConfigurationService : ConfigurationService
        where TDbContext : DatabaseContext
    {
        Environment.SetEnvironmentVariable(CONNECTION_STRING_HB, connectionString);

        services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<ConfigurationService, TConfigurationService>();
        services.AddSingleton<ConfigurationService, TConfigurationService>();

        var entityTypes = (Type[])typeof(TDbContext)
            .GetMethod(nameof(DatabaseContext.GetEntityTypes))!
            .Invoke(null, null)!;

        AddRepositories(services, entityTypes);

        return services;
    }

    private static void AddRepositories(IServiceCollection services, Type[] entityTypes)
    {
        foreach (var entityType in entityTypes)
        {
            services.AddSingleton(
                typeof(IRepository<>).MakeGenericType(entityType),
                typeof(Repository<>).MakeGenericType(entityType));
        }
    }
}
