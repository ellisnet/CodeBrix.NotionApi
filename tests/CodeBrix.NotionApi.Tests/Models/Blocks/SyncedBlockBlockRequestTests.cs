using System.Collections.Generic;
using System.Text.Json;
using CodeBrix.NotionApi;
using Xunit;
using SilverAssertions;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests.Models.Blocks.Request;

public class SyncedBlockBlockRequestTests
{
    [Fact]
    public void Serializes_SyncedFrom_Null_As_Explicit_Null()
    {
        // Arrange
        var request = new SyncedBlockBlockRequest
        {
            SyncedBlock = new SyncedBlockBlockRequest.Data
            {
                SyncedFrom = null,
                Children = new List<ISyncedBlockChildrenRequest>()
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain(@"""synced_from"":null");
    }

    [Fact]
    public void Serializes_SyncedFrom_With_BlockId()
    {
        // Arrange
        var request = new SyncedBlockBlockRequest
        {
            SyncedBlock = new SyncedBlockBlockRequest.Data
            {
                SyncedFrom = new SyncedBlockBlockRequest.Data.SyncedFromBlockId
                {
                    Type = "block_id",
                    BlockId = "abc123"
                },
                Children = new List<ISyncedBlockChildrenRequest>()
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain(@"""synced_from"":{");
        json.Should().Contain(@"""type"":""block_id""");
        json.Should().Contain(@"""block_id"":""abc123""");
    }
}
