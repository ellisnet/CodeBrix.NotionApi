using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmbedBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("embed")]
    public Info Embed { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Embed;

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("caption")]
        public IEnumerable<RichTextBase> Caption { get; set; }
    }
}
