using System;
using System.Text.Json.Serialization;
using CodeBrix.Json.Extensions.Polymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(FileImportSuccessResult), "success")]
[JsonKnownType(typeof(FileImportErrorResult), "error")]
[JsonFallbackType(typeof(UnknownFileImportResult))]
public abstract class FileImportResult
{
    [JsonPropertyName("type")]
    public virtual string Type { get; set; }

    [JsonPropertyName("imported_time")]
    public DateTime ImportedTime { get; set; }
}
