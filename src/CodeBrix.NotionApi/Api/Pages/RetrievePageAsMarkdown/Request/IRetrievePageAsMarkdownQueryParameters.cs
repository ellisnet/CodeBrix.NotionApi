using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: (global namespace);

public interface IRetrievePageAsMarkdownQueryParameters
{
    /// <summary>
    /// Whether to include meeting note transcripts. Defaults to false.
    /// When true, full transcripts are included;
    /// when false, a placeholder with the meeting note URL is shown instead.
    /// </summary>
    [JsonPropertyName("include_transcript")]
    bool IncludeTranscript { get; set; }
}
