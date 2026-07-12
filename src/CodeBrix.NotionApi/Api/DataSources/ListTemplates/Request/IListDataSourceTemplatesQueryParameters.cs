using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IListDataSourceTemplatesQueryParameters : IPaginationParameters
{
    /// <summary>
    /// Filter templates by name (case-insensitive substring match).
    /// </summary>
    [JsonPropertyName("name")]
    string Name { get; set; }
}
