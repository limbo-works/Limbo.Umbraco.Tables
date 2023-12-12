using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing a row in a <see cref="TableModel"/> value.
/// </summary>
public class TableRow : TableObject {

    /// <summary>
    /// gets a reference to the parent <see cref="TableModel"/>.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public TableModel Table { get; }

    /// <summary>
    /// Gets the index of the row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int Index { get; }

    /// <summary>
    /// Gets whether the row is a header row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsHeader { get; }

    /// <summary>
    /// Gets whether the row is a footer row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsFooter { get; }

    /// <summary>
    /// Gets a list of the cells of the row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public IReadOnlyList<TableCell> Cells => Table.Cells[Index];

    internal TableRow(int index, JObject json, int rowsCount, TableModel table) : base(json) {
        Table = table;
        Index = index;
        IsHeader = index == 0 && table.UseFirstRowAsHeader;
        IsFooter = !IsHeader && index == rowsCount - 1 && table.UseLastRowAsFooter;
    }

}