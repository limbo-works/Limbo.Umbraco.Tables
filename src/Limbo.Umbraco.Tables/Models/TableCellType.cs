using Newtonsoft.Json;
using Skybrud.Essentials.Json.Newtonsoft.Converters.Enums;

namespace Limbo.Umbraco.Tables.Models;

/// <summary>
/// Enum class describing the type of a <see cref="TableCell"/>.
/// </summary>
[JsonConverter(typeof(EnumCamelCaseConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(Json.Microsoft.Converters.EnumCamelCaseConverter))]
public enum TableCellType {

    /// <summary>
    /// Indicates that a cell is a data cell (aka <c>td</c>).
    /// </summary>
    Td,

    /// <summary>
    /// Indicates that a cell is a header cell (aka <c>th</c>).
    /// </summary>
    Th

}