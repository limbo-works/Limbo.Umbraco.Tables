using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Limbo.Umbraco.Tables.Models {

    /// <summary>
    /// Class representing a column in a <see cref="TablesDataModel"/> value.
    /// </summary>
    public class TablesDataColumn : TablesDataObject {

        /// <summary>
        /// Gets a reference to the parent <see cref="TablesDataModel"/>.
        /// </summary>
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TablesDataModel Table { get; }

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

        internal TablesDataColumn(int index, JObject json, TablesDataModel table) : base(json) {
            Index = index;
            Table = table;
            IsHeader = index == 0 && table.UseFirstColumnAsHeader;
        }

    }

}