using System;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi;

// Newtonsoft.Json serializes the RUNTIME type of every value, so upstream notion-sdk-net could
// declare request properties as abstract bases or interfaces (e.g. Filter, IBlockObjectRequest)
// without any discriminator machinery. System.Text.Json serializes the DECLARED type, which
// would emit "{}" for such properties. This factory restores the Newtonsoft behavior for
// abstract/interface types that carry no [JsonConverter] of their own: writing serializes the
// value's runtime type; reading is unsupported (matching Newtonsoft, which could not
// instantiate such types either). Registered in RestClient.DefaultSerializerOptions.
internal sealed class RuntimeTypeConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => (typeToConvert.IsInterface || typeToConvert.IsAbstract)
           && typeToConvert != typeof(object)
           && !typeof(IEnumerable).IsAssignableFrom(typeToConvert)
           && !Attribute.IsDefined(typeToConvert, typeof(JsonConverterAttribute), inherit: false);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (JsonConverter)Activator.CreateInstance(
            typeof(RuntimeTypeConverter<>).MakeGenericType(typeToConvert));
}

internal sealed class RuntimeTypeConverter<T> : JsonConverter<T>
    where T : class
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => throw new JsonException(
            $"Deserializing '{typeToConvert}' is not supported: the type is not instantiable and " +
            "declares no [JsonDiscriminator] mapping.");

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
