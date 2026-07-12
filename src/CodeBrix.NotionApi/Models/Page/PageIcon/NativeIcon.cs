using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class NativeIcon
{
    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class NativeIconObject : IPageIcon
{
    [JsonPropertyName("icon")]
    public NativeIcon Icon { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
