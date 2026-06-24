using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Tables.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Index value factory for <see cref="TableEditor"/>. Extracts plain-text cell content for full-text search indexing.
/// </summary>
public class TablePropertyIndexValueFactory : IPropertyIndexValueFactory {
    private static IEnumerable<string?> ProcessRow(JArray row) {

        foreach (JToken cell in row) {

            JToken? value = cell.Value<JToken>("value");

            switch (value?.Type) {

                case JTokenType.String:
                    yield return value.Value<string>()?.StripHtml();
                    break;

                case JTokenType.Object:
                    string? markup = ((JObject) value).GetValue("markup")?.Value<string>();
                    yield return markup?.StripHtml();
                    break;

            }

        }

    }

    /// <inheritdoc />
    public IEnumerable<IndexValue> GetIndexValues(IProperty property, string? culture, string? segment, bool published, IEnumerable<string> availableCultures,
        IDictionary<Guid, IContentType> contentTypeDictionary) {
        // Get the source value from the property
        object? source = property.GetValue(culture, segment, published);

        // Validate the source value
        if (source is not string str || !str.DetectIsJson()) yield break;

        // Add the property value (JSON serialized string) to the index
        yield return new IndexValue() {
            Culture = culture,
            FieldName = property.Alias,
            Values = [str]
        };

        // Parse the property value into a JSON object
        JObject tableData = JsonUtils.ParseJsonObject(str);

        // We can't parse via TablesHtmlParser as the HtmlParser attempts to resolve umbraco links
        // But there is no UmbracoContext at this point, so it throws an exception
        var processedData = tableData
            .GetArrayOrNew("cells")
            .ForEach((_, x) => ProcessRow(x))
            .WhereNotNull()
            .SelectMany(c => c);
        // Return the search friendly text based on the JSON structure
        yield return new IndexValue() {
            Culture = culture,
            FieldName = $"{property.Alias}_search",
            Values = [processedData]
        };

    }
}