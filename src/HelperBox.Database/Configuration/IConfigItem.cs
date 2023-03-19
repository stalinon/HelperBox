using Microsoft.EntityFrameworkCore;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Элемент конфигурации
/// </summary>
internal interface IConfigItem
{
    /// <summary>
    ///     Установить конфигурацию
    /// </summary>
    void Setup(ModelBuilder modelBuilder);

    /// <summary>
    ///     Тип таблицы, к которой относится элемент конфигурации
    /// </summary>
    Type Table { get; }

    /// <summary>
    ///     Тип элемента конфигурации
    /// </summary>
    string Type { get; }

    /// <summary>
    ///     Название элемента конфигурации
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Описание элемента конфигурации
    /// </summary>
    string Description { get; }
}
