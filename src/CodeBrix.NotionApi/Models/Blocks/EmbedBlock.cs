using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmbedBlock : Block, IColumnChildrenBlock, INonColumnBlock
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
