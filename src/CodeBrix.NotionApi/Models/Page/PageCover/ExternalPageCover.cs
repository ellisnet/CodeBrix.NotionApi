using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ExternalPageCover : IPageCover
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "external";

    [JsonPropertyName("external")]
    public ExternalFileInfo External { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
