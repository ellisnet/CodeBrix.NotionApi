using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UniqueIdProperty : Property
{
    public override PropertyType Type => PropertyType.UniqueId;

    [JsonPropertyName("unique_id")]
    public Dictionary<string, object> UniqueId { get; set; }
}
