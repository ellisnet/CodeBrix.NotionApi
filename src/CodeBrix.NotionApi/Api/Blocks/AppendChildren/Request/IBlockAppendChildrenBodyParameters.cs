using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IBlockAppendChildrenBodyParameters
{
    [JsonPropertyName("children")]
    IEnumerable<IBlockObjectRequest> Children { get; set; }

    /// <summary>
    ///     The ID of the existing block that the new block should be appended after.
    /// </summary>
    [JsonPropertyName("after")]
    public string After { get; set; }
}

internal class BlockAppendChildrenBodyParameters : IBlockAppendChildrenBodyParameters
{
    [JsonPropertyName("children")]
    public IEnumerable<IBlockObjectRequest> Children { get; set; }

    public string After { get; set; }

    public BlockAppendChildrenBodyParameters(BlockAppendChildrenRequest request)
    {
        Children = request.Children;
        After = request.After;
    }
}
