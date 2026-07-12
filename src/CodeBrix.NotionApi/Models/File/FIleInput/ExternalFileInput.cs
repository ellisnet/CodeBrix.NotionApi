using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ExternalFileInput : IFileObjectInput
{
    [JsonPropertyName("external")]
    public Data External { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("caption")]
    public IEnumerable<RichTextBaseInput> Caption { get; set; }

    public class Data
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
