using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi; //was previously: Notion.Client.Extensions;

internal static class HttpResponseMessageExtensions
{
    internal static async Task<T> ParseStreamAsync<T>(
        this HttpResponseMessage response,
        JsonSerializerOptions serializerOptions = null)
    {
        using var stream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<T>(stream, serializerOptions ?? RestClient.DefaultSerializerOptions);
    }
}
