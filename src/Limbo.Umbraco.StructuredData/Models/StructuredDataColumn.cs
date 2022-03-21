using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;

namespace Limbo.Umbraco.StructuredData.Models {

    /// <summary>
    /// Class representing a column in a <see cref="StructuredDataModel"/> value.
    /// </summary>
    public class StructuredDataColumn : JsonObjectBase {

        /// <summary>
        /// Gets the column index.
        /// </summary>
        [JsonIgnore]
        public int Index { get; }

        internal StructuredDataColumn(int index, JObject json) : base(json) {
            Index = index;
        }

    }

}