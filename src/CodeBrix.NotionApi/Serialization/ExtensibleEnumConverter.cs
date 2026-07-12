using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// A System.Text.Json converter for extensible enum structs.
/// Serializes as a plain string and deserializes any string value —
/// including values unknown at compile time — without throwing.
/// <para>
/// Apply via <c>[JsonConverter(typeof(ExtensibleEnumConverter&lt;T&gt;))]</c> on the struct type itself.
/// Nullable occurrences (<c>T?</c>) are handled automatically by System.Text.Json.
/// </para>
/// </summary>
public class ExtensibleEnumConverter<T> : JsonConverter<T>
    where T : struct
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var value = root.ValueKind == JsonValueKind.String
            ? root.GetString()
            : root.GetRawText();

        return (T)Activator.CreateInstance(typeof(T), value ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString() ?? string.Empty);
    }
}
