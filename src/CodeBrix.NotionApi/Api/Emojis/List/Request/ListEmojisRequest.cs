using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListEmojisRequest : IListEmojisQueryParameters
{
    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
