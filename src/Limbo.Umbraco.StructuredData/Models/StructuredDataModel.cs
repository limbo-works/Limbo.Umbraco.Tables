using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.StructuredData.Parsers;
using Limbo.Umbraco.StructuredData.PropertyEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Limbo.Umbraco.StructuredData.Models {

    /// <summary>
    /// Class representing the value of a <see cref="StructuredDataEditor"/>.
    /// </summary>
    public class StructuredDataModel : JsonObjectBase {

        #region Properties

        /// <summary>
        /// Gets whether the first row of the table should be used as a header.
        /// </summary>
        [JsonProperty("useFirstRowAsHeader")]
        public bool UseFirstRowAsHeader { get; }

        /// <summary>
        /// Gets whether the first column of the table should be used as a header.
        /// </summary>
        [JsonProperty("useFirstColumnAsHeader")]
        public bool UseFirstColumnAsHeader { get; }

        /// <summary>
        /// Gets a list of the rows in the structued data table.
        /// </summary>
        [JsonProperty("rows")]
        public IReadOnlyList<StructuredDataRow> Rows { get; }

        /// <summary>
        /// Gets a list of the columns in the structued data table.
        /// </summary>
        [JsonProperty("columns")]
        public IReadOnlyList<StructuredDataColumn> Columns { get; }

        /// <summary>
        /// Gets a list of the cells in the structued data table.
        /// </summary>
        [JsonProperty("cells")]
        public IReadOnlyList<IReadOnlyList<StructuredDataCell>> Cells { get; }

        #endregion

        #region Constructors

        private StructuredDataModel(JObject json, StructuredDataHtmlParser htmlParser, bool preview) : base(json) {

            UseFirstRowAsHeader = json.GetBoolean("useFirstRowAsHeader");
            UseFirstColumnAsHeader = json.GetBoolean("useFirstColumnAsHeader");

            Rows = json.GetArrayOrNew("rows")
                .ForEach((i, x) => new StructuredDataRow(i, x))
                .ToList();

            Columns = json.GetArrayOrNew("columns")
                .ForEach((i, x) => new StructuredDataColumn(i, x))
                .ToList();

            Cells = json
                .GetArrayOrNew("cells")
                .ForEach((i, x) => ParseCellRow(i, x, htmlParser, preview))
                .ToList();

        }

        #endregion

        #region Member methods

        private List<StructuredDataCell> ParseCellRow(int index, JArray array, StructuredDataHtmlParser htmlParser, bool preview) {

            StructuredDataRow row = Rows[index];

            List<StructuredDataCell> temp = new();

            for (int c = 0; c < array.Count; c++) {

                int columnIndex = c;
                StructuredDataColumn column = Columns[columnIndex];

                temp.Add(array.GetObject(c, x => new StructuredDataCell(x, index, row, columnIndex, column, htmlParser, preview)));

            }

            return temp;

        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a new instance of <see cref="StructuredDataModel"/> parsed from the specified <paramref name="json"/> object, or <c>null</c> if <paramref name="json"/> is null.
        /// </summary>
        /// <param name="json">The JSON object.</param>
        /// <param name="htmlParser">An instance of <see cref="StructuredDataHtmlParser"/> to be used for parsing HTML values.</param>
        /// <param name="preview">Whether the model is part of a page being viewed in preview mode.</param>
        /// <returns>An instance of <see cref="StructuredDataModel"/>, or <c>null</c> if <paramref name="json"/> is null.</returns>
        public static StructuredDataModel Parse(JObject json, StructuredDataHtmlParser htmlParser, bool preview) {
            return json == null ? null : new StructuredDataModel(json, htmlParser, preview);
        }

        #endregion

    }

}