using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FilePageIcon : IPageIcon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = PageIconTypes.File;

    [JsonPropertyName(PageIconTypes.File)]
    public InternalFileInfo File { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalData { get; set; }
}
