namespace HelperBox.Database.Services;

/// <summary>
///     Фабрика <see cref="IRepositoryContext" />
/// </summary>
internal interface IRepositoryContextFactory<TDbContext>
    where TDbContext : DatabaseContext
{
    /// <summary>
    ///     Создаёт подключение к БД
    /// </summary>
    TDbContext CreateScopeDatabase();
}
