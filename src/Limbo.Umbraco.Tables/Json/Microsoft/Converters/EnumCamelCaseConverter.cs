using System.Text.Json;

namespace Limbo.Umbraco.Tables.Json.Microsoft.Converters;

internal class EnumCamelCaseConverter : System.Text.Json.Serialization.JsonStringEnumConverter {

    public EnumCamelCaseConverter() : base(JsonNamingPolicy.CamelCase) { }

}