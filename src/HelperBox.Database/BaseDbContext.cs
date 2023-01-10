using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database;

/// <summary>
///     Базовый класс контекста базы данных
/// </summary>
public abstract class BaseDbContext : DbContext
{
    /// <inheritdoc cref="BaseDbContext" />
    protected BaseDbContext(DbContextOptions options) : base(options)
    { }
}