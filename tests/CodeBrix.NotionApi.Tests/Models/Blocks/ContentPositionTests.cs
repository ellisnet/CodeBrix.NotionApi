using System.Collections.Generic;
using System.Text.Json;
using CodeBrix.NotionApi;
using SilverAssertions;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

// The append-children endpoint replaced the flat "after" string with a "position" object in
// Notion API version 2026-03-11.
public class ContentPositionTests
{
    private static string SerializeBody(BlockAppendChildrenRequest request)
    {
        var body = (IBlockAppendChildrenBodyParameters)new BlockAppendChildrenBodyParametersProbe(request);

        return JsonSerializer.Serialize(body, RestClient.DefaultSerializerOptions);
    }

    // BlockAppendChildrenBodyParameters is internal to the library; InternalsVisibleTo makes it
    // reachable here, but going through a tiny named shim keeps the intent obvious.
    private sealed class BlockAppendChildrenBodyParametersProbe : IBlockAppendChildrenBodyParameters
    {
        public BlockAppendChildrenBodyParametersProbe(BlockAppendChildrenRequest request)
        {
            Children = request.Children;
            Position = request.Position;
        }

        public IEnumerable<IBlockObjectRequest> Children { get; set; }

        public ContentPosition Position { get; set; }
    }

    [Fact]
    public void After_block_position_serializes_as_a_position_object()
    {
        // Arrange
        var request = new BlockAppendChildrenRequest
        {
            BlockId = "block-1",
            Children = [],
            Position = new AfterBlockContentPosition
            {
                AfterBlock = new AfterBlockReference { Id = "sibling-1" }
            }
        };

        // Act
        var json = SerializeBody(request);

        // Assert
        json.Should().Contain("\"position\"");
        json.Should().Contain("\"type\":\"after_block\"");
        json.Should().Contain("\"after_block\":{\"id\":\"sibling-1\"}");
        json.Should().NotContain("\"after\":\"");
    }

    [Fact]
    public void Start_position_serializes_its_type_only()
    {
        // Arrange
        var request = new BlockAppendChildrenRequest
        {
            BlockId = "block-1",
            Children = [],
            Position = new StartContentPosition()
        };

        // Act
        var json = SerializeBody(request);

        // Assert
        json.Should().Contain("\"position\":{\"type\":\"start\"}");
    }

    [Fact]
    public void End_position_serializes_its_type_only()
    {
        // Arrange
        var request = new BlockAppendChildrenRequest
        {
            BlockId = "block-1",
            Children = [],
            Position = new EndContentPosition()
        };

        // Act
        var json = SerializeBody(request);

        // Assert
        json.Should().Contain("\"position\":{\"type\":\"end\"}");
    }

    [Fact]
    public void An_omitted_position_is_not_serialized()
    {
        // Arrange
        var request = new BlockAppendChildrenRequest
        {
            BlockId = "block-1",
            Children = []
        };

        // Act
        var json = SerializeBody(request);

        // Assert
        json.Should().NotContain("position");
    }
}
