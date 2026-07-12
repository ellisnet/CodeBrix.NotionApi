using CodeBrix.JsonPolymorphism.Internal;
using SilverAssertions;
using Xunit;

namespace CodeBrix.JsonPolymorphism.Tests;

public class DiscriminatorMapTests
{
    [Fact]
    public void Get_returns_cached_instance_on_repeat_calls()
        => DiscriminatorMap.Get(typeof(IShape)).Should().BeSameAs(DiscriminatorMap.Get(typeof(IShape)));

    [Fact]
    public void Get_exposes_declared_discriminator_property_name()
        => DiscriminatorMap.Get(typeof(Animal)).PropertyName.Should().Be("kind");

    [Fact]
    public void Get_builds_known_type_dictionary_from_attributes()
    {
        //Arrange + Act
        var map = DiscriminatorMap.Get(typeof(IShape));

        //Assert
        map.KnownTypes["circle"].Should().Be(typeof(Circle));
        map.KnownTypes["square"].Should().Be(typeof(Square));
    }

    [Fact]
    public void Get_exposes_declared_fallback_type()
        => DiscriminatorMap.Get(typeof(IShape)).FallbackType.Should().Be(typeof(UnknownShape));

    [Fact]
    public void Get_leaves_fallback_null_when_not_declared()
        => DiscriminatorMap.Get(typeof(IStrict)).FallbackType.Should().BeNull();
}
