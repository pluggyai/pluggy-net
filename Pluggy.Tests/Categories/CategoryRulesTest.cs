using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.Categories
{
    [TestFixture]
    public class CategoryRulesTest
    {
        private static readonly string RULE_ID = "7c1d2e3f-4a5b-4c6d-8e9f-0a1b2c3d4e5f";
        private static readonly string BASE_URL = "https://api.pluggy.ai/";

        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        [Test]
        public async Task DeleteCategoryRule_SendsDeleteToRulePath()
        {
            // The API answers 204 with an empty body.
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, "");
            var client = new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);

            await client.DeleteCategoryRule(Guid.Parse(RULE_ID));

            Assert.AreEqual(2, handler.RequestUris.Count);
            Assert.AreEqual($"/categories/rules/{RULE_ID}", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(string.Empty, handler.RequestUris[1].Query);
            Assert.AreEqual(HttpMethod.Delete, handler.RequestMethods[1]);
        }
    }
}
