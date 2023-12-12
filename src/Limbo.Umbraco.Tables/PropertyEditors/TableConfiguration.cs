using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents the configuration for <see cref="TableEditor"/>.
/// </summary>
public class TableConfiguration {

    /// <summary>
    /// Gets or sets whether the <strong>Use first row as header</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseFirstRowAsHeader", "Allow 'Use first row as header' option", "boolean")]
    public bool AllowUseFirstRowAsHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the <strong>Use first column as header</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseFirstColumnAsHeader", "Allow 'Use first column as header' option", "boolean")]
    public bool AllowUseFirstColumnAsHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the <strong>Use last row as footer</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseLastRowAsFooter", "Allow 'Use last row as footer' option", "boolean")]
    public bool AllowUseLastRowAsFooter { get; set; }

    /// <summary>
    /// Gets or sets whether the label of the property editor should be hidden.
    /// </summary>
    [ConfigurationField("hideLabel", "Hide label", "boolean", Description = "Select whether the label and description of properties using this data type should be hidden.")]
    public bool HideLabel { get; set; }

    /// <summary>
    /// Configuration for the RTE.
    /// </summary>
    [ConfigurationField("rte", "Rich text editor", "views/propertyeditors/rte/rte.prevalues.html", Description = "Rich text editor configuration", HideLabel = true)]
    public object? Rte { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to <see cref="PropertyCacheLevel.Elements"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel", "Cache Level", "/App_Plugins/Limbo.Umbraco.Tables/Views/CacheLevel.html", Description = "Select the cache level of the underlying property value converter.")]
    public PropertyCacheLevel? CacheLevel { get; set; }

}