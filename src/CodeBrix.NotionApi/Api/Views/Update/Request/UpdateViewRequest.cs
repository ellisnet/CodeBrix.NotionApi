using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class UpdateViewRequest
{
    /// <summary>
    /// The ID of the view to update. This is a path parameter and is not serialized in the request body.
    /// </summary>
    [JsonIgnore]
    public string ViewId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("filter")]
    public object Filter { get; set; }

    [JsonPropertyName("sorts")]
    public IEnumerable<UpdateViewSort> Sorts { get; set; }

    [JsonPropertyName("configuration")]
    public ViewConfiguration Configuration { get; set; }
}

public class UpdateViewSort
{
    [JsonPropertyName("property")]
    public string Property { get; set; }

    [JsonPropertyName("direction")]
    public string Direction { get; set; }
}
