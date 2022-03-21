using Limbo.Umbraco.StructuredData.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Umbraco.StructuredData.Models {

    /// <summary>
    /// Class representing a cell in a <see cref="StructuredDataModel"/> value.
    /// </summary>
    public class StructuredDataCell : JsonObjectBase {

        /// <summary>
        /// Gets the row index.
        /// </summary>
        [JsonProperty("rowIndex")]
        public int RowIndex { get; }

        /// <summary>
        /// Gets a reference to the row.
        /// </summary>
        [JsonIgnore]
        public StructuredDataRow Row { get; }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        [JsonProperty("columnIndex")]
        public int ColumnIndex { get; }

        /// <summary>
        /// Gets a reference to the column.
        /// </summary>
        [JsonIgnore]
        public StructuredDataColumn Column { get; }

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
        public string Scope { get; }

        internal StructuredDataCell(JObject json, int rowIndex, StructuredDataRow row, int columnIndex, StructuredDataColumn column, StructuredDataHtmlParser htmlParser, bool preview) : base(json) {
            RowIndex = rowIndex;
            Row = row;
            ColumnIndex = columnIndex;
            Column = column;
            Value = json.GetString("value", x => htmlParser.Parse(x, preview));
        }

    }

}