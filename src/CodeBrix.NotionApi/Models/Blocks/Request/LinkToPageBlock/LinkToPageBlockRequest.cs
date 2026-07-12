using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LinkToPageBlockRequest : BlockObjectRequest, IColumnChildrenBlockRequest, INonColumnBlockRequest
{
    [JsonPropertyName("link_to_page")]
    public ILinkToPage LinkToPage { get; set; }

    [JsonPropertyName("type")]
    public override BlockType Type => BlockType.LinkToPage;
}
