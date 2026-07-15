using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(UploadedFileWithName), "file")]
[JsonKnownType(typeof(UploadedFileWithId), "file_upload")]
[JsonKnownType(typeof(ExternalFileWithName), "external")]
[JsonFallbackType(typeof(UnknownFileObjectWithName))]
public abstract class FileObjectWithName
{
    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
