using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database.Services.Impl;

/// <summary>
///     Сервис конфигурации базы данных
/// </summary>
public interface IConfigurationService
{
    /// <inheritdoc />
    void Setup(ModelBuilder modelBuilder);
}
