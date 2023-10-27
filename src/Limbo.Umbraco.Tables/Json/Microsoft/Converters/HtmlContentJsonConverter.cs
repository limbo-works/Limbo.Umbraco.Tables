using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Html;

namespace Limbo.Umbraco.Tables.Json.Microsoft.Converters;

internal class HtmlContentJsonConverter : JsonConverter<IHtmlContent> {

    /// <inheritdoc />
    public override IHtmlContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return reader.TokenType switch {
            JsonTokenType.Null => null,
            JsonTokenType.String => new HtmlString(reader.GetString()),
            _ => throw new Exception($"Unsupported token type: {reader.TokenType}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IHtmlContent? value, JsonSerializerOptions options) {

        if (value is null) {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.ToString());

    }

}