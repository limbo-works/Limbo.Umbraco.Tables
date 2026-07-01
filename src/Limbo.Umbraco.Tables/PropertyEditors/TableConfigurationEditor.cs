using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents the configuration editor for the tables editor.
/// </summary>
public class TableConfigurationEditor(IIOHelper ioHelper) : ConfigurationEditor<TableConfiguration>(ioHelper);