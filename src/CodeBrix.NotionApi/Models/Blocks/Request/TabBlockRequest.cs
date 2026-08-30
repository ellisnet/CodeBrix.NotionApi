using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class TabBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.Tab;

    [JsonPropertyName("tab")]
    public Data Tab { get; set; }

    public class Data
    {
        /// <summary>
        /// Paragraph blocks that represent individual tabs. Only paragraph blocks are valid children.
        /// </summary>
        [JsonPropertyName("children")]
        public IEnumerable<ParagraphBlockRequest> Children { get; set; }
    }
}
