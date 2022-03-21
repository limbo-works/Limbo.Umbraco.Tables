using System;
using Limbo.Umbraco.StructuredData.Models;
using Limbo.Umbraco.StructuredData.Parsers;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.StructuredData.PropertyEditors {

    /// <summary>
    /// Property value converter for <see cref="StructuredDataEditor"/>.
    /// </summary>
    public class StructuredDataValueConverter : PropertyValueConverterBase {

        private readonly StructuredDataHtmlParser _htmlParser;

        public StructuredDataValueConverter(StructuredDataHtmlParser htmlParser) {
            _htmlParser = htmlParser;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias == StructuredDataEditor.EditorAlias;
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {
            return source is string str && str.DetectIsJson() ? JsonUtils.ParseJsonObject(str) : null;
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return StructuredDataModel.Parse(inter as JObject, _htmlParser, preview);
        }

        public override object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return null;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(StructuredDataModel);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Elements;
        }

    }

}