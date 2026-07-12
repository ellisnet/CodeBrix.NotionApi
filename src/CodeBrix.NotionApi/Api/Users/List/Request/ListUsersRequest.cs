using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client.List.Request;

public interface IListUsersQueryParameters : IPaginationParameters
{
}

public class ListUsersRequest : IListUsersQueryParameters
{
    [JsonPropertyName("start_cursor")]
    public string StartCursor { get; set; }

    [JsonPropertyName("page_size")]
    public int? PageSize { get; set; }
}
