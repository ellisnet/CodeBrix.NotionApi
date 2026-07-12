using System.Linq;
using CodeBrix.NotionApi;
using Xunit;
using SilverAssertions;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests;

public class NotionTextTests
{
    [Fact]
    public void Run_applies_annotations_and_link()
    {
        //Act
        var run = NotionText.Run("hi", bold: true, italic: true, code: true, linkUrl: "https://example.test");

        //Assert
        run.Text.Content.Should().Be("hi");
        run.Text.Link.Url.Should().Be("https://example.test");
        run.Annotations.IsBold.Should().BeTrue();
        run.Annotations.IsItalic.Should().BeTrue();
        run.Annotations.IsCode.Should().BeTrue();
    }

    [Fact]
    public void Run_without_annotations_leaves_annotations_null()
        => NotionText.Run("plain").Annotations.Should().BeNull();

    [Fact]
    public void Split_returns_a_single_run_for_short_text()
        => NotionText.Split("short").Should().HaveCount(1);

    [Fact]
    public void Split_keeps_runs_within_the_limit_preserves_content_and_annotations()
    {
        //Arrange — ~6000 characters of whole words
        var content = string.Concat(Enumerable.Repeat("lorem ", 1000)).Trim();

        //Act
        var runs = NotionText.Split(content, bold: true);

        //Assert
        (runs.Count > 1).Should().BeTrue();
        runs.All(r => r.Text.Content.Length <= NotionText.MaxRunLength).Should().BeTrue();
        runs.All(r => r.Annotations.IsBold).Should().BeTrue();
        string.Concat(runs.Select(r => r.Text.Content)).Should().Be(content);
    }

    [Fact]
    public void Split_hard_splits_a_word_with_no_spaces_still_within_limit()
    {
        //Arrange — one 2500-char "word" with no break points
        var content = new string('a', 2500);

        //Act
        var runs = NotionText.Split(content);

        //Assert
        runs.All(r => r.Text.Content.Length <= NotionText.MaxRunLength).Should().BeTrue();
        string.Concat(runs.Select(r => r.Text.Content)).Should().Be(content);
    }
}
