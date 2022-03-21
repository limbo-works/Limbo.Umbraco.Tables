using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;

namespace Limbo.Umbraco.StructuredData.Models {

    /// <summary>
    /// Class representing a row in a <see cref="StructuredDataModel"/> value.
    /// </summary>
    public class StructuredDataRow : JsonObjectBase {

        /// <summary>
        /// Gets the index of the row.
        /// </summary>
        [JsonIgnore]
        public int Index { get; }

        internal StructuredDataRow(int index, JObject json) : base(json) {
            Index = index;
        }

    }

}