using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents the configuration for <see cref="TableEditor"/>.
/// </summary>
public class TableConfiguration : IConfigureValueType {

    /// <summary>
    /// Gets or sets whether the <strong>Use first row as header</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseFirstRowAsHeader")]
    public bool AllowUseFirstRowAsHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the <strong>Use first column as header</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseFirstColumnAsHeader")]
    public bool AllowUseFirstColumnAsHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the <strong>Use last row as footer</strong> option is enabled in the property editor.
    /// </summary>
    [ConfigurationField("allowUseLastRowAsFooter")]
    public bool AllowUseLastRowAsFooter { get; set; }

    /// <summary>
    /// Gets or sets whether the label of the property editor should be hidden.
    /// </summary>
    [ConfigurationField("hideLabel")]
    public bool HideLabel { get; set; }

    /// <summary>
    /// Configuration for the RTE.
    /// </summary>
    [ConfigurationField("rte")]
    public object? Rte { get; set; }

    /// <summary>
    /// Gets or sets the overlay size of the link picker overlay.
    /// </summary>
    [ConfigurationField("overlaySize")]
    public string? OverlaySize { get; set; }

    /// <summary>
    /// Gets or sets the property cache level of the underlying property value converter. Defaults to <see cref="PropertyCacheLevel.Elements"/> if not specified.
    /// </summary>
    [ConfigurationField("cacheLevel")]
    public PropertyCacheLevel? CacheLevel { get; set; }

    public string ValueType => ValueTypes.String;
}