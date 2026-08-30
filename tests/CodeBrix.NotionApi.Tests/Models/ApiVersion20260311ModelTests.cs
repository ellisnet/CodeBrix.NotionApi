using System.Linq;
using System.Text.Json;
using CodeBrix.NotionApi;
using SilverAssertions;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

// Model-level coverage for the non-block additions that came with Notion API version 2026-03-11.
public class ApiVersion20260311ModelTests
{
    [Fact]
    public void Default_notion_version_is_2026_03_11()
    {
        // Assert - the constant every request's Notion-Version header is built from
        Constants.DefaultNotionVersion.Should().Be("2026-03-11");
    }

    [Fact]
    public void Page_carries_is_locked()
    {
        // Arrange
        const string Json = """
            { "object": "page", "id": "page-1", "in_trash": false, "is_locked": true }
            """;

        // Act
        var page = JsonSerializer.Deserialize<Page>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        page.IsLocked.Should().BeTrue();
        page.InTrash.Should().BeFalse();
    }

    [Fact]
    public void Page_without_is_locked_leaves_it_null()
    {
        // Arrange
        const string Json = """{ "object": "page", "id": "page-1" }""";

        // Act
        var page = JsonSerializer.Deserialize<Page>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        page.IsLocked.Should().BeNull();
    }

    [Fact]
    public void FileUpload_carries_in_trash()
    {
        // Arrange
        const string Json = """
            { "object": "file_upload", "id": "upload-1", "in_trash": true, "status": "uploaded" }
            """;

        // Act
        var upload = JsonSerializer.Deserialize<FileUpload>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        upload.InTrash.Should().BeTrue();
    }

    [Fact]
    public void Place_property_item_deserializes_through_the_discriminator()
    {
        // Arrange
        const string Json = """
            {
              "object": "property_item",
              "id": "prop-1",
              "type": "place",
              "place": {
                "lat": 44.0521,
                "lon": -123.0868,
                "name": "Eugene",
                "address": "Eugene, OR"
              }
            }
            """;

        // Act
        var item = JsonSerializer.Deserialize<IPropertyItemObject>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        var place = Assert.IsType<PlacePropertyItem>(item);
        place.Type.Should().Be("place");
        place.Place.Name.Should().Be("Eugene");
        place.Place.Lat.Should().Be(44.0521);
        place.Place.Lon.Should().Be(-123.0868);
    }

    [Fact]
    public void Status_property_config_deserializes_into_typed_options_and_groups()
    {
        // Arrange
        const string Json = """
            {
              "id": "prop-1",
              "name": "Status",
              "type": "status",
              "status": {
                "options": [
                  { "id": "opt-1", "name": "Not started", "color": "default" },
                  { "id": "opt-2", "name": "In progress", "color": "blue" }
                ],
                "groups": [
                  { "id": "grp-1", "name": "To-do", "color": "gray", "option_ids": [ "opt-1" ] }
                ]
              }
            }
            """;

        // Act
        var config = JsonSerializer.Deserialize<DataSourcePropertyConfig>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        var status = Assert.IsType<StatusDataSourcePropertyConfig>(config);
        status.Status.Options.Should().HaveCount(2);
        status.Status.Options.Last().Name.Should().Be("In progress");
        status.Status.Options.Last().Color.Should().Be(Color.Blue);
        status.Status.Groups.Single().OptionIds.Single().Should().Be("opt-1");
    }

    [Fact]
    public void Verification_property_value_deserializes_state_as_a_typed_status()
    {
        // Arrange
        const string Json = """
            {
              "id": "prop-1",
              "type": "verification",
              "verification": { "state": "verified" }
            }
            """;

        // Act
        var value = JsonSerializer.Deserialize<PropertyValue>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        var verification = Assert.IsType<VerificationPropertyValue>(value);
        verification.Verification.State.Should().Be(VerificationStatus.Verified);
    }

    [Fact]
    public void Verification_status_round_trips_the_new_unverified_value()
    {
        // Arrange
        var status = VerificationStatus.Unverified;

        // Act
        var json = JsonSerializer.Serialize(status, RestClient.DefaultSerializerOptions);
        var parsed = JsonSerializer.Deserialize<VerificationStatus>(json, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Be("\"unverified\"");
        parsed.Should().Be(VerificationStatus.Unverified);
    }

    [Fact]
    public void View_configuration_falls_back_for_an_unknown_view_type()
    {
        // Arrange
        const string Json = """{ "type": "some_future_view", "extra": 1 }""";

        // Act
        var configuration = JsonSerializer.Deserialize<ViewConfiguration>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        var unknown = Assert.IsType<UnknownViewConfiguration>(configuration);
        unknown.AdditionalData.Should().ContainKey("extra");
    }

    [Fact]
    public void View_configuration_deserializes_a_known_view_type()
    {
        // Arrange
        const string Json = """{ "type": "board", "group_by": "Status" }""";

        // Act
        var configuration = JsonSerializer.Deserialize<ViewConfiguration>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        var board = Assert.IsType<BoardViewConfiguration>(configuration);
        board.Type.Should().Be(ViewType.BoardValue);
        board.AdditionalData.Should().ContainKey("group_by");
    }

    [Fact]
    public void View_deserializes_with_its_configuration_and_sorts()
    {
        // Arrange
        const string Json = """
            {
              "object": "view",
              "id": "view-1",
              "type": "table",
              "name": "All tasks",
              "data_source_id": "ds-1",
              "sorts": [ { "property": "Name", "direction": "ascending" } ],
              "configuration": { "type": "table" }
            }
            """;

        // Act
        var view = JsonSerializer.Deserialize<View>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        view.Type.Should().Be(ViewType.Table);
        view.Name.Should().Be("All tasks");
        view.Sorts.Single().Direction.Should().Be("ascending");
        Assert.IsType<TableViewConfiguration>(view.Configuration);
    }

    [Fact]
    public void List_emojis_response_deserializes_its_results()
    {
        // Arrange
        const string Json = """
            {
              "object": "list",
              "type": "custom_emoji",
              "results": [ { "id": "emoji-1", "name": "party-parrot", "url": "https://example.com/p.gif" } ],
              "has_more": false,
              "next_cursor": null
            }
            """;

        // Act
        var response = JsonSerializer.Deserialize<ListEmojisResponse>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        response.Results.Single().Name.Should().Be("party-parrot");
        response.HasMore.Should().BeFalse();
    }

    [Fact]
    public void Query_meeting_notes_response_deserializes_its_results()
    {
        // Arrange
        const string Json = """
            {
              "object": "list",
              "results": [
                {
                  "object": "block",
                  "id": "block-1",
                  "type": "meeting_notes",
                  "meeting_notes": { "status": "completed" }
                }
              ],
              "has_more": false
            }
            """;

        // Act
        var response = JsonSerializer.Deserialize<QueryMeetingNotesResponse>(Json, RestClient.DefaultSerializerOptions);

        // Assert
        response.Results.Single().MeetingNotes.Status.Should().Be("completed");
    }
}
