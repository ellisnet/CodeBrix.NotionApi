using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class EmbedUpdateBlock : UpdateBlock
{
    [JsonPropertyName("embed")]
    public Info Embed { get; set; }

    public class Info
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
