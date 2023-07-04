using System;
using Limbo.Umbraco.Tables.Models;
using Limbo.Umbraco.Tables.Parsers;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors {

    /// <summary>
    /// Property value converter for <see cref="TablesDataEditor"/>.
    /// </summary>
    public class TablesDataValueConverter : PropertyValueConverterBase {

        private readonly TablesHtmlParser _htmlParser;

        public TablesDataValueConverter(TablesHtmlParser htmlParser) {
            _htmlParser = htmlParser;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias == TablesDataEditor.EditorAlias;
        }

        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
            return source switch {
                JObject json => json,
                string str => str.DetectIsJson() ? JsonUtils.ParseJsonObject(str) : null,
                _ => null
            };
        }

        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
            return TablesDataModel.Parse(inter as JObject, _htmlParser, preview);
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(TablesDataModel);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Elements;
        }

    }

}