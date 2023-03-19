using DocXml.Reflection;
using HelperBox.Database.Configuration;
using HelperBox.Database.Models;
using LoxSmoke.DocXml;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace HelperBox.Database.Services.Impl;

/// <inheritdoc cref="IDocGenerator" />
internal class DocGenerator : IDocGenerator
{
    #region Fields

    private readonly DocXmlReader _docXmlReader;
    private readonly DatabaseConfiguration _databaseConfig;

    #endregion

    #region Constructor

    /// <inheritdoc cref="DocGenerator" />
    public DocGenerator(DocXmlReader docXmlReader, DatabaseConfiguration databaseConfig)
    {
        _docXmlReader = docXmlReader;
        _databaseConfig = databaseConfig;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task GenerateDocumentationAsync()
    {
        var docs = _databaseConfig.GenerateDocumentation();
        var documentation = GetDocumentation(docs);

        var filePath = Environment.GetEnvironmentVariable(ServiceCollectionExtensions.GENERATED_DOC_PATH)!;
        await File.WriteAllTextAsync(filePath, documentation);
    }

    private string GetDocumentation(TableDocumentation[] docs)
    {
        var documentation = string.Empty;

        foreach (var doc in docs)
        {
            var tableName = doc.Table.GetCustomAttribute<TableAttribute>()!.Name;
            var tableDesc = _docXmlReader.GetTypeComments(doc.Table);
            var columnDesc = GetColumnDescriptions(doc);

            documentation += $"## Table `{tableName}`\n\n";
            documentation += tableDesc + "\n\n";

            for (var i = 1; i < columnDesc.Length + 1; i++)
            {
                var column = columnDesc[i - 1];
                documentation += $"{i}. {column}. \n";
            }

            documentation += "\n\n";
            documentation += tableDesc + "\n\n";
        }

        return documentation;
    }

    private string[] GetColumnDescriptions(TableDocumentation doc)
    {
        var columnDescs = new List<string>();

        foreach (var column in doc.Columns)
        {
            var columnName = column.GetCustomAttribute<ColumnAttribute>()!.Name;
            var columnDesc = _docXmlReader.GetMemberComment(column);
            var columnType = column.PropertyType.ToNameString();

            var columnDoc = $"`{columnName}` (type:{columnType}) - {columnDesc}";
            columnDescs.Add(columnDoc);
        }

        return columnDescs.ToArray();
    }

    #endregion
}
