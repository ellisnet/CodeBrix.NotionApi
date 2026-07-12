using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IFileObjectInput
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("caption")]
    public IEnumerable<RichTextBaseInput> Caption { get; set; }
}
