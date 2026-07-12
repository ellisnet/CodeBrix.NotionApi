using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BulletedListItemBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("bulleted_list_item")]
    public Info BulletedListItem { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.BulletedListItem;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<INonColumnBlock> Children { get; set; }
    }
}
