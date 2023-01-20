namespace HelperBox.Database.Services;

/// <summary>
///     Контекст репозиториев
/// </summary>
internal interface IRepositoryContext : IDisposable
{
    /// <summary>
    ///     Сохранить контекст БД
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Получить репозиторий
    /// </summary>
    IRepository<TEntity> Repository<TEntity>() where TEntity : Entity;
}
