using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.Tables.Models;
using Limbo.Umbraco.Tables.PropertyEditors;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Newtonsoft;
using Skybrud.Essentials.Json.Newtonsoft.Extensions;

namespace Limbo.Umbraco.Tables.Parsers;

/// <summary>
/// Parses table JSON payloads into a <see cref="TableModel"/>, including columns, rows, and cells.
/// </summary>
/// <remarks>Supports current and legacy table payload shapes, applies header and footer behavior based on JSON
/// flags and <see cref="TableConfiguration"/>, and delegates cell HTML parsing to <see
/// cref="TablesHtmlParser"/>.</remarks>
public class TablesJsonParser {

    // TODO: should this class be backed by an interface?

    /// <summary>
    /// Gets the parser used to process HTML table content.
    /// </summary>
    public TablesHtmlParser HtmlParser { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TablesJsonParser"/> class with the specified HTML parser.
    /// </summary>
    /// <param name="htmlParser">HTML parser used to extract table data.</param>
    public TablesJsonParser(TablesHtmlParser htmlParser) {
        HtmlParser = htmlParser;
    }

    /// <summary>
    /// Parses a JSON string into a <see cref="TableModel"/> using the specified table configuration and preview mode.
    /// </summary>
    /// <param name="json">The JSON content to parse.</param>
    /// <param name="config">The parsing configuration. If null, a default configuration is used.</param>
    /// <param name="preview"><see langword="true"/> to parse in preview mode; otherwise, <see langword="false"/>.</param>
    /// <returns>A <see cref="TableModel"/> created from the parsed JSON content.</returns>
    public TableModel Parse(string json, TableConfiguration? config = null, bool preview = false) {
        JObject obj = JsonUtils.ParseJsonObject(json);
        return Parse(obj, config, preview);
    }

    /// <summary>
    /// Parses a JSON object into a <see cref="TableModel"/> using the specified table configuration and preview mode.
    /// </summary>
    /// <param name="json">The JSON object to parse.</param>
    /// <param name="config">The parsing configuration. If null, a default configuration is used.</param>
    /// <param name="preview"><see langword="true"/> to parse in preview mode; otherwise, <see langword="false"/>.</param>
    /// <returns>A <see cref="TableModel"/> created from the parsed JSON object.</returns>
    public virtual TableModel Parse(JObject json, TableConfiguration? config = null, bool preview = false) {

        config ??= new TableConfiguration();

        List<TableColumn> columns = [];
        List<TableRow> rows = [];

        // Initialize the table model with the parsed columns and rows, and apply header/footer flags based on the JSON and configuration
        TableModel table = new(json) {
            UseFirstRowAsHeader = json.GetBoolean("useFirstRowAsHeader") && config.AllowUseFirstRowAsHeader,
            UseFirstColumnAsHeader = json.GetBoolean("useFirstColumnAsHeader") && config.AllowUseFirstColumnAsHeader,
            UseLastRowAsFooter = json.GetBoolean("useLastRowAsFooter") && config.AllowUseLastRowAsFooter,
            Columns = columns,
            Rows = rows
        };

        // Parse the columns
        int c = 0;
        foreach (JObject column in json.GetObjectArray("columns")) {
            columns.Add(ParseColumn(c, column, table));
            c++;
        }

        // If the JSON contains a legacy "cells" array, parse it and populate the table model accordingly
        if (TryParseLegacyCellsArray(json, table, config, preview)) return table;

        // Parse the rows
        int r = 0;
        JObject[] array = json.GetObjectArray("rows");
        foreach (JObject row in array) {
            rows.Add(ParseRow(r, row, array.Length, table, config, preview));
            r++;
        }

        // Return the fully populated table model
        return table;

    }


    /// <summary>
    /// Parses a table column from a JSON object and its position within a table model.
    /// </summary>
    /// <param name="index">Zero-based position of the column in the table.</param>
    /// <param name="json">JSON object that contains the column data.</param>
    /// <param name="table">Table model that owns the column.</param>
    /// <returns>A new <see cref="TableColumn"/> instance initialized from the specified index, JSON object, and table model.</returns>
    public virtual TableColumn ParseColumn(int index, JObject json, TableModel table) {
        return new TableColumn(index, json, table);
    }

    /// <summary>
    /// Creates a <see cref="TableRow"/> from a JSON row payload, sets row metadata, and parses its cells.
    /// </summary>
    /// <remarks>A row is marked as header when it is the first row and <c>UseFirstRowAsHeader</c> is enabled.
    /// A row is marked as footer when it is the last row, not a header, and <c>UseLastRowAsFooter</c> is
    /// enabled.</remarks>
    /// <param name="index">Zero-based position of the row in the source sequence.</param>
    /// <param name="json">JSON object that contains the row data, including the <c>cells</c> array.</param>
    /// <param name="rowsCount">Total number of rows in the source sequence.</param>
    /// <param name="table">Table model that owns the row and provides header and footer configuration.</param>
    /// <param name="config">Configuration used while parsing cells.</param>
    /// <param name="preview">Indicates whether parsing is performed in preview mode.</param>
    /// <returns>A populated <see cref="TableRow"/> associated with <paramref name="table"/>, including parsed cells and header
    /// or footer flags when applicable.</returns>
    public virtual TableRow ParseRow(int index, JObject json, int rowsCount, TableModel table, TableConfiguration config, bool preview) {

        bool isHeader = index == 0 && table.UseFirstRowAsHeader;
        bool isFooter = !isHeader && index == rowsCount - 1 && table.UseLastRowAsFooter;

        List<TableCell> cells = [];

        TableRow row = new(json) {
            Table = table,
            Index = index,
            IsHeader = isHeader,
            IsFooter = isFooter,
            Cells = cells
        };

        int cellIndex = 0;
        foreach (JObject cell in json.GetObjectArray("cells")) {
            cells.Add(ParseCell(cellIndex, cell, row, config, preview));
            cellIndex++;
        }

        return row;

    }

    /// <summary>
    /// Attempts to parse a legacy cells array from the specified JSON source and populate the table model.
    /// </summary>
    /// <param name="source">The JSON object that contains the legacy cells array.</param>
    /// <param name="table">The table model to populate.</param>
    /// <param name="config">The table configuration.</param>
    /// <param name="preview">Indicates whether parsing is performed in preview mode.</param>
    /// <returns><see langword="true"/>> if the legacy cells array was successfully parsed; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Prior to Umbraco 17, the package would save both a "rows" and a "cells" property. If we detect the latter, we
    /// need to treat the data as legacy, and convert it as such.
    /// </remarks>
    protected bool TryParseLegacyCellsArray(JObject source , TableModel table, TableConfiguration config, bool preview) {

        // Return right away if "cells" is not present, as this indicates that the data is not in legacy format.
        if (GetLegacyCellsArray(source) is not { } cells) return false;

        // Get the "rows" array from the source JSON, which contains metadata for each row
        JObject[] rowsArray = source.GetObjectArray("rows");

        int rowIndex = 0;
        foreach (JObject[] array in cells) {

            bool isHeader = rowIndex == 0 && table.UseFirstRowAsHeader;
            bool isFooter = !isHeader && rowIndex == rowsArray.Length - 1 && table.UseLastRowAsFooter;

            List<TableCell> rowCells = [];

            TableRow row = new(rowsArray[rowIndex]) {
                Table = table,
                Index = rowIndex,
                IsHeader = isHeader,
                IsFooter = isFooter,
                Cells = rowCells
            };

            int cellIndex = 0;
            foreach (JObject cell in array) {
                rowCells.Add(ParseCell(cellIndex, cell, row, config, preview));
                cellIndex++;
            }

            rowIndex++;

        }

        return true;

    }

    /// <summary>
    /// Parses and returns a single <see cref="TableCell"/> from the specified parameters.
    /// </summary>
    /// <param name="cellIndex">Zero-based position of the cell within the row.</param>
    /// <param name="cell">The JSON object representing the cell.</param>
    /// <param name="row">The row to which the cell belongs.</param>
    /// <param name="config">The table configuration.</param>
    /// <param name="preview">Indicates whether the cell is being parsed in preview mode.</param>
    /// <returns>The parsed <see cref="TableCell"/>.</returns>
    public virtual TableCell ParseCell(int cellIndex, JObject cell, TableRow row, TableConfiguration config, bool preview) {

        TableColumn column = row.Table.Columns[cellIndex];

        TableCellScope scope = TableCellScope.None;
        if (row is { Index: 0, Table.UseFirstRowAsHeader: true }) {
            scope = TableCellScope.Col;
        } else if (cellIndex == 0 && row.Table.UseFirstColumnAsHeader) {
            scope = TableCellScope.Row;
        }

        return new TableCell(cell) {
            Row = row,
            RowIndex = row.Index,
            ColumnIndex = cellIndex,
            Column = column,
            Value = new HtmlString(cell.GetString("value", x => HtmlParser.Parse(x, preview))!),
            Type = row.IsHeader || column.IsHeader ? TableCellType.Th : TableCellType.Td,
            Scope = scope
        };

    }

    private static JObject[][]? GetLegacyCellsArray(JObject? json) {
        JArray? cells = json.GetArray("cells");
        return cells?.OfType<JArray>().Select(x => x.OfType<JObject>().ToArray()).ToArray();
    }

}