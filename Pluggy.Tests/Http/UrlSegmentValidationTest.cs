using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK;
using Pluggy.SDK.Model;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.Http
{
    [TestFixture]
    public class UrlSegmentValidationTest
    {
        private static readonly Guid PARENT_ID = Guid.Parse("11111111-1111-4111-8111-111111111111");
        private static readonly string BASE_URL = "https://api.pluggy.ai/";
        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        // Every public method that places a free-form string argument in the URL path.
        private static readonly Dictionary<string, Func<PluggyAPI, string, Task>> StringIdMethods =
            new Dictionary<string, Func<PluggyAPI, string, Task>>
            {
                { nameof(PluggyAPI.FetchAutomaticPixSchedule), (client, id) => client.FetchAutomaticPixSchedule(PARENT_ID, id) },
                { nameof(PluggyAPI.CancelAutomaticPixSchedule), (client, id) => client.CancelAutomaticPixSchedule(PARENT_ID, id) },
                { nameof(PluggyAPI.RetryAutomaticPixSchedule), (client, id) => client.RetryAutomaticPixSchedule(PARENT_ID, id, new RetryAutomaticPixPaymentRequest()) },
                { nameof(PluggyAPI.FetchSmartTransferPayment), (client, id) => client.FetchSmartTransferPayment(id) },
                { nameof(PluggyAPI.FetchSmartTransferPreauthorization), (client, id) => client.FetchSmartTransferPreauthorization(id) },
                { nameof(PluggyAPI.FetchSmartTransferPreauthorizationPayments), (client, id) => client.FetchSmartTransferPreauthorizationPayments(id) },
                { nameof(PluggyAPI.FetchSmartTransferPreauthorizationBalance), (client, id) => client.FetchSmartTransferPreauthorizationBalance(id) },
                { nameof(PluggyAPI.CancelSmartTransferPreauthorizationDataConsent), (client, id) => client.CancelSmartTransferPreauthorizationDataConsent(id) },
                { nameof(PluggyAPI.FetchBill), (client, id) => client.FetchBill(id) },
                { nameof(PluggyAPI.FetchBoleto), (client, id) => client.FetchBoleto(id) },
                { nameof(PluggyAPI.CancelBoleto), (client, id) => client.CancelBoleto(id) },
            };

        private static IEnumerable<string> StringIdMethodNames => StringIdMethods.Keys;

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        [Test]
        public void StringIdMethod_RejectsValueThatCannotBeAPathSegment_WithoutSendingARequest(
            [ValueSource(nameof(StringIdMethodNames))] string method,
            [Values(".", "..", "", null)] string id)
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE);

            Assert.ThrowsAsync<ArgumentException>(() => StringIdMethods[method](CreateClient(handler), id));

            Assert.IsEmpty(handler.RequestUris);
        }

        [TestCase("22222222-2222-4222-8222-222222222222", "22222222-2222-4222-8222-222222222222")]
        [TestCase("...", "...")]
        [TestCase("a/b", "a%2Fb")]
        [TestCase("../x", "..%2Fx")]
        [TestCase("%2e%2e", "%252e%252e")]
        public async Task StringIdMethod_KeepsAnyOtherValueInsideItsSegment(string id, string expectedSegment)
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE);

            await CreateClient(handler).CancelAutomaticPixSchedule(PARENT_ID, id);

            Assert.AreEqual(
                $"/payments/requests/{PARENT_ID}/automatic-pix/schedules/{expectedSegment}/cancel",
                handler.RequestUris[1].AbsolutePath);
        }
    }
}
