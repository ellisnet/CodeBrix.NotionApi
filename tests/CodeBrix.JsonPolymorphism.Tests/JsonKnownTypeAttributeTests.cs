using System;
using SilverAssertions;
using Xunit;

namespace CodeBrix.JsonPolymorphism.Tests;

public class JsonKnownTypeAttributeTests
{
    [Fact]
    public void KnownType_round_trips_from_constructor()
        => new JsonKnownTypeAttribute(typeof(Circle), "circle").KnownType.Should().Be(typeof(Circle));

    [Fact]
    public void DiscriminatorValue_round_trips_from_constructor()
        => new JsonKnownTypeAttribute(typeof(Circle), "circle").DiscriminatorValue.Should().Be("circle");

    [Fact]
    public void Constructor_throws_on_null_known_type()
    {
        //Arrange
        Action act = () => _ = new JsonKnownTypeAttribute(null, "circle");

        //Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_throws_on_null_discriminator_value()
    {
        //Arrange
        Action act = () => _ = new JsonKnownTypeAttribute(typeof(Circle), null);

        //Act + Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
