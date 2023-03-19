namespace HelperBox.Database.Services;

/// <summary>
///     Генератор документации
/// </summary>
internal interface IDocGenerator
{
    /// <summary>
    ///     Сгенерировать документацию
    /// </summary>
    Task GenerateDocumentationAsync();
}
