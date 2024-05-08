﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Limbo.Umbraco.Tables.Parsers;
using Limbo.Umbraco.Tables.PropertyEditors;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;
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
    public bool UseFirstRowAsHeader { get; }

    /// <summary>
    /// Gets whether the first column of the table should be used as a header.
    /// </summary>
    [JsonProperty("useFirstColumnAsHeader")]
    [JsonPropertyName("useFirstColumnAsHeader")]
    public bool UseFirstColumnAsHeader { get; }

    /// <summary>
    /// Gets whether the last row of the table should be used as a footer.
    /// </summary>
    [JsonProperty("useLastRowAsFooter")]
    [JsonPropertyName("useLastRowAsFooter")]
    public bool UseLastRowAsFooter { get; }

    /// <summary>
    /// Gets a list of the rows in the structued data table.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public IReadOnlyList<TableRow> Rows { get; }

    /// <summary>
    /// Gets a list of the columns in the structued data table.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public IReadOnlyList<TableColumn> Columns { get; }

    /// <summary>
    /// Gets a list of the cells in the structued data table.
    /// </summary>
    [JsonProperty("cells")]
    [JsonPropertyName("cells")]
    public IReadOnlyList<IReadOnlyList<TableCell>> Cells { get; }

    #endregion

    #region Constructors

    private TableModel(JObject json, TableConfiguration config, TablesHtmlParser htmlParser, bool preview) : base(json) {

        UseFirstRowAsHeader = json.GetBoolean("useFirstRowAsHeader") && config.AllowUseFirstRowAsHeader;
        UseFirstColumnAsHeader = json.GetBoolean("useFirstColumnAsHeader") && config.AllowUseFirstColumnAsHeader;
        UseLastRowAsFooter = json.GetBoolean("useLastRowAsFooter") && config.AllowUseLastRowAsFooter;

        JArray? jsonCells = json.GetArray("cells");

        TableRow[] rows = Array.Empty<TableRow>();
        TableColumn[]? columns = null;
        TableCell[][] cells = Array.Empty<TableCell[]>();

        if (jsonCells is not null) {

            int rowCount = jsonCells.Count;

            rows = new TableRow[rowCount];

            cells = new TableCell[rowCount][];

            int rowIndex = 0;

            // ReSharper disable PossibleInvalidCastExceptionInForeachLoop

            foreach (JArray array in jsonCells) {

                int columnCount = array.Count;

                rows[rowIndex] = new TableRow(rowIndex, rowCount, this);

                cells[rowIndex] = new TableCell[columnCount];

                int columnIndex = 0;

                foreach (JObject jsonCell in array) {

                    columns ??= new TableColumn[columnCount];

                    columns[columnIndex] = new TableColumn(columnIndex, this);

                    cells[rowIndex][columnIndex] = new TableCell(jsonCell, rowIndex, rowCount, columnIndex, this, htmlParser, preview);

                    columnIndex++;

                }

                rowIndex++;

            }

            // ReSharper restore PossibleInvalidCastExceptionInForeachLoop

        }

        Rows = rows;
        Columns = columns ?? Array.Empty<TableColumn>();
        Cells = cells;

    }

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

        foreach (var cell in row.Table.Cells[row.Index]) {

            writer.Write($"      <{cell.Type.ToLower()}");
            if (cell.Scope is not TableCellScope.None) writer.Write($" scope=\"{cell.Scope.ToLower()}\"");
            writer.WriteLine(">");

            writer.WriteLine($"        {cell.Value}");

            writer.WriteLine($"      </{cell.Type.ToLower()}>");

        }

        writer.WriteLine("    </tr>");

    }

    #endregion

    #region Static methods

    /// <summary>
    /// Returns a new instance of <see cref="TableModel"/> parsed from the specified <paramref name="json"/> object, or <c>null</c> if <paramref name="json"/> is null.
    /// </summary>
    /// <param name="json">The JSON object.</param>
    /// <param name="config">The table configuration.</param>
    /// <param name="htmlParser">An instance of <see cref="TablesHtmlParser"/> to be used for parsing HTML values.</param>
    /// <param name="preview">Whether the model is part of a page being viewed in preview mode.</param>
    /// <returns>An instance of <see cref="TableModel"/>, or <c>null</c> if <paramref name="json"/> is null.</returns>
    [return: NotNullIfNotNull("json")]
    public static TableModel? Parse(JObject? json, TableConfiguration config, TablesHtmlParser htmlParser, bool preview) {
        return json == null ? null : new TableModel(json, config, htmlParser, preview);
    }

    #endregion

}