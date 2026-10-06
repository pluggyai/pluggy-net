using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK;
using Pluggy.SDK.Model;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.Items
{
    [TestFixture]
    public class ItemCursorTest
    {
        private static readonly string ITEM_ID_1 = "d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c";
        private static readonly string ITEM_ID_2 = "e1f9559f-1267-4c5b-bf7d-4f3b7e0c006d";
        private static readonly string ITEM_ID_3 = "f20a66a0-2378-4d6c-c08e-504c8f1d117e";
        private static readonly string CLIENT_USER_ID = "user-123";
        private static readonly int CONNECTOR_ID = 201;
        private static readonly string BASE_URL = "https://api.pluggy.ai/";

        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static string MakePage(string[] itemIds, string next)
        {
            var results = itemIds.Select(id => new
            {
                id,
                status = "UPDATED",
                clientUserId = CLIENT_USER_ID,
                createdAt = "2026-09-01T12:00:00.000Z"
            });
            return JsonConvert.SerializeObject(new { results, next });
        }

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        // ─── FetchItemsCursor ────────────────────────────────────────────────

        [Test]
        public async Task FetchItemsCursor_WithoutParams_SendsNoQueryString()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, MakePage(new[] { ITEM_ID_1, ITEM_ID_2 }, null));

            var result = await CreateClient(handler).FetchItemsCursor();

            Assert.AreEqual(2, result.Results.Count);
            Assert.IsNull(result.Next);
            Assert.AreEqual(Guid.Parse(ITEM_ID_1), result.Results[0].Id);
            Assert.AreEqual(Guid.Parse(ITEM_ID_2), result.Results[1].Id);
            Assert.AreEqual(CLIENT_USER_ID, result.Results[0].ClientUserId);

            var request = handler.RequestUris[1];
            Assert.AreEqual("/v2/items", request.AbsolutePath);
            Assert.AreEqual(string.Empty, request.Query);
        }

        [Test]
        public async Task FetchItemsCursor_PassesFilters()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, MakePage(new[] { ITEM_ID_1 }, null));

            await CreateClient(handler).FetchItemsCursor(new ItemCursorParameters
            {
                ClientUserId = CLIENT_USER_ID,
                ConnectorId = CONNECTOR_ID
            });

            var request = handler.RequestUris[1];
            Assert.AreEqual("/v2/items", request.AbsolutePath);
            StringAssert.Contains("clientUserId=" + CLIENT_USER_ID, request.Query);
            StringAssert.Contains("connectorId=" + CONNECTOR_ID, request.Query);
            StringAssert.DoesNotContain("after=", request.Query);
        }

        [Test]
        public async Task FetchItemsCursor_OmitsUnsetParams()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, MakePage(new[] { ITEM_ID_1 }, null));

            await CreateClient(handler).FetchItemsCursor(new ItemCursorParameters { ConnectorId = 0 });

            Assert.AreEqual("?connectorId=0", handler.RequestUris[1].Query);
        }

        [Test]
        public async Task FetchItemsCursor_PassesAfterCursor_AndReturnsNext()
        {
            var next = $"?clientUserId={CLIENT_USER_ID}&after=cursor-page3";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, MakePage(new[] { ITEM_ID_1 }, next));

            var result = await CreateClient(handler).FetchItemsCursor(new ItemCursorParameters { After = "cursor-page2" });

            Assert.AreEqual(next, result.Next);
            StringAssert.Contains("after=cursor-page2", handler.RequestUris[1].Query);
        }

        // ─── FetchAllItems ───────────────────────────────────────────────────

        [Test]
        public async Task FetchAllItems_SinglePage_ReturnsAllResults()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, MakePage(new[] { ITEM_ID_1, ITEM_ID_2 }, null));

            var result = await CreateClient(handler).FetchAllItems();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, handler.RequestUris.Count); // auth + 1 page
        }

        [Test]
        public async Task FetchAllItems_ThreePages_AggregatesInOrder()
        {
            var page1 = MakePage(new[] { ITEM_ID_1 }, "?after=cursor-page2");
            var page2 = MakePage(new[] { ITEM_ID_2 }, "?after=cursor-page3");
            var page3 = MakePage(new[] { ITEM_ID_3 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2, page3);

            var result = await CreateClient(handler).FetchAllItems();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(Guid.Parse(ITEM_ID_1), result[0].Id);
            Assert.AreEqual(Guid.Parse(ITEM_ID_2), result[1].Id);
            Assert.AreEqual(Guid.Parse(ITEM_ID_3), result[2].Id);
            Assert.AreEqual(4, handler.RequestUris.Count); // auth + 3 pages
            Assert.AreEqual(string.Empty, handler.RequestUris[1].Query);
            Assert.AreEqual("?after=cursor-page2", handler.RequestUris[2].Query);
            Assert.AreEqual("?after=cursor-page3", handler.RequestUris[3].Query);
        }

        [Test]
        public async Task FetchAllItems_ForwardsFiltersAcrossPages()
        {
            var page1 = MakePage(new[] { ITEM_ID_1 }, $"?clientUserId={CLIENT_USER_ID}&connectorId={CONNECTOR_ID}&after=cursor-page2");
            var page2 = MakePage(new[] { ITEM_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2);

            var result = await CreateClient(handler).FetchAllItems(new ItemCursorParameters
            {
                ClientUserId = CLIENT_USER_ID,
                ConnectorId = CONNECTOR_ID
            });

            Assert.AreEqual(2, result.Count);
            var secondPageRequest = handler.RequestUris[2];
            Assert.AreEqual("/v2/items", secondPageRequest.AbsolutePath);
            StringAssert.Contains("clientUserId=" + CLIENT_USER_ID, secondPageRequest.Query);
            StringAssert.Contains("connectorId=" + CONNECTOR_ID, secondPageRequest.Query);
            StringAssert.Contains("after=cursor-page2", secondPageRequest.Query);
        }

        [Test]
        public async Task FetchAllItems_DoesNotMutateCallerParameters()
        {
            var page1 = MakePage(new[] { ITEM_ID_1 }, "?after=cursor-page2");
            var page2 = MakePage(new[] { ITEM_ID_2 }, null);
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, page1, page2);
            var parameters = new ItemCursorParameters { ConnectorId = CONNECTOR_ID };

            await CreateClient(handler).FetchAllItems(parameters);

            Assert.IsNull(parameters.After);
        }
    }
}
