using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FilesProperty : Property
{
    public override PropertyType Type => PropertyType.Files;

    [JsonPropertyName("files")]
    public Dictionary<string, object> Files { get; set; }
}
