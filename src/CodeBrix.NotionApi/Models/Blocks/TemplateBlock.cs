using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TemplateBlock : Block, IColumnChildrenBlock, INonColumnBlock
{
    [JsonPropertyName("template")]
    public Data Template { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Template;

    public class Data
    {
        [JsonPropertyName("rich_text")]
        public IEnumerable<RichTextBase> RichText { get; set; }

        [JsonPropertyName("children")]
        public IEnumerable<ITemplateChildrenBlock> Children { get; set; }
    }
}
