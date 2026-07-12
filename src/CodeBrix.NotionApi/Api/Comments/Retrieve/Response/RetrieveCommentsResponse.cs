using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrieveCommentsResponse : PaginatedList<Comment>
{
    [JsonPropertyName("comment")]
    public Dictionary<string, object> Comment { get; set; }
}
