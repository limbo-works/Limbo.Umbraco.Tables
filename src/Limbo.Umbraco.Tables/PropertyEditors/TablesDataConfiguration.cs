using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.Tables.PropertyEditors {

    /// <summary>
    /// Represents the configuration for the tables editor.
    /// </summary>
    public class TablesDataConfiguration {

        ///// <summary>
        ///// Gets or sets whether the <strong>Use first row as header</strong> option is enabled in the property editor.
        ///// </summary>
        //[ConfigurationField("allowUseFirstRowAsHeader", "Allow 'Use first row as header' option", "boolean")]
        //public bool AllowUseFirstRowAsHeader { get; set; } = true;

        ///// <summary>
        ///// Gets or sets whether the <strong>Use first column as header</strong> option is enabled in the property editor.
        ///// </summary>
        //[ConfigurationField("allowUseFirstColumnAsHeader", "Allow 'Use first column as header' option", "boolean")]
        //public bool AllowUseFirstColumnAsHeader { get; set; } = true;

        ///// <summary>
        ///// Gets or sets whether the <strong>Use last row as footer</strong> option is enabled in the property editor.
        ///// </summary>
        //[ConfigurationField("allowUseLastRowAsFooter", "Allow 'Use last row as footer' option", "boolean")]
        //public bool AllowUseLastRowAsFooter { get; set; }

        /// <summary>
        /// Editor
        /// </summary>
        [ConfigurationField("editor", "Editor", "views/propertyeditors/rte/rte.prevalues.html", HideLabel = true)]
        public object? Editor { get; set; }

    }

}