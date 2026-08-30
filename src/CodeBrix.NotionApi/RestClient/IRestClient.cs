using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CodeBrix.NotionApi; //was previously: Notion.Client;

public interface IRestClient : IDisposable
{
    Task<T> GetAsync<T>(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default);

    Task<T> PostAsync<T>(
        string uri,
        object body,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default);

    Task<T> PostAsync<T>(
        string uri,
        ISendFileUploadFormDataParameters formData,
        IEnumerable<KeyValuePair<string, string>> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        IBasicAuthenticationParameters basicAuthenticationParameters = null,
        CancellationToken cancellationToken = default);

    Task<T> PatchAsync<T>(
        string uri,
        object body,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        CancellationToken cancellationToken = default);

    Task<T> DeleteAsync<T>(
        string uri,
        IDictionary<string, string> queryParams = null,
        IDictionary<string, string> headers = null,
        JsonSerializerOptions serializerOptions = null,
        CancellationToken cancellationToken = default);
}
