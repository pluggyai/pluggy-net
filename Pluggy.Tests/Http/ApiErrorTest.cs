using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK;
using Pluggy.SDK.Errors;

namespace Pluggy.Tests.Http
{
    internal class StatusHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<(HttpStatusCode Status, string Body)> _responses;
        public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();

        public StatusHttpMessageHandler(params (HttpStatusCode Status, string Body)[] responses)
        {
            _responses = new Queue<(HttpStatusCode, string)>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            var (status, body) = _responses.Count > 0 ? _responses.Dequeue() : (HttpStatusCode.OK, "{}");
            return Task.FromResult(new HttpResponseMessage(status)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
        }
    }

    [TestFixture]
    public class ApiErrorTest
    {
        private static readonly string BASE_URL = "https://api.pluggy.ai/";
        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private const string IP_NOT_ALLOWED_MESSAGE =
            "The request IP address is not in the list of allowed IPs configured for this application.";

        private static readonly string IP_NOT_ALLOWED_BODY =
            "{ \"code\": 403, \"codeDescription\": \"CLIENT_IP_NOT_ALLOWED\", \"message\": \"" + IP_NOT_ALLOWED_MESSAGE + "\" }";

        private static PluggyAPI CreateClient(StatusHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        // ─── Error body mapping ──────────────────────────────────────────────

        [Test]
        public void ApiError_DeserializesApiErrorBody()
        {
            var error = JsonConvert.DeserializeObject<ApiError>(IP_NOT_ALLOWED_BODY);

            Assert.AreEqual(403, error.Code);
            Assert.AreEqual("CLIENT_IP_NOT_ALLOWED", error.CodeDescription);
            Assert.AreEqual(IP_NOT_ALLOWED_MESSAGE, error.Message);
            // Compatibility properties carry the same values.
            Assert.AreEqual(403, error.StatusCode);
            Assert.AreEqual("CLIENT_IP_NOT_ALLOWED", error.ErrorCode);
            Assert.IsTrue(error.IsStatus(HttpStatusCode.Forbidden));
            Assert.IsTrue(error.IsCode(ApiErrorCode.CLIENT_IP_NOT_ALLOWED));
        }

        [Test]
        public void ApiError_LegacyFieldNames_StillPopulateBothNames()
        {
            var error = JsonConvert.DeserializeObject<ApiError>(
                "{ \"statusCode\": 400, \"errorCode\": \"SOME_CODE\", \"message\": \"m\" }");

            Assert.AreEqual(400, error.StatusCode);
            Assert.AreEqual("SOME_CODE", error.ErrorCode);
            Assert.AreEqual("SOME_CODE", error.CodeDescription);
            Assert.IsNull(error.Code);
        }

        [Test]
        public void ApiError_NonNumericCode_DoesNotBreakDeserialization()
        {
            var error = JsonConvert.DeserializeObject<ApiError>(
                "{ \"code\": \"oops\", \"codeDescription\": \"SOME_CODE\", \"message\": \"m\" }");

            Assert.IsNull(error.Code);
            Assert.AreEqual("SOME_CODE", error.CodeDescription);
            Assert.AreEqual("m", error.Message);
        }

        // ─── CLIENT_IP_NOT_ALLOWED reaches the caller ────────────────────────

        [Test]
        public void AuthForbidden_SurfacesClientIpNotAllowed_WithoutRetry()
        {
            var handler = new StatusHttpMessageHandler((HttpStatusCode.Forbidden, IP_NOT_ALLOWED_BODY));
            var client = CreateClient(handler);

            var exception = Assert.ThrowsAsync<ApiException>(() => client.CreateConnectToken());

            Assert.AreEqual(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.AreEqual(ApiErrorCode.CLIENT_IP_NOT_ALLOWED, exception.ErrorCode);
            Assert.IsTrue(exception.HasErrorCode(ApiErrorCode.CLIENT_IP_NOT_ALLOWED));
            Assert.AreEqual(IP_NOT_ALLOWED_MESSAGE, exception.Message);
            Assert.AreEqual(403, exception.ApiError.Code);

            // Only the /auth call was made: no retry and the original request was never sent.
            Assert.AreEqual(1, handler.Requests.Count);
            Assert.AreEqual("/auth", handler.Requests[0].RequestUri.AbsolutePath);
        }

        [Test]
        public void ApiKeyRequestForbidden_SurfacesClientIpNotAllowed_WithoutRetry()
        {
            var handler = new StatusHttpMessageHandler(
                (HttpStatusCode.OK, AUTH_RESPONSE),
                (HttpStatusCode.Forbidden, IP_NOT_ALLOWED_BODY));
            var client = CreateClient(handler);

            var exception = Assert.ThrowsAsync<ApiException>(() => client.CreateConnectToken());

            Assert.AreEqual(HttpStatusCode.Forbidden, exception.StatusCode);
            Assert.IsTrue(exception.HasErrorCode(ApiErrorCode.CLIENT_IP_NOT_ALLOWED));
            Assert.AreEqual(IP_NOT_ALLOWED_MESSAGE, exception.Message);

            Assert.AreEqual(2, handler.Requests.Count);
            Assert.AreEqual("/auth", handler.Requests[0].RequestUri.AbsolutePath);
            Assert.AreEqual("/connect_token", handler.Requests[1].RequestUri.AbsolutePath);
            Assert.IsTrue(handler.Requests[1].Headers.TryGetValues("X-API-KEY", out var apiKey));
            CollectionAssert.AreEqual(new[] { "test-api-key" }, apiKey);
        }

        [Test]
        public void OtherErrorCode_IsNotReportedAsClientIpNotAllowed()
        {
            var handler = new StatusHttpMessageHandler(
                (HttpStatusCode.OK, AUTH_RESPONSE),
                (HttpStatusCode.Forbidden, "{ \"code\": 403, \"codeDescription\": \"SOMETHING_ELSE\", \"message\": \"m\" }"));
            var client = CreateClient(handler);

            var exception = Assert.ThrowsAsync<ApiException>(() => client.CreateConnectToken());

            Assert.AreEqual("SOMETHING_ELSE", exception.ErrorCode);
            Assert.IsFalse(exception.HasErrorCode(ApiErrorCode.CLIENT_IP_NOT_ALLOWED));
        }
    }
}
