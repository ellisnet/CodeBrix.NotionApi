using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ListUsersResponse : PaginatedList<User>
{
    [JsonPropertyName("user")]
    public Dictionary<string, object> User { get; set; }
}
