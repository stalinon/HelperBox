namespace HelperBox.Database.Services;

/// <summary>
///     Фабрика контекстов репозиториев
/// </summary>
public interface IRepositoryContextFactory
{
    /// <summary>
    ///     Создать scoped-контекст репозиториев
    /// </summary>
    IRepositoryContext CreateScope();
}
