using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi; //was previously: Notion.Client.Extensions;

internal static class HttpResponseMessageExtensions
{
    internal static async Task<T> ParseStreamAsync<T>(
        this HttpResponseMessage response,
        JsonSerializerOptions serializerOptions = null)
    {
        using var stream = await response.Content.ReadAsStreamAsync();

        var options = serializerOptions ?? RestClient.DefaultSerializerOptions;
        if (options.TypeInfoResolver == null)
        {
            options = new JsonSerializerOptions(options)
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };
        }
        return await JsonSerializer.DeserializeAsync<T>(stream, options);
    }
}
