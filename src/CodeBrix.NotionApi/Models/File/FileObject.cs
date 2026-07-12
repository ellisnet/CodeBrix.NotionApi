using System.Collections.Generic;
using System.Text.Json.Serialization;
using CodeBrix.JsonPolymorphism;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

[JsonConverter(typeof(FallbackTypeConverterFactory))]
[JsonDiscriminator("type")]
[JsonKnownType(typeof(UploadedFile), "file")]
[JsonKnownType(typeof(NewFileUpload), "file_upload")]
[JsonKnownType(typeof(ExternalFile), "external")]
[JsonFallbackType(typeof(UnknownFileObject))]
public abstract class FileObject
{
    [JsonPropertyName("caption")]
    public IEnumerable<RichTextBase> Caption { get; set; }

    /// <summary>
    /// The name of the file block, as shown in the Notion UI. Note that the UI may auto-append .pdf or other extensions.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public virtual string Type { get; set; }
}
