using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ToDoBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("to_do")]
    public Info ToDo { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.ToDo;

    public class Info
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("checked")]
        public bool IsChecked { get; set; }

        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<INonColumnBlock> Children { get; set; }
    }
}
