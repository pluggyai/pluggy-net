using System;
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
    public class ItemResourcesTest
    {
        private static readonly string ITEM_ID = "d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c";
        private static readonly string BASE_URL = "https://api.pluggy.ai/";

        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static readonly string PAGE = JsonConvert.SerializeObject(new
        {
            page = 1,
            total = 2,
            totalPages = 1,
            results = new[]
            {
                new { resourceId = "1f9a5e0c-3d2b-4a1e-9c7f-2b8d6e4a1c30", type = "CREDIT_CARD_ACCOUNT", status = "PENDING_AUTHORISATION" },
                // A type outside the documented list must come through as-is.
                new { resourceId = "a3e71d92-5b48-4c6f-8e20-1d9c3b7a4e50", type = "EXCHANGE", status = "AVAILABLE" }
            }
        });

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        [Test]
        public async Task FetchItemResources_DeserializesPage_KeepingUnknownTypes()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, PAGE);

            var result = await CreateClient(handler).FetchItemResources(Guid.Parse(ITEM_ID));

            Assert.AreEqual(1, result.PageNr);
            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(1, result.TotalPages);
            Assert.AreEqual(2, result.Results.Count);
            Assert.AreEqual("1f9a5e0c-3d2b-4a1e-9c7f-2b8d6e4a1c30", result.Results[0].ResourceId);
            Assert.AreEqual(ItemResourceType.CREDIT_CARD_ACCOUNT, result.Results[0].Type);
            Assert.AreEqual(ItemResourceStatus.PENDING_AUTHORISATION, result.Results[0].Status);
            Assert.AreEqual("EXCHANGE", result.Results[1].Type);
            Assert.AreEqual(ItemResourceStatus.AVAILABLE, result.Results[1].Status);
        }

        [Test]
        public async Task FetchItemResources_WithoutParams_SendsNoQueryString()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, PAGE);

            await CreateClient(handler).FetchItemResources(Guid.Parse(ITEM_ID));

            var request = handler.RequestUris[1];
            Assert.AreEqual($"/items/{ITEM_ID}/resources", request.AbsolutePath);
            Assert.AreEqual(string.Empty, request.Query);
        }

        [Test]
        public async Task FetchItemResources_PassesPagingAndStatus()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, PAGE);

            await CreateClient(handler).FetchItemResources(Guid.Parse(ITEM_ID), new ItemResourceParameters
            {
                Page = 2,
                PageSize = 50,
                Status = ItemResourceStatus.PENDING_AUTHORISATION
            });

            var request = handler.RequestUris[1];
            Assert.AreEqual($"/items/{ITEM_ID}/resources", request.AbsolutePath);
            StringAssert.Contains("page=2", request.Query);
            StringAssert.Contains("pageSize=50", request.Query);
            StringAssert.Contains("status=PENDING_AUTHORISATION", request.Query);
        }

        [Test]
        public async Task FetchItemResources_OmitsUnsetParams()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, PAGE);

            await CreateClient(handler).FetchItemResources(Guid.Parse(ITEM_ID), new ItemResourceParameters
            {
                Status = ItemResourceStatus.UNAVAILABLE
            });

            Assert.AreEqual("?status=UNAVAILABLE", handler.RequestUris[1].Query);
        }

        [Test]
        public void Item_DeserializesResourceFields()
        {
            var item = JsonConvert.DeserializeObject<Item>(@"{
                ""id"": ""d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c"",
                ""resourcesCollectedAt"": ""2026-09-01T12:00:00.000Z"",
                ""hasResourcesPendingAuthorization"": true
            }");

            Assert.AreEqual(new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc), item.ResourcesCollectedAt.Value.ToUniversalTime());
            Assert.IsTrue(item.HasResourcesPendingAuthorization);
        }

        [Test]
        public void Item_ResourceFieldsDefaultWhenNullOrAbsent()
        {
            var item = JsonConvert.DeserializeObject<Item>(@"{
                ""id"": ""d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c"",
                ""resourcesCollectedAt"": null
            }");

            Assert.IsNull(item.ResourcesCollectedAt);
            Assert.IsFalse(item.HasResourcesPendingAuthorization);
        }
    }
}
