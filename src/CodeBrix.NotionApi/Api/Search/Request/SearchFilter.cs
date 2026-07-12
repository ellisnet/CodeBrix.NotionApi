using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class SearchFilter
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SearchObjectType Value { get; set; }

    public string Property => "object";
}
