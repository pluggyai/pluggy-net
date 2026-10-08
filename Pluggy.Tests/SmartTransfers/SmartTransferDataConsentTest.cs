using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.SDK.Model;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.SmartTransfers
{
    [TestFixture]
    public class SmartTransferDataConsentTest
    {
        private static readonly string PREAUTH_ID = "0b7d7a2e-6a47-4c1e-9c63-1f3a1d5b7e90";
        private static readonly string BASE_URL = "https://api.pluggy.ai/";
        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        [Test]
        public async Task FetchSmartTransferPreauthorizationBalance_RequestsBalanceAndDeserializes()
        {
            var response = @"{ ""balance"": 1234.56, ""overdraft"": { ""contracted"": 1000, ""used"": 250.5, ""available"": 749.5 } }";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).FetchSmartTransferPreauthorizationBalance(PREAUTH_ID);

            Assert.AreEqual(HttpMethod.Get, handler.RequestMethods[1]);
            Assert.AreEqual($"/smart-transfers/preauthorizations/{PREAUTH_ID}/balance", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(1234.56m, result.Balance);
            Assert.AreEqual(1000m, result.Overdraft.Contracted);
            Assert.AreEqual(250.5m, result.Overdraft.Used);
            Assert.AreEqual(749.5m, result.Overdraft.Available);
        }

        [Test]
        public async Task FetchSmartTransferPreauthorizationBalance_DeserializesNullOverdraft()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, @"{ ""balance"": 1234.56, ""overdraft"": null }");

            var result = await CreateClient(handler).FetchSmartTransferPreauthorizationBalance(PREAUTH_ID);

            Assert.AreEqual(1234.56m, result.Balance);
            Assert.IsNull(result.Overdraft);
        }

        [Test]
        public async Task CancelSmartTransferPreauthorizationDataConsent_SendsDeleteAndDeserializesDataConsent()
        {
            var response = $@"{{ ""id"": ""{PREAUTH_ID}"", ""dataConsent"": {{ ""status"": ""REJECTED"", ""rejectionReason"": ""CONSENT_REVOKED"", ""updatedAt"": ""2026-10-01T12:00:00.000Z"" }} }}";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).CancelSmartTransferPreauthorizationDataConsent(PREAUTH_ID);

            Assert.AreEqual(HttpMethod.Delete, handler.RequestMethods[1]);
            Assert.AreEqual($"/smart-transfers/preauthorizations/{PREAUTH_ID}/data-consent", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual("REJECTED", result.DataConsent.Status);
            Assert.AreEqual("CONSENT_REVOKED", result.DataConsent.RejectionReason);
            Assert.IsNotNull(result.DataConsent.UpdatedAt);
        }

        [Test]
        public void CreateSmartTransferPreauthorization_ToBody_IncludesLinkedJourneyOnlyWhenSet()
        {
            var withFlag = new CreateSmartTransferPreauthorization { ConnectorId = 1, LinkedJourney = true }.ToBody();
            var withoutFlag = new CreateSmartTransferPreauthorization { ConnectorId = 1 }.ToBody();

            Assert.AreEqual(true, withFlag["linkedJourney"]);
            Assert.IsFalse(withoutFlag.ContainsKey("linkedJourney"));
        }
    }
}
