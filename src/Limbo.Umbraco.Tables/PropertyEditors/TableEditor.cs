using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable 1591

namespace Limbo.Umbraco.Tables.PropertyEditors;

/// <summary>
/// Represents a block list property editor.
/// </summary>
[DataEditor(EditorAlias)]

public class TableEditor(
    IDataValueEditorFactory dataValueEditorFactory,
    IIOHelper ioHelper,
    TablePropertyIndexValueFactory indexValueFactory) : DataEditor(dataValueEditorFactory) {

    #region Constants

    /// <summary>
    /// Gets the alias of the <see cref="TableEditor"/> property editor.
    /// </summary>
    public const string EditorAlias = "limbo.table";


    #endregion

    #region Constructors

    #endregion

    #region Member methods

    public override IPropertyIndexValueFactory PropertyIndexValueFactory => indexValueFactory;

    protected override IConfigurationEditor CreateConfigurationEditor() => new TableConfigurationEditor(ioHelper);


    #endregion

}