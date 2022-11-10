using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;

namespace Limbo.Umbraco.Tables.Models {

    /// <summary>
    /// Class representing a row in a <see cref="TablesDataModel"/> value.
    /// </summary>
    public class TablesDataRow : JsonObjectBase {

        /// <summary>
        /// Gets the index of the row.
        /// </summary>
        [JsonIgnore]
        public int Index { get; }

        internal TablesDataRow(int index, JObject json) : base(json) {
            Index = index;
        }

    }

}