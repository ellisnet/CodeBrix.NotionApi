using System.Collections.Generic;
using System.Text.Json;
using CodeBrix.NotionApi;
using SilverAssertions;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

// Regression tests for a port defect: System.Text.Json serializes the DECLARED type and does NOT
// inherit a base class's [JsonConverter]. Two empty/near-empty CONCRETE bases -- RichTextBaseInput
// and DataSourcePropertyConfigRequest -- were therefore serialized as themselves, silently dropping
// every subtype payload put on the wire. Both are now abstract, which routes them through
// RuntimeTypeConverterFactory (the same treatment the Filter base already had).
public class DeclaredTypeSerializationTests
{
    private static string Serialize<T>(T value) =>
        JsonSerializer.Serialize(value, RestClient.DefaultSerializerOptions);

    [Fact]
    public void Block_update_rich_text_keeps_its_text_content()
    {
        // Arrange
        IUpdateBlock updateBlock = new ParagraphUpdateBlock
        {
            Paragraph = new ParagraphUpdateBlock.Info
            {
                RichText = [new RichTextTextInput { Text = new Text { Content = "kept" } }]
            }
        };

        // Act
        var json = Serialize(updateBlock);

        // Assert
        json.Should().Contain("\"text\":{\"content\":\"kept\"}");
    }

    [Fact]
    public void Heading_update_rich_text_keeps_its_text_content()
    {
        // Arrange
        IUpdateBlock updateBlock = new HeadingThreeUpdateBlock
        {
            Heading_3 = new HeadingThreeUpdateBlock.Info
            {
                RichText = [new RichTextTextInput { Text = new Text { Content = "kept" } }]
            }
        };

        // Act
        var json = Serialize(updateBlock);

        // Assert
        json.Should().Contain("\"content\":\"kept\"");
    }

    [Fact]
    public void Comment_create_rich_text_keeps_its_text_content()
    {
        // Arrange
        var request = CreateCommentRequest.CreatePageComment(
            new ParentPageInput { PageId = "page-1" },
            [new RichTextTextInput { Text = new Text { Content = "kept" } }]);

        // Act
        var json = JsonSerializer.Serialize<object>(
            (ICreateCommentsBodyParameters)request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"content\":\"kept\"");
    }

    [Fact]
    public void Comment_create_body_uses_notions_snake_case_names()
    {
        // Arrange - System.Text.Json ignores [JsonPropertyName] declared on an interface, so
        // ICreateCommentsBodyParameters' names have to be repeated on CreateCommentRequest. Without
        // them the body went out as "richText"/"discussionId" and every CreateAsync call failed with
        // "body.rich_text should be defined, instead was `undefined`".
        var request = CreateCommentRequest.CreatePageComment(
            new ParentPageInput { PageId = "page-1" },
            [new RichTextTextInput { Text = new Text { Content = "hello" } }]);

        // Act - CommentsClient.CreateAsync posts the request through this interface
        var json = JsonSerializer.Serialize<object>(
            (ICreateCommentsBodyParameters)request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"rich_text\"");
        json.Should().Contain("\"parent\"");
        json.Should().NotContain("richText");
        json.Should().NotContain("discussionId");
    }

    [Fact]
    public void Discussion_comment_create_body_uses_notions_snake_case_names()
    {
        // Arrange
        var request = CreateCommentRequest.CreateDiscussionComment(
            "discussion-1",
            [new RichTextTextInput { Text = new Text { Content = "hello" } }]);

        // Act
        var json = JsonSerializer.Serialize<object>(
            (ICreateCommentsBodyParameters)request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"discussion_id\":\"discussion-1\"");
        json.Should().Contain("\"rich_text\"");
        json.Should().NotContain("discussionId");
    }

    [Fact]
    public void Rich_text_equation_input_keeps_its_expression()
    {
        // Arrange
        IEnumerable<RichTextBaseInput> richText =
            [new RichTextEquationInput { Equation = new Equation { Expression = "e=mc^2" } }];

        // Act
        var json = Serialize(richText);

        // Assert
        json.Should().Contain("\"expression\":\"e=mc^2\"");
    }

    [Fact]
    public void Create_data_source_property_config_keeps_its_per_type_payload()
    {
        // Arrange
        var request = new CreateDataSourceRequest
        {
            Parent = new DatabaseParentRequest { DatabaseId = "db-1" },
            Properties = new Dictionary<string, DataSourcePropertyConfigRequest>
            {
                { "Name", new TitleDataSourcePropertyConfigRequest { Title = new Dictionary<string, object>() } },
                {
                    "Stage",
                    new SelectDataSourcePropertyConfigRequest
                    {
                        Select = new SelectDataSourcePropertyConfigRequest.SelectOptions
                        {
                            Options = [new SelectOptionRequest { Name = "Draft" }]
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(
            (ICreateDataSourceBodyParameters)request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"type\":\"title\"");
        json.Should().Contain("\"title\":{}");
        json.Should().Contain("\"type\":\"select\"");
        json.Should().Contain("\"name\":\"Draft\"");
    }

    [Fact]
    public void Create_data_source_status_property_config_keeps_its_typed_options()
    {
        // Arrange - the typed StatusConfigRequest introduced with Notion API version 2026-03-11
        var request = new CreateDataSourceRequest
        {
            Parent = new DatabaseParentRequest { DatabaseId = "db-1" },
            Properties = new Dictionary<string, DataSourcePropertyConfigRequest>
            {
                {
                    "Status",
                    new StatusDataSourcePropertyConfigRequest
                    {
                        Status = new StatusConfigRequest
                        {
                            Options = [new StatusOptionRequest { Name = "In progress", Color = Color.Blue }],
                            Groups = [new StatusGroupRequest { Name = "Active", OptionIds = ["opt-1"] }]
                        }
                    }
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize<object>(
            (ICreateDataSourceBodyParameters)request, RestClient.DefaultSerializerOptions);

        // Assert
        json.Should().Contain("\"type\":\"status\"");
        json.Should().Contain("\"name\":\"In progress\"");
        json.Should().Contain("\"option_ids\":[\"opt-1\"]");
    }
}
