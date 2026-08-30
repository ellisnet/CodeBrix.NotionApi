using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class RetrievePageAsMarkdownRequest :
    IRetrievePageAsMarkdownPathParameters,
    IRetrievePageAsMarkdownQueryParameters
{
    [JsonPropertyName("page_id")]
    public string PageId { get; set; }

    [JsonPropertyName("include_transcript")]
    public bool IncludeTranscript { get; set; }
}
