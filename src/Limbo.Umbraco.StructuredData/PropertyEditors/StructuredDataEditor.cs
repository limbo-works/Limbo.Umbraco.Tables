using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.StructuredData.PropertyEditors {

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    [DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
    public class StructuredDataEditor : DataEditor {

        #region Constants

        internal const string EditorAlias = "Limbo.Umbraco.StructuredData";

        internal const string EditorName = "Limbo Structured Data";

        internal const string EditorView = "/App_Plugins/Limbo.Umbraco.StructuredData/Views/StructuredDataEditor.html";

        internal const string EditorIcon = "icon-grid color-limbo";

        #endregion

        #region Constructors

        public StructuredDataEditor(IDataValueEditorFactory dataValueEditorFactory) : base(dataValueEditorFactory) { }

        #endregion

    }

}