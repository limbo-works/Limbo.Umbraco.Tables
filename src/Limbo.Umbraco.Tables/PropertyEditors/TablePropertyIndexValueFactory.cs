using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Tables.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Tables.PropertyEditors;

internal class TablePropertyIndexValueFactory : IPropertyIndexValueFactory {

    public IEnumerable<KeyValuePair<string, IEnumerable<object?>>> GetIndexValues(IProperty property, string? culture, string? segment, bool published) {

        // Get the source value from the property
        object? source = property.GetValue(culture, segment, published);

        // Validate the source value
        if (source is not string str || !str.DetectIsJson()) yield break;

        // Add the property value (JSON serialized string) to the index
        yield return new KeyValuePair<string, IEnumerable<object?>>(property.Alias, new[] { str });

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
        yield return new KeyValuePair<string, IEnumerable<object?>>($"{property.Alias}_search", processedData);

    }

    private static IEnumerable<string?> ProcessRow(JArray row) {
        foreach (JToken cell in row) {
            yield return cell.Value<string>("value")?.StripHtml();
        }
    }

}