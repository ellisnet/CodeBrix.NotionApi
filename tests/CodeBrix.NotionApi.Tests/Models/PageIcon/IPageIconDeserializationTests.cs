using System.Text.Json;
using CodeBrix.NotionApi;
using Xunit;
using SilverAssertions;

namespace CodeBrix.NotionApi.Tests; //was previously: Notion.UnitTests.Models.PageIcon;

/// <summary>
/// Regression tests for the polymorphic <see cref="IPageIcon"/> discriminator map.
/// Guards against re-introducing a duplicate <c>[JsonKnownType]</c> mapping for the <c>"icon"</c>
/// discriminator, which used to throw the first time any page icon was deserialized (e.g. when reading
/// back a page or a callout block that carries an icon).
/// </summary>
public class IPageIconDeserializationTests
{
    [Fact]
    public void Deserializes_icon_type_as_IconPageIcon()
    {
        //Arrange
        var json = @"{""type"":""icon"",""icon"":{""name"":""star"",""color"":""blue""}}";

        //Act
        var icon = JsonSerializer.Deserialize<IPageIcon>(json, RestClient.DefaultSerializerOptions);

        //Assert
        icon.Should().BeOfType<IconPageIcon>();
        var typed = (IconPageIcon)icon;
        typed.Icon.Name.Should().Be("star");
        typed.Icon.Color.Should().Be("blue");
    }

    [Fact]
    public void Deserializes_emoji_type_as_EmojiPageIcon()
        => JsonSerializer.Deserialize<IPageIcon>(@"{""type"":""emoji"",""emoji"":""🚀""}", RestClient.DefaultSerializerOptions)
            .Should().BeOfType<EmojiPageIcon>();

    [Fact]
    public void Deserializing_any_page_icon_does_not_throw_on_duplicate_discriminator()
    {
        //Act
        var act = () => JsonSerializer.Deserialize<IPageIcon>(
            @"{""type"":""emoji"",""emoji"":""📌""}", RestClient.DefaultSerializerOptions);

        //Assert
        act.Should().NotThrow();
    }
}
