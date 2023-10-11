using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Tables.Models;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Limbo.Umbraco.Tables.PropertyEditors {

    internal class TablesDataPropertyIndexValueFactory : IPropertyIndexValueFactory {

        public IEnumerable<KeyValuePair<string, IEnumerable<object?>>> GetIndexValues(IProperty property, string? culture, string? segment, bool published) {

            object? source = property.GetValue(culture, segment, published);

            if (source is not string str || !str.DetectIsJson()) {
                yield break;
            }

            JObject tableData = JsonUtils.ParseJsonObject(str);

            // We can't parse via TablesHtmlParser as the HtmlParser attempts to resolve umbraco links
            // But there is no UmbracoContext at this point, so it throws an exception
            var processedData = tableData
                .GetArrayOrNew("cells")
                .ForEach((_, x) => ProcessRow(x))
                .WhereNotNull()
                .SelectMany(c => c);

            yield return new KeyValuePair<string, IEnumerable<object?>>(property.Alias, processedData);

        }

        private static IEnumerable<string?> ProcessRow(JArray row) {
            foreach (JToken cell in row) {
                yield return cell.Value<string>("value")?.StripHtml();
            }
        }

    }

}