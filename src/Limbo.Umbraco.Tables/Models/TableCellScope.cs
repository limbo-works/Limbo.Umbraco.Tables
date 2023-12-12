using Newtonsoft.Json;
using Skybrud.Essentials.Json.Newtonsoft.Converters.Enums;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Enum class representing the scope of a <see cref="TableCell"/>.
/// </summary>
[JsonConverter(typeof(EnumCamelCaseConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Microsoft.Converters.EnumCamelCaseConverter))]
public enum TableCellScope {

    /// <summary>
    /// Indicates that a cell has no scope in particular.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that a cell has the scope of a column.
    /// </summary>
    Col,

    /// <summary>
    /// Indicates that a cell has the scope of a row.
    /// </summary>
    Row

}