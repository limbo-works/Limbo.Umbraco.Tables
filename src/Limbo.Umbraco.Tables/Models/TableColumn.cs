using Newtonsoft.Json;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing a column in a <see cref="TableModel"/> value.
/// </summary>
public class TableColumn {

    /// <summary>
    /// Gets a reference to the parent <see cref="TableModel"/>.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public TableModel Table { get; }

    /// <summary>
    /// Gets the column index.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int Index { get; }

    /// <summary>
    /// Gets whether the column is a header column.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsHeader { get; }

    internal TableColumn(int index, TableModel table) {
        Index = index;
        Table = table;
        IsHeader = index == 0 && table.UseFirstColumnAsHeader;
    }

}