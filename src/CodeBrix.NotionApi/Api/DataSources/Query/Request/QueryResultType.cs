using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public enum QueryResultType
{
    Unknown,

    [JsonStringEnumMemberName("page")]
    Page,

    [JsonStringEnumMemberName("data_source")]
    DataSource
}
