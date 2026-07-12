using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FilePageCover : IPageCover
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "file";

    [JsonPropertyName("file")]
    public InternalFileInfo File { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
