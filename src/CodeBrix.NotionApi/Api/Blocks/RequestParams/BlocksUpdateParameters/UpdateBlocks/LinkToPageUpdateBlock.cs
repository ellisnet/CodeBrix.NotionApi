using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class LinkToPageUpdateBlock : UpdateBlock
{
    [JsonPropertyName("link_to_page")]
    public ILinkToPage LinkToPage { get; set; }
}
