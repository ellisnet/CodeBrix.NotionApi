using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BookmarkBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("bookmark")]
    public Info Bookmark { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Bookmark;

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("caption")]
        public IEnumerable<RichTextBase> Caption { get; set; }
    }
}
