using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents a block list property editor.
/// </summary>
[DataEditor(EditorAlias, EditorName, EditorView, ValueType = ValueTypes.Json, Group = "Limbo", Icon = EditorIcon)]
public class TableEditor : DataEditor {

    #region Constants

    /// <summary>
    /// Gets the alias of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorAlias = "Limbo.Umbraco.Tables";

    /// <summary>
    /// Gets the name of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorName = "Limbo Tables";

    /// <summary>
    /// Gets the URL of the view of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorView = "/App_Plugins/Limbo.Umbraco.Tables/Views/TableEditor.html";

    /// <summary>
    /// Gets the icon of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorIcon = "icon-grid color-limbo";

    #endregion

    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    #region Constructors

    public TableEditor(
        IDataValueEditorFactory dataValueEditorFactory,
        IIOHelper ioHelper,
        IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    #endregion

    #region Member methods

    public override IPropertyIndexValueFactory PropertyIndexValueFactory => new TablePropertyIndexValueFactory();

    protected override IConfigurationEditor CreateConfigurationEditor() => new TableConfigurationEditor(_ioHelper, _editorConfigurationParser);

    public override IDataValueEditor GetValueEditor(object? configuration) {

        IDataValueEditor editor = base.GetValueEditor(configuration);

        if (editor is DataValueEditor dve) {
            dve.View += $"?v={TablesPackage.Version}";
        }

        return editor;

    }

    #endregion

}