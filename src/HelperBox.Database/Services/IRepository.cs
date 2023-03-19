using HelperBox.Database.Enums;
using System.Linq.Expressions;

namespace HelperBox.Database.Services;

/// <summary>
///     Репозиторий
/// </summary>
public interface IRepository<TEntity>
    where TEntity : Entity
{
    /// <summary>
    ///     Добавляет <paramref name="entity"/> в контекст
    /// </summary>
    void Add(TEntity entity);

    /// <summary>
    ///     Добавляет <paramref name="entity"/> в контекст
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Добавляет <paramref name="entities"/> в контекст
    /// </summary>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Удаляет <paramref name="entity"/> из контекста
    /// </summary>
    void Remove(TEntity entity);

    /// <summary>
    ///     Удаляет <paramref name="entities"/> из контекста
    /// </summary>
    void RemoveRange(TEntity[] entities);

    /// <summary>
    ///     Сохранение контекста
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Создать произвольный запрос на получение сущностей из БД
    /// </summary>
    IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    ///     Получить объекты согласно фильтру
    /// </summary>
    Task<TEntity[]> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? sort = null,
        SortType sortDirection = SortType.ASC,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Получить объект согласно фильтру
    /// </summary>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Возвращает список неотслеживаемых сущностей
    /// </summary>
    IQueryable<TEntity> NoTracking();

}

