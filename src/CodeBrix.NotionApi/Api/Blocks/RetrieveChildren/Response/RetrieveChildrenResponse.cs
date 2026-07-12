using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrieveChildrenResponse : PaginatedList<IBlock>
{
    [JsonPropertyName("block")]
    public Dictionary<string, object> Block { get; set; }
}
