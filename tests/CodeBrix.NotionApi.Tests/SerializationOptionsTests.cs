using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using SilverAssertions;
using Xunit;

namespace CodeBrix.NotionApi.Tests;

public class SerializationOptionsTests
{
    [Fact]
    public async Task response_options_are_copied_when_a_resolver_is_needed()
    {
        //Arrange
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"name\":\"Android\",\"person\":{\"email\":\"test@example.com\"}}")
        };

        //Act
        var user = await response.ParseStreamAsync<User>(options);

        //Assert
        user.Name.Should().Be("Android");
        user.Person.Email.Should().Be("test@example.com");
        options.TypeInfoResolver.Should().BeNull();
        options.IsReadOnly.Should().BeFalse();
    }

    [Fact]
    public async Task supplied_resolver_is_not_replaced_with_reflection()
    {
        //Arrange
        var options = new JsonSerializerOptions { TypeInfoResolver = JsonTypeInfoResolver.Combine() };
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"name\":\"Android\"}")
        };

        //Act
        Func<Task> action = async () => await response.ParseStreamAsync<User>(options);

        //Assert
        await action.Should().ThrowAsync<NotSupportedException>();
    }
}
