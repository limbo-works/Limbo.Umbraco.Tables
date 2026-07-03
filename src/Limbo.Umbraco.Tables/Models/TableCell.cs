using Limbo.Umbraco.Tables.Json.Microsoft.Converters;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Converters;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing a cell in a <see cref="TableModel"/> value.
/// </summary>
public class TableCell : TableObject {

    /// <summary>
    /// Gets a reference to the row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required TableRow Row { get; init; }

    /// <summary>
    /// Gets the row index.
    /// </summary>
    [JsonProperty("rowIndex")]
    [System.Text.Json.Serialization.JsonPropertyName("rowIndex")]
    public required int RowIndex { get; init; }

    /// <summary>
    /// Gets the column index.
    /// </summary>
    [JsonProperty("columnIndex")]
    [System.Text.Json.Serialization.JsonPropertyName("columnIndex")]
    public required int ColumnIndex { get; init; }

    /// <summary>
    /// Gets a reference to the column.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required TableColumn Column { get; init; }

    /// <summary>
    /// Gets a reference to the column value.
    /// </summary>
    [JsonProperty("value")]
    [System.Text.Json.Serialization.JsonPropertyName("value")]
    [JsonConverter(typeof(StringJsonConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(HtmlContentJsonConverter))]
    public required IHtmlContent Value { get; init; }

    /// <summary>
    /// Gets a reference to the type of the cell - e.g. <see cref="TableCellType.Td"/> or <see cref="TableCellType.Th"/>.
    /// </summary>
    [JsonProperty("type")]
    [System.Text.Json.Serialization.JsonPropertyName("type")]
    public required TableCellType Type { get; init; }

    /// <summary>
    /// Gets a reference to the scope of the cell - e.g. <see cref="TableCellScope.Col"/> or <see cref="TableCellScope.Row"/>.
    /// </summary>
    [JsonProperty("scope", DefaultValueHandling = DefaultValueHandling.Ignore)]
    [System.Text.Json.Serialization.JsonPropertyName("scope")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public required TableCellScope Scope { get; init; }


    /// <summary>
    /// Initializes a new instance of <see cref="TableCell"/> from the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the cell.</param>
    public TableCell(JObject json) : base(json) { }

}