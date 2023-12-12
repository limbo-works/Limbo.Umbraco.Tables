using Limbo.Umbraco.Tables.Json.Microsoft.Converters;
using Limbo.Umbraco.Tables.Parsers;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Converters;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing a cell in a <see cref="TableModel"/> value.
/// </summary>
public class TableCell : TableObject {

    /// <summary>
    /// Gets the row index.
    /// </summary>
    [JsonProperty("rowIndex")]
    [System.Text.Json.Serialization.JsonPropertyName("rowIndex")]
    public int RowIndex { get; }

    /// <summary>
    /// Gets a reference to the row.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public TableRow Row { get; }

    /// <summary>
    /// Gets the column index.
    /// </summary>
    [JsonProperty("columnIndex")]
    [System.Text.Json.Serialization.JsonPropertyName("columnIndex")]
    public int ColumnIndex { get; }

    /// <summary>
    /// Gets a reference to the column.
    /// </summary>
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public TableColumn Column { get; }

    /// <summary>
    /// Gets a reference to the column value.
    /// </summary>
    [JsonProperty("value")]
    [System.Text.Json.Serialization.JsonPropertyName("value")]
    [JsonConverter(typeof(StringJsonConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(HtmlContentJsonConverter))]
    public IHtmlContent Value { get; }

    /// <summary>
    /// Gets a reference to the type of the cell - eg. <see cref="TableCellType.Td"/> or <see cref="TableCellType.Th"/>.
    /// </summary>
    [JsonProperty("type")]
    [System.Text.Json.Serialization.JsonPropertyName("type")]
    public TableCellType Type { get; }

    /// <summary>
    /// Gets a reference to the scope of the cell - eg. <see cref="TableCellScope.Col"/> or <see cref="TableCellScope.Row"/>.
    /// </summary>
    [JsonProperty("scope", DefaultValueHandling = DefaultValueHandling.Ignore)]
    [System.Text.Json.Serialization.JsonPropertyName("scope")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public TableCellScope Scope { get; }

    internal TableCell(JObject json, int rowIndex, TableRow row, int columnIndex, TableColumn column, TableModel model, TablesHtmlParser htmlParser, bool preview) : base(json) {
        RowIndex = rowIndex;
        Row = row;
        ColumnIndex = columnIndex;
        Column = column;
        Value = new HtmlString(json.GetString("value", x => htmlParser.Parse(x, preview))!);
        Type = row.IsHeader || column.IsHeader ? TableCellType.Th : TableCellType.Td;

        if (RowIndex == 0 && model.UseFirstRowAsHeader) {
            Scope = TableCellScope.Col;
        } else if (ColumnIndex == 0 && model.UseFirstColumnAsHeader) {
            Scope = TableCellScope.Row;
        }

    }

}