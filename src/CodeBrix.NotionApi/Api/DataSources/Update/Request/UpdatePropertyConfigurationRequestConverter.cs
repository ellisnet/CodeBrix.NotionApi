using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

// write custom json convert for UpdatePropertyConfigurationRequest to flatten PropertyRequest into parent object
public class UpdatePropertyConfigurationRequestConverter<T> : JsonConverter<UpdatePropertyConfigurationRequest<T>> where T : DataSourcePropertyConfigRequest
{
    public override void Write(Utf8JsonWriter writer, UpdatePropertyConfigurationRequest<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Name != null)
        {
            writer.WritePropertyName("name");
            writer.WriteStringValue(value.Name);
        }

        if (value.PropertyRequest != null)
        {
            using var propertyRequestJson = JsonSerializer.SerializeToDocument<object>(value.PropertyRequest, options);

            foreach (var property in propertyRequestJson.RootElement.EnumerateObject())
            {
                property.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }

    public override UpdatePropertyConfigurationRequest<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization is not implemented for UpdatePropertyConfigurationRequest.");
    }
}
