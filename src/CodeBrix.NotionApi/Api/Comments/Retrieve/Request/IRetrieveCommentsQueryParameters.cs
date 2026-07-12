using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IRetrieveCommentsQueryParameters : IPaginationParameters
{
    [JsonPropertyName("block_id")]
    string BlockId { get; set; }
}
