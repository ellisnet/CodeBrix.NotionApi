using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IBlockAppendChildrenBodyParameters
{
    [JsonPropertyName("children")]
    IEnumerable<IBlockObjectRequest> Children { get; set; }

    /// <summary>
    ///     Controls where the new blocks are placed within the parent.
    ///     Supports <see cref="AfterBlockContentPosition"/>, <see cref="StartContentPosition"/>,
    ///     and <see cref="EndContentPosition"/>. Defaults to end when omitted.
    /// </summary>
    [JsonPropertyName("position")]
    ContentPosition Position { get; set; }
}

internal class BlockAppendChildrenBodyParameters : IBlockAppendChildrenBodyParameters
{
    [JsonPropertyName("children")]
    public IEnumerable<IBlockObjectRequest> Children { get; set; }

    [JsonPropertyName("position")]
    public ContentPosition Position { get; set; }

    public BlockAppendChildrenBodyParameters(BlockAppendChildrenRequest request)
    {
        Children = request.Children;
        Position = request.Position;
    }
}
