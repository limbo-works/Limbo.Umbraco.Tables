using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Tables.PropertyEditors {

    /// <summary>
    /// Represents the configuration for the tables editor.
    /// </summary>
    public class TablesDataConfiguration {

        /// <summary>
        /// Editor
        /// </summary>
        [ConfigurationField("editor", "Editor", "views/propertyeditors/rte/rte.prevalues.html", HideLabel = true)]
        public object? Editor { get; set; }

    }

}