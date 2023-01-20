using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database.Services.Impl;

/// <summary>
///     Сервис конфигурации базы данных
/// </summary>
public abstract class ConfigurationService
{
    /// <inheritdoc />
    public abstract void Setup(ModelBuilder modelBuilder);
}
