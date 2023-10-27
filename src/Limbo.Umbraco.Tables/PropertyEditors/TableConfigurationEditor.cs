using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents the configuration editor for the tables editor.
/// </summary>
public class TableConfigurationEditor : ConfigurationEditor<TableConfiguration> {

    public TableConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {

        int index1 = Fields.FindIndex(x => x.Key == "rte");

        if (index1 >= 0) {
            Fields.Insert(index1, new ConfigurationField {
                Key = "rteSeparator",
                Name = Fields[index1].Name,
                View = "/App_Plugins/Limbo.Umbraco.Tables/Views/TableSeparator.html",
                HideLabel = true
            });
        }

    }

}