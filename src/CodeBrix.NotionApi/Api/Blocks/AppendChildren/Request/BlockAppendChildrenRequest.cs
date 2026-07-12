using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public class BlockAppendChildrenRequest : IBlockAppendChildrenBodyParameters, IBlockAppendChildrenPathParameters
{
    [JsonPropertyName("children")]
    public IEnumerable<IBlockObjectRequest> Children { get; set; }

    public string After { get; set; }

    public string BlockId { get; set; }
}
