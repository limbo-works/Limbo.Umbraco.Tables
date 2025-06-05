using System;
using Limbo.Umbraco.Tables.Models;
using Limbo.Umbraco.Tables.Parsers;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Property value converter for <see cref="TableEditor"/>.
/// </summary>
public class TableValueConverter(TablesHtmlParser htmlParser) : PropertyValueConverterBase {
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias == TableEditor.EditorAlias;
    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) {
        return source switch {
            JObject json => json,
            string str => str.DetectIsJson() ? JsonUtils.ParseJsonObject(str) : null,
            _ => null
        };
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {
        var config = propertyType.DataType.ConfigurationAs<TableConfiguration>()!;
        return TableModel.Parse(inter as JObject, config, htmlParser, preview);
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
        return typeof(TableModel);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {

        // Default to "Elements" if configuration doesn't match (probably wouldn't happen)
        var config = propertyType.DataType.ConfigurationAs<TableConfiguration>();
        if (config is  null) return PropertyCacheLevel.Elements;

        // Return the configured cachwe level (or "Elements" if not specified)
        return config.CacheLevel ?? PropertyCacheLevel.Elements;

    }

}