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
    public required TableModel Table { get; init; }

    /// <summary>
    /// Gets the index of the row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required int Index { get; init; }

    /// <summary>
    /// Gets whether the row is a header row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required bool IsHeader { get; init; }

    /// <summary>
    /// Gets whether the row is a footer row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required bool IsFooter { get; init; }

    /// <summary>
    /// Gets a list of the cells of the row.
    /// </summary>
    public required IReadOnlyList<TableCell> Cells { get; init; }

    /// <summary>
    /// Initializes a new instance of <see cref="TableRow"/> from the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the row.</param>
    public TableRow(JObject json) : base(json) { }

}