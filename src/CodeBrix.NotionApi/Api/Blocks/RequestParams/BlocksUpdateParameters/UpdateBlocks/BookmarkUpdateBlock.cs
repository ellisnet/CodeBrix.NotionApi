using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BookmarkUpdateBlock : IUpdateBlock
{
    [JsonPropertyName("bookmark")]
    public Info Bookmark { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("caption")]
        public IEnumerable<RichTextBaseInput> Caption { get; set; }
    }
}
