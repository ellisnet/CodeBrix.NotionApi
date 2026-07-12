using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class FileImportSuccessResult : FileImportResult
{
    public override string Type => "success";

    [JsonPropertyName("success")]
    public Dictionary<string, object> Success { get; set; }
}
