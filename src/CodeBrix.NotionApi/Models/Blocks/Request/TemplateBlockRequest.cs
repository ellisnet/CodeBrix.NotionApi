using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TemplateBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
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
        public IEnumerable<ITemplateChildrenBlockRequest> Children { get; set; }
    }
}
