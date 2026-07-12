using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public enum SearchObjectType
{
    [JsonStringEnumMemberName("page")]
    Page,

    [JsonStringEnumMemberName("data_source")]
    DataSource
}
