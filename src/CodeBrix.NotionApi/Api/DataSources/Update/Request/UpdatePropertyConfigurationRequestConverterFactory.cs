using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

/// <summary>
/// Converter factory that creates the appropriate generic converter for UpdatePropertyConfigurationRequest.
/// This solves the issue with open generic types in JsonConverter attributes.
/// </summary>
public class UpdatePropertyConfigurationRequestConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert == typeof(IUpdatePropertyConfigurationRequest))
        {
            return true;
        }

        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(UpdatePropertyConfigurationRequest<>) &&
               typeof(DataSourcePropertyConfigRequest).IsAssignableFrom(typeToConvert.GetGenericArguments()[0]);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(IUpdatePropertyConfigurationRequest))
        {
            // Values declared as the marker interface serialize via their runtime type, which
            // routes back into this factory's generic branch below.
            return new RuntimeTypeConverter<IUpdatePropertyConfigurationRequest>();
        }

        var genericArgument = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(UpdatePropertyConfigurationRequestConverter<>).MakeGenericType(genericArgument);

        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}
