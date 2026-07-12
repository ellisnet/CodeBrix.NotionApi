using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BulletedListItemUpdateBlock : UpdateBlock
{
    [JsonPropertyName("bulleted_list_item")]
    public Info BulletedListItem { get; set; }

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBaseInput> RichText { get; set; }
    }
}
