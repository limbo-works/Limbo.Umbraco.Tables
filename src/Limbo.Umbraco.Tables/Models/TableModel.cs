using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Limbo.Umbraco.Tables.PropertyEditors;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Class representing the value of a <see cref="TableEditor"/>.
/// </summary>
public class TableModel : TableObject, IHtmlContent {

    #region Properties

    /// <summary>
    /// Gets whether the first row of the table should be used as a header.
    /// </summary>
    [JsonProperty("useFirstRowAsHeader")]
    [JsonPropertyName("useFirstRowAsHeader")]
    public bool UseFirstRowAsHeader { get; init; }

    /// <summary>
    /// Gets whether the first column of the table should be used as a header.
    /// </summary>
    [JsonProperty("useFirstColumnAsHeader")]
    [JsonPropertyName("useFirstColumnAsHeader")]
    public bool UseFirstColumnAsHeader { get; init; }

    /// <summary>
    /// Gets whether the last row of the table should be used as a footer.
    /// </summary>
    [JsonProperty("useLastRowAsFooter")]
    [JsonPropertyName("useLastRowAsFooter")]
    public bool UseLastRowAsFooter { get; init; }

    /// <summary>
    /// Gets a list of the columns in the table.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required IReadOnlyList<TableColumn> Columns { get; init; }

    /// <summary>
    /// Gets a list of the rows in the table.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public required IReadOnlyList<TableRow> Rows { get; init; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of <see cref="TableModel"/> from the specified <paramref name="json"/> object.
    /// </summary>
    /// <param name="json">The JSON object representing the table.</param>
    public TableModel(JObject json) : base(json) { }

    #endregion

    #region Member methods

    /// <inheritdoc />
    public void WriteTo(TextWriter writer, HtmlEncoder encoder) {

        writer.WriteLine("<table>");

        int r = 0;

        if (UseFirstRowAsHeader && Rows.Count > 0) {
            writer.WriteLine("  <thead>");
            WriteRow(writer, Rows[0]);
            writer.WriteLine("  </thead>");
            r++;
        }

        int rows = UseLastRowAsFooter ? Rows.Count - 1 : Rows.Count;

        writer.WriteLine("  <tbody>");
        for (; r < rows; r++) {
            WriteRow(writer, Rows[r]);
        }
        writer.WriteLine("  </tbody>");

        if (UseLastRowAsFooter && Rows.Count > 1) {
            writer.WriteLine("  <tfoot>");
            WriteRow(writer, Rows.Last());
            writer.WriteLine("  </tfoot>");
        }

        writer.WriteLine("</table>");

    }

    private void WriteRow(TextWriter writer, TableRow row) {

        writer.WriteLine("    <tr>");

        foreach (var cell in row.Cells) {

            writer.Write($"      <{cell.Type.ToLower()}");
            if (cell.Scope is not TableCellScope.None) writer.Write($" scope=\"{cell.Scope.ToLower()}\"");
            writer.WriteLine(">");

            writer.WriteLine($"        {cell.Value}");

            writer.WriteLine($"      </{cell.Type.ToLower()}>");

        }

        writer.WriteLine("    </tr>");

    }

    #endregion

}