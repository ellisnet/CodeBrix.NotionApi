using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileImportErrorResult : FileImportResult
{
    public override string Type => "error";

    [JsonPropertyName("error")]
    public FileImportError Error { get; set; }
}
