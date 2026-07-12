using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ExternalPageIcon : IPageIcon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageIconTypes.External;

    [JsonPropertyName(PageIconTypes.External)]
    public ExternalFileInfo External { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
