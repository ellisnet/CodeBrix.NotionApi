using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class ExternalFileWithName : FileObjectWithName
{
    public override string Type => "external";

    [JsonPropertyName("external")]
    public Info External { get; set; }

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
