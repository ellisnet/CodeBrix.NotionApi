using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LinkPreviewBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("link_preview")]
    public Data LinkPreview { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.LinkPreview;

    public class Data
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
