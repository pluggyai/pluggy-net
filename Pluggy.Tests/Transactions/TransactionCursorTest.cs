using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK;
using Pluggy.SDK.Model;

namespace Pluggy.Tests.Transactions
{
    /// <summary>
    /// Intercepts HttpClient calls and returns pre-configured JSON responses in order.
    /// The first response is always consumed by the /auth call made by PluggyAPI.
    /// </summary>
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<string> _responses;
        public List<Uri> RequestUris { get; } = new List<Uri>();

        public MockHttpMessageHandler(params string[] jsonResponses)
        {
            _responses = new Queue<string>(jsonResponses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestUris.Add(request.RequestUri);
            var json = _responses.Count > 0 ? _responses.Dequeue() : "{}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    [TestFixture]
    public class TransactionCursorTest
    {
        private static readonly string ACCOUNT_ID = "562b795d-1653-429f-be86-74ead9502813";
        private static readonly string TX_ID_1 = "a8534c85-53ce-4f21-94d7-50e9d2ee4957";
        private static readonly string TX_ID_2 = "b9645d96-64df-5032-a5e8-61f0e3ff5068";
        private static readonly string TX_ID_3 = "c0756ea7-75e0-6143-b6f9-720104cc6179";
        private static readonly string BASE_URL = "https://api.pluggy.ai/";

        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static string MakePage(string[] txIds, string next)
        {
            var results = txIds.Select(id => new
            {
                id,
                description = "Test transaction",
                descriptionRaw = (string)null,
                currencyCode = "BRL",
                amount = -100.0,
                date = "2024-01-15T00:00:00.000Z",
                balance = 900.0,
                accountId = ACCOUNT_ID,
                type = "DEBIT",
                status = "POSTED",
                paymentData = (object)null
            });
            return JsonConvert.SerializeObject(new { results, next });
        }

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            var httpClient = new HttpClient(handler);
            return new PluggyAPI("client-id", "client-secret", httpClient, BASE_URL);
        }

        // ─── FetchTransactionsCursor ─────────────────────────────────────────

        [Test]
        public async Task FetchTransactionsCursor_SinglePage_ReturnsResultsAndNullNext()
        {
            var page = MakePage(new[] { TX_ID_1, TX_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page);
            var client = CreateClient(handler);

            var result = await client.FetchTransactionsCursor(Guid.Parse(ACCOUNT_ID));

            Assert.AreEqual(2, result.Results.Count);
            Assert.IsNull(result.Next);
            Assert.AreEqual(Guid.Parse(TX_ID_1), result.Results[0].Id);
            Assert.AreEqual(Guid.Parse(TX_ID_2), result.Results[1].Id);
        }

        [Test]
        public async Task FetchTransactionsCursor_WithMorePages_ReturnsNonNullNext()
        {
            var nextCursor = $"?accountId={ACCOUNT_ID}&after=cursor-xyz";
            var page = MakePage(new[] { TX_ID_1 }, nextCursor);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page);
            var client = CreateClient(handler);

            var result = await client.FetchTransactionsCursor(Guid.Parse(ACCOUNT_ID), new TransactionCursorParameters { DateFrom = new DateTime(2024, 1, 1) });

            Assert.AreEqual(1, result.Results.Count);
            Assert.IsNotNull(result.Next);
            StringAssert.Contains("cursor-xyz", result.Next);
        }

        [Test]
        public async Task FetchTransactionsCursor_PassesAfterCursorAsQueryParam()
        {
            var page = MakePage(new[] { TX_ID_1, TX_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page);
            var client = CreateClient(handler);

            await client.FetchTransactionsCursor(Guid.Parse(ACCOUNT_ID), new TransactionCursorParameters { After = "cursor-xyz" });

            var txRequest = handler.RequestUris[1];
            StringAssert.Contains("after=cursor-xyz", txRequest.Query);
            StringAssert.Contains("accountId=" + ACCOUNT_ID, txRequest.Query);
        }

        [Test]
        public async Task FetchTransactionsCursor_PassesDateFilters()
        {
            var page = MakePage(new[] { TX_ID_1 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page);
            var client = CreateClient(handler);

            await client.FetchTransactionsCursor(Guid.Parse(ACCOUNT_ID), new TransactionCursorParameters
            {
                DateFrom = new DateTime(2024, 1, 1),
                DateTo = new DateTime(2024, 12, 31)
            });

            var txRequest = handler.RequestUris[1];
            StringAssert.Contains("dateFrom=2024-01-01", txRequest.Query);
            StringAssert.Contains("dateTo=2024-12-31", txRequest.Query);
        }

        // ─── FetchAllTransactions ────────────────────────────────────────────

        [Test]
        public async Task FetchAllTransactions_SinglePage_ReturnsAllResults()
        {
            var page = MakePage(new[] { TX_ID_1, TX_ID_2, TX_ID_3 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page);
            var client = CreateClient(handler);

            var result = await client.FetchAllTransactions(Guid.Parse(ACCOUNT_ID));

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, handler.RequestUris.Count); // auth + 1 page
        }

        [Test]
        public async Task FetchAllTransactions_TwoPages_AggregatesAllResults()
        {
            var cursor1 = "cursor-page2";
            var page1 = MakePage(new[] { TX_ID_1, TX_ID_2 }, $"?accountId={ACCOUNT_ID}&after={cursor1}");
            var page2 = MakePage(new[] { TX_ID_3 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2);
            var client = CreateClient(handler);

            var result = await client.FetchAllTransactions(Guid.Parse(ACCOUNT_ID));

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(Guid.Parse(TX_ID_1), result[0].Id);
            Assert.AreEqual(Guid.Parse(TX_ID_2), result[1].Id);
            Assert.AreEqual(Guid.Parse(TX_ID_3), result[2].Id);
            Assert.AreEqual(3, handler.RequestUris.Count); // auth + 2 pages
        }

        [Test]
        public async Task FetchAllTransactions_ThreePages_AggregatesAllResults()
        {
            var cursor1 = "cursor-page2";
            var cursor2 = "cursor-page3";
            var page1 = MakePage(new[] { TX_ID_1 }, $"?accountId={ACCOUNT_ID}&after={cursor1}");
            var page2 = MakePage(new[] { TX_ID_2 }, $"?accountId={ACCOUNT_ID}&after={cursor2}");
            var page3 = MakePage(new[] { TX_ID_3 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2, page3);
            var client = CreateClient(handler);

            var result = await client.FetchAllTransactions(Guid.Parse(ACCOUNT_ID));

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(4, handler.RequestUris.Count); // auth + 3 pages
        }

        [Test]
        public async Task FetchAllTransactions_PassesAfterCursorOnSecondPage()
        {
            var cursor1 = "cursor-page2";
            var page1 = MakePage(new[] { TX_ID_1 }, $"?accountId={ACCOUNT_ID}&after={cursor1}");
            var page2 = MakePage(new[] { TX_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2);
            var client = CreateClient(handler);

            await client.FetchAllTransactions(Guid.Parse(ACCOUNT_ID));

            var secondPageRequest = handler.RequestUris[2];
            StringAssert.Contains($"after={cursor1}", secondPageRequest.Query);
        }

        [Test]
        public async Task FetchAllTransactions_PreservesFiltersAcrossPages()
        {
            var cursor1 = "cursor-page2";
            var page1 = MakePage(new[] { TX_ID_1 }, $"?accountId={ACCOUNT_ID}&after={cursor1}");
            var page2 = MakePage(new[] { TX_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2);
            var client = CreateClient(handler);

            await client.FetchAllTransactions(Guid.Parse(ACCOUNT_ID), new TransactionCursorParameters
            {
                DateFrom = new DateTime(2024, 1, 1),
                DateTo = new DateTime(2024, 12, 31)
            });

            var secondPageRequest = handler.RequestUris[2];
            StringAssert.Contains("dateFrom=2024-01-01", secondPageRequest.Query);
            StringAssert.Contains("dateTo=2024-12-31", secondPageRequest.Query);
            StringAssert.Contains($"after={cursor1}", secondPageRequest.Query);
        }
    }
}
