using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.StructuredData.PropertyEditors {

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
    public class StructuredDataEditor : DataEditor {

        #region Constants

        /// <summary>
        /// Gets the alias of the <see cref="StructuredDataEditor"/> property editor.
        /// </summary>
        public const string EditorAlias = "Limbo.Umbraco.StructuredData";

        /// <summary>
        /// Gets the name of the <see cref="StructuredDataEditor"/> property editor.
        /// </summary>
        public const string EditorName = "Limbo Structured Data";

        /// <summary>
        /// Gets the URL of the view of the <see cref="StructuredDataEditor"/> property editor.
        /// </summary>
        public const string EditorView = "/App_Plugins/Limbo.Umbraco.StructuredData/Views/StructuredDataEditor.html";

        /// <summary>
        /// Gets the icon of the <see cref="StructuredDataEditor"/> property editor.
        /// </summary>
        public const string EditorIcon = "icon-grid color-limbo";

        #endregion

        #region Constructors

        public StructuredDataEditor(IDataValueEditorFactory dataValueEditorFactory) : base(dataValueEditorFactory) { }

        #endregion

    }

}