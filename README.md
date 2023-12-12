# Limbo Tables

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/Limbo.Umbraco.Tables.svg)](https://www.nuget.org/packages/Limbo.Umbraco.Tables)
[![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.Tables.svg)](https://www.nuget.org/packages/Limbo.Umbraco.Tables)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.tables)

Table editor for Umbraco 10+.


<br /><br />

## Installation

Install for Umbraco 10+ via [**NuGet**](https://www.nuget.org/packages/Limbo.Umbraco.BlockList/1.1.2). Either via the .NET CLI:

```
dotnet add package Limbo.Umbraco.Tables --version 1.1.2
```

or the NuGet Package Manager:

```
Install-Package Limbo.Umbraco.Tables -Version 1.1.2
```







<br /><br />

## Documentation

The package adds a **Limbo Tables** property that you can use either on a content type or on an element type (eg. in Umbraco's block list).

Properties using this property editor exposes an instance of `Limbo.Umbraco.Tables.Models.TablesDataModel` representing the tabular data. The property is nullable, so if the user hasn't yet entered any data, or all cells are empty, `null` will be returned instead.

The returned offers a number of different properties for accessing and rendering the data - eg. the `Cells` containing a two-dimensional array with the tabular data:

```cshtml
@using Limbo.Umbraco.Tables.Models
@{
    var value = block.Content.Value("data");
    if (value is TableModel data)
    {
        <table>
            @foreach (IReadOnlyList<TableCell> row in data.Cells)
            {
                <tr>
                    @foreach (TableCell cell in row) {
                        @if (cell.Type == TableCellType.Th)
                        {
                            <th>@Html.Raw(cell.Value)</th>
                        }
                        else
                        {
                            <td>@Html.Raw(cell.Value)</td>
                        }
                    }
                </tr>
            }
        </table>
    }
}
```


<br /><br />

## Property Editor

The property editor gives users the ability to create tabular data, with each cell value being a richtext editor on it's own.

The table can be configured to use either the first row or the first column as a header - or both.

![image](https://user-images.githubusercontent.com/3634580/159875238-bc72a39a-311d-423b-a23a-313f6bc2ae44.png)
