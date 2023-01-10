using System.ComponentModel.DataAnnotations.Schema;

namespace HelperBox.Database;

/// <summary>
///     Базовый класс таблицы
/// </summary>
public class BaseEntity
{
    /// <summary>
    ///     Идентификатор сущности в БД
    /// </summary>
    [Column("id")]
    public long Id { get; set; }
}