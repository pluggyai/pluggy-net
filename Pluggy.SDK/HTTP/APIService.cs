using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pluggy.SDK.Errors;

namespace Pluggy.SDK.HTTP
{
    /// <summary>
    /// The communication layer between the SDK and the HTTP REST backend.
    /// </summary>
    public class APIService : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private bool _disposeHttpClient;

        /// <summary>
        /// Creates a ne w instance of HttpService using a provided <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="apiKey">A API KEY provided by Pluggy to access the resources</param>
        /// <param name="baseUrl">The URL of the API</param>
        internal APIService(string apiKey, string baseUrl)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "API Key is required to execute");
            _baseUrl = baseUrl;
            _httpClient = new HttpClient(new HttpClientHandler());
        }

        private void ApplyHeaders(HttpRequestMessage message, IDictionary<string, object> headers)
        {
            // Set the authorization header
            if (headers == null || !headers.ContainsKey("X-API-KEY"))
                // Auth header can be overridden by passing custom value in headers dictionary
                if (!string.IsNullOrEmpty(_apiKey))
                    message.Headers.Add("X-API-KEY", _apiKey);

            // Apply other headers
            if (headers != null)
                foreach (var pair in headers)
                    if (pair.Key != null && pair.Value != null)
                        message.Headers.Add(pair.Key, pair.Value.ToString());
        }

        private HttpContent BuildMessageContent(object body, IDictionary<string, object> parameters)
        {
            if (parameters != null)
                return
                    new FormUrlEncodedContent(
                        parameters.Select(
                            kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value?.ToString() ?? string.Empty)));

            string serializedBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return new StringContent(serializedBody, Encoding.UTF8, "application/json");
        }

        private Uri BuildRequestUri(string resource, IDictionary<string, string> urlSegments,
            IDictionary<string, string> queryStrings)
        {
            return Utils.BuildUri(_baseUrl, resource, urlSegments, queryStrings);
        }

        /// <summary>
        /// Performs an HTTP DELETE.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="queryStrings"></param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Delete operation.</returns>
        internal async Task<T> DeleteAsync<T>(string resource, IDictionary<string, string> urlSegments,
            IDictionary<string, string> queryStrings) where T : class
        {
            return await RunAsync<T>(resource,
                HttpMethod.Delete,
                null,
                urlSegments,
                queryStrings,
                null,
                null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Performs an HTTP DELETE.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="body">The body.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="queryStrings"></param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Delete operation.</returns>
        internal async Task<T> DeleteAsync<T>(string resource, object body, IDictionary<string, string> urlSegments, IDictionary<string, string> queryStrings) where T : class
        {
            return await RunAsync<T>(resource,
                HttpMethod.Delete,
                body,
                urlSegments,
                queryStrings,
                null,
                null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP GET.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        internal async Task<T> GetAsync<T>(string resource, IDictionary<string, string> urlSegments = null,
            IDictionary<string, string> queryStrings = null, IDictionary<string, object> headers = null) where T : class
        {
            return await RunAsync<T>(resource,
                HttpMethod.Get,
                null,
                urlSegments,
                queryStrings,
                null,
                headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP PATCH operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="body">The body.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Patch operation.</returns>
        internal async Task<T> PatchAsync<T>(string resource, object body, Dictionary<string, string> urlSegments)
            where T : class
        {
            return await RunAsync<T>(resource,
                new HttpMethod("PATCH"),
                body,
                urlSegments,
                null,
                null,
                null).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP POST.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="body">The body.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Post operation.</returns>
        internal async Task<T> PostAsync<T>(string resource, object body, IDictionary<string, object> parameters,
            IDictionary<string, string> urlSegments = null, IDictionary<string, object> headers = null,
            IDictionary<string, string> queryStrings = null) where T : class
        {
            return await RunAsync<T>(resource,
                HttpMethod.Post,
                body,
                urlSegments,
                queryStrings,
                parameters,
                headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs an HTTP PUT.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="body">The body.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Put operation.</returns>
        internal async Task<T> PutAsync<T>(string resource, object body, IDictionary<string, object> parameters,
            IDictionary<string, string> urlSegments, IDictionary<string, object> headers,
            IDictionary<string, string> queryStrings) where T : class
        {
            return await RunAsync<T>(resource,
                HttpMethod.Put,
                body,
                urlSegments,
                queryStrings,
                parameters,
                headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request. All requests will pass through this method as it will apply the headers, do the JSON
        /// formatting, check for errors on return, etc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="body">The body.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>A <see cref="Task{T}"/> that represents the asynchronous Run operation.</returns>
        private async Task<T> RunAsync<T>(string resource, HttpMethod httpMethod, object body,
            IDictionary<string, string> urlSegments, IDictionary<string, string> queryStrings,
            IDictionary<string, object> parameters, IDictionary<string, object> headers) where T : class
        {
            // Build the request URL
            var requestMessage = new HttpRequestMessage(httpMethod, BuildRequestUri(resource, urlSegments, queryStrings));

            // Get the message content
            if (httpMethod != HttpMethod.Get && (body != null || parameters != null))
                requestMessage.Content = BuildMessageContent(body, parameters);

            // Apply the headers
            ApplyHeaders(requestMessage, headers);

            // Send the request
            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            // Handle API errors
            await HandleErrors(response).ConfigureAwait(false);

            // Deserialize the content
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (typeof(T) == typeof(string)) // Let string content pass through
                return (T)(object)content;

            return JsonConvert.DeserializeObject<T>(content);
        }


        private async Task HandleErrors(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                ApiError apiError = null;

                // Grab the content
                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(responseContent))
                        try
                        {
                            apiError = JsonConvert.DeserializeObject<ApiError>(responseContent);
                            if (apiError.StatusCode == 0)
                                apiError.StatusCode = (int)response.StatusCode;
                        }
                        catch (Exception)
                        {
                            apiError = new ApiError
                            {
                                Error = responseContent,
                                Message = responseContent,
                                StatusCode = (int)response.StatusCode
                            };
                        }
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException(response.StatusCode, apiError);
                    default:
                        throw new ApiException(response.StatusCode, apiError);
                }
            }
        }

        /// <summary>
        /// Disposes of any owned disposable resources such as a HttpClient
        /// </summary>
        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                _httpClient.Dispose();
                _disposeHttpClient = false;
            }
        }
    }
}
