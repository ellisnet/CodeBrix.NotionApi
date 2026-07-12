using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FilesPropertyItem : SimplePropertyItem
{
    [JsonPropertyName("type")]
    public override string Type => "files";

    [JsonPropertyName("files")]
    public IEnumerable<FileObjectWithName> Files { get; set; }
}
