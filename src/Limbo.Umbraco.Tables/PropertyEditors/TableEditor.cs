using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents a block list property editor.
/// </summary>
[DataEditor(EditorAlias)]

public class TableEditor : DataEditor {

    private readonly IIOHelper _ioHelper;
    private readonly TablePropertyIndexValueFactory _indexValueFactory;

    #region Constants

    /// <summary>
    /// Gets the alias of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorAlias = "Limbo.Umbraco.Tables";

    #endregion

    #region Constructors

    /// <summary>
    /// Represents a block list property editor.
    /// </summary>
    public TableEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, TablePropertyIndexValueFactory indexValueFactory) : base(dataValueEditorFactory)
    {
        _ioHelper = ioHelper;
        _indexValueFactory = indexValueFactory;
    }

    #endregion

    #region Member methods

    public override IPropertyIndexValueFactory PropertyIndexValueFactory => _indexValueFactory;

    protected override IConfigurationEditor CreateConfigurationEditor() => new TableConfigurationEditor(_ioHelper);


    #endregion

}