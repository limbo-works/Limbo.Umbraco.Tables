using Limbo.Umbraco.Tables.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public string Value { get; }

        /// <summary>
        /// Gets a reference to the type of the cell - eg. <c>td</c> or <c>th</c>.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// Gets a reference to the scope of the cell - eg. <c>row</c> or <c>col</c>.
        /// </summary>
        [JsonProperty("scope")]
        public string? Scope { get; }

        internal TablesDataCell(JObject json, int rowIndex, TablesDataRow row, int columnIndex, TablesDataColumn column, TablesHtmlParser htmlParser, bool preview) : base(json) {
            RowIndex = rowIndex;
            Row = row;
            ColumnIndex = columnIndex;
            Column = column;
            Value = json.GetString("value", x => htmlParser.Parse(x, preview))!;
            Type = json.GetString("type")!;
            Scope = json.GetString("scope");
        }

    }

}