using System.Reflection;

namespace HelperBox.Database.Models;

/// <summary>
///     Документация таблицы
/// </summary>
internal class TableDocumentation
{
    /// <summary>
    ///     Тип таблицы
    /// </summary>
    public Type Table { get; set; }

    /// <summary>
    ///     Колонки таблицы
    /// </summary>
    public PropertyInfo[] Columns { get; set; }

    /// <summary>
    ///     Описание конфигурации таблицы
    /// </summary>
    public string ConfigurationDescription { get; private set; }

    /// <inheritdoc cref="TableDocumentation" />
    public TableDocumentation(Type table, PropertyInfo[] columns, string configurationDescription)
    {
        Table = table;
        Columns = columns;
        ConfigurationDescription = configurationDescription;
    }
}
