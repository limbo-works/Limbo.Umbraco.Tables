using Limbo.Umbraco.Tables.Json.Microsoft.Converters;
using Limbo.Umbraco.Tables.Parsers;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Converters;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Umbraco.Tables.Models {

    /// <summary>
    /// Class representing a cell in a <see cref="TablesDataModel"/> value.
    /// </summary>
    public class TablesDataCell : TablesDataObject {

        /// <summary>
        /// Gets the row index.
        /// </summary>
        [JsonProperty("rowIndex")]
        public int RowIndex { get; }

        /// <summary>
        /// Gets a reference to the row.
        /// </summary>
        [JsonIgnore]
        public TablesDataRow Row { get; }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        [JsonProperty("columnIndex")]
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets a reference to the column.
        /// </summary>
        [JsonIgnore]
        public TablesDataColumn Column { get; }

        /// <summary>
        /// Gets a reference to the column value.
        /// </summary>
        [JsonProperty("value")]
        [JsonConverter(typeof(StringJsonConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(HtmlContentJsonConverter))]
        public IHtmlContent Value { get; }

        /// <summary>
        /// Gets a reference to the type of the cell - eg. <see cref="TableCellType.Td"/> or <see cref="TableCellType.Th"/>.
        /// </summary>
        [JsonProperty("type")]
        public TableCellType Type { get; }

        /// <summary>
        /// Gets a reference to the scope of the cell - eg. <c>row</c> or <c>col</c>.
        /// </summary>
        [JsonProperty("scope")]
        public TableCellScope Scope { get; }

        internal TablesDataCell(JObject json, int rowIndex, TablesDataRow row, int columnIndex, TablesDataColumn column, TablesHtmlParser htmlParser, bool preview) : base(json) {
            RowIndex = rowIndex;
            Row = row;
            ColumnIndex = columnIndex;
            Column = column;
            Value = new HtmlString(json.GetString("value", x => htmlParser.Parse(x, preview))!);
            Type = row.IsHeader || column.IsHeader ? TableCellType.Th : TableCellType.Td;
            Scope = json.GetEnum("scope", TableCellScope.None);
        }

    }

}