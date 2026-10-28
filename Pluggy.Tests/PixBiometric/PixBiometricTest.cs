using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.SDK.Model;
using Pluggy.Tests.Transactions;

namespace Pluggy.Tests.PixBiometric
{
    [TestFixture]
    public class PixBiometricTest
    {
        private static readonly Guid ENROLLMENT_ID = Guid.Parse("c91a5e2b-7d0f-4f3e-9a51-2f6b8e1d4c77");
        private static readonly Guid CUSTOMER_ID = Guid.Parse("a2d4c1e8-5b7f-4c3a-9e2d-1f6b8c0d7e55");
        private static readonly Guid RECIPIENT_ID = Guid.Parse("ab276a3d-17ba-47eb-97bf-688475037ffe");
        private static readonly string BASE_URL = "https://api.pluggy.ai/";
        private static readonly string AUTH_RESPONSE = JsonConvert.SerializeObject(new { apiKey = "test-api-key" });

        private static readonly string ENROLLMENT_JSON = $@"{{
            ""id"": ""{ENROLLMENT_ID}"",
            ""status"": ""REVOKED"",
            ""connector"": {{ ""id"": 608, ""name"": ""Example Bank"", ""imageUrl"": ""https://example.com/logo.svg"" }},
            ""debtor"": {{ ""name"": ""John Doe"", ""taxNumber"": ""***.456.789-**"", ""bankAccount"": {{ ""agency"": ""1234"", ""account"": ""****4821"", ""name"": ""Example Bank"" }} }},
            ""cpf"": ""***.456.789-**"",
            ""cnpj"": null,
            ""customerId"": ""{CUSTOMER_ID}"",
            ""limits"": {{ ""transaction"": 1000, ""daily"": 3000, ""dailyUsed"": 149.9 }},
            ""expiresAt"": ""2027-10-28T13:05:00.000Z"",
            ""dataConsent"": {{ ""status"": ""AUTHORISED"", ""rejectionReason"": null, ""updatedAt"": ""2026-10-28T13:05:00.000Z"" }},
            ""revokedBy"": ""CLIENT"",
            ""errorDetail"": null,
            ""createdAt"": ""2026-10-28T13:02:00.000Z"",
            ""updatedAt"": ""2026-10-28T13:05:00.000Z""
        }}";

        private static PluggyAPI CreateClient(MockHttpMessageHandler handler)
        {
            return new PluggyAPI("client-id", "client-secret", new HttpClient(handler), BASE_URL);
        }

        [Test]
        public async Task CreatePaymentRequestPixBiometric_PostsAndDeserializesPixBiometricDetails()
        {
            var response = $@"{{ ""id"": ""{Guid.NewGuid()}"", ""amount"": 0.01, ""pixBiometric"": {{ ""linkedJourney"": true, ""enrollment"": null }} }}";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).CreatePaymentRequestPixBiometric(
                new CreatePixBiometricPaymentRequest { Amount = 0.01, RecipientId = RECIPIENT_ID, LinkedJourney = true });

            Assert.AreEqual(HttpMethod.Post, handler.RequestMethods[1]);
            Assert.AreEqual("/payments/requests/pix-biometric", handler.RequestUris[1].AbsolutePath);
            Assert.IsTrue(result.PixBiometric.LinkedJourney);
            Assert.IsNull(result.PixBiometric.Enrollment);
        }

        [Test]
        public void CreatePixBiometricPaymentRequest_ToBody_OmitsUnsetOptionalFields()
        {
            var minimal = new CreatePixBiometricPaymentRequest { Amount = 10.5, RecipientId = RECIPIENT_ID }.ToBody();
            var full = new CreatePixBiometricPaymentRequest
            {
                Amount = 10.5,
                RecipientId = RECIPIENT_ID,
                Description = "Order 4821",
                CustomerId = CUSTOMER_ID,
                ClientPaymentId = "4821",
                CallbackUrls = new PaymentCallbackUrls { Success = "https://example.com/ok" },
                LinkedJourney = false
            }.ToBody();

            Assert.AreEqual(2, minimal.Count);
            Assert.AreEqual(RECIPIENT_ID.ToString(), minimal["recipientId"]);
            Assert.IsFalse(minimal.ContainsKey("linkedJourney"));
            Assert.IsFalse(minimal.ContainsKey("isSandbox"));
            Assert.AreEqual(false, full["linkedJourney"]);
            Assert.AreEqual(CUSTOMER_ID.ToString(), full["customerId"]);
            Assert.AreEqual("4821", full["clientPaymentId"]);
        }

        [Test]
        public async Task FetchPixBiometricEnrollments_SendsFiltersAsQuery()
        {
            var response = $@"{{ ""total"": 1, ""totalPages"": 1, ""page"": 1, ""results"": [ {ENROLLMENT_JSON} ] }}";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).FetchPixBiometricEnrollments(new PixBiometricEnrollmentParameters
            {
                CustomerId = CUSTOMER_ID,
                Cpf = "12345678900",
                Status = PixBiometricEnrollmentStatus.AUTHORIZED,
                Page = 2,
                PageSize = 10
            });

            Assert.AreEqual(HttpMethod.Get, handler.RequestMethods[1]);
            Assert.AreEqual("/pix-biometric/enrollments", handler.RequestUris[1].AbsolutePath);
            var query = handler.RequestUris[1].Query;
            StringAssert.Contains($"customerId={CUSTOMER_ID}", query);
            StringAssert.Contains("cpf=12345678900", query);
            StringAssert.Contains("status=AUTHORIZED", query);
            StringAssert.Contains("page=2", query);
            StringAssert.Contains("pageSize=10", query);
            StringAssert.DoesNotContain("cnpj", query);
            Assert.AreEqual(1, result.Results.Count);
            Assert.AreEqual(ENROLLMENT_ID, result.Results[0].Id);
        }

        [Test]
        public async Task FetchPixBiometricEnrollments_WithoutFilters_SendsNoQuery()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, @"{ ""total"": 0, ""totalPages"": 0, ""page"": 1, ""results"": [] }");

            await CreateClient(handler).FetchPixBiometricEnrollments();

            Assert.AreEqual(string.Empty, handler.RequestUris[1].Query);
        }

        [Test]
        public async Task FetchPixBiometricEnrollment_DeserializesEveryField()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, ENROLLMENT_JSON);

            var result = await CreateClient(handler).FetchPixBiometricEnrollment(ENROLLMENT_ID);

            Assert.AreEqual(HttpMethod.Get, handler.RequestMethods[1]);
            Assert.AreEqual($"/pix-biometric/enrollments/{ENROLLMENT_ID}", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(PixBiometricEnrollmentStatus.REVOKED, result.Status);
            Assert.AreEqual(608, result.Connector.Id);
            Assert.AreEqual("John Doe", result.Debtor.Name);
            Assert.AreEqual("***.456.789-**", result.Debtor.TaxNumber);
            Assert.AreEqual("****4821", result.Debtor.BankAccount.Account);
            Assert.AreEqual("***.456.789-**", result.Cpf);
            Assert.IsNull(result.Cnpj);
            Assert.AreEqual(CUSTOMER_ID, result.CustomerId);
            Assert.AreEqual(1000m, result.Limits.Transaction);
            Assert.AreEqual(149.9m, result.Limits.DailyUsed);
            Assert.IsNotNull(result.ExpiresAt);
            Assert.AreEqual(PixBiometricDataConsentStatus.AUTHORISED, result.DataConsent.Status);
            Assert.IsNull(result.DataConsent.RejectionReason);
            Assert.AreEqual(PixBiometricRevokedBy.CLIENT, result.RevokedBy);
            Assert.IsNull(result.ErrorDetail);
            Assert.IsNotNull(result.CreatedAt);
            Assert.IsNotNull(result.UpdatedAt);
        }

        [Test]
        public async Task FetchPixBiometricEnrollment_DeserializesNullableFieldsAsNull()
        {
            var response = $@"{{ ""id"": ""{ENROLLMENT_ID}"", ""status"": ""CREATED"", ""connector"": null, ""debtor"": null, ""cpf"": null, ""cnpj"": null,
                ""customerId"": null, ""limits"": null, ""expiresAt"": null, ""dataConsent"": null, ""revokedBy"": null,
                ""errorDetail"": {{ ""code"": ""E1"", ""description"": ""Rejected"" }}, ""createdAt"": ""2026-10-28T13:02:00.000Z"", ""updatedAt"": ""2026-10-28T13:02:00.000Z"" }}";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).FetchPixBiometricEnrollment(ENROLLMENT_ID);

            Assert.IsNull(result.Connector);
            Assert.IsNull(result.Debtor);
            Assert.IsNull(result.CustomerId);
            Assert.IsNull(result.Limits);
            Assert.IsNull(result.ExpiresAt);
            Assert.IsNull(result.DataConsent);
            Assert.IsNull(result.RevokedBy);
            Assert.AreEqual("E1", result.ErrorDetail.Code);
        }

        [Test]
        public async Task FetchPixBiometricEnrollmentBalance_RequestsBalanceAndDeserializes()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE,
                @"{ ""balance"": 5.48, ""overdraft"": { ""contracted"": 1000, ""used"": 0, ""available"": 1000 } }");

            var result = await CreateClient(handler).FetchPixBiometricEnrollmentBalance(ENROLLMENT_ID);

            Assert.AreEqual(HttpMethod.Get, handler.RequestMethods[1]);
            Assert.AreEqual($"/pix-biometric/enrollments/{ENROLLMENT_ID}/balance", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(5.48m, result.Balance);
            Assert.AreEqual(1000m, result.Overdraft.Contracted);
            Assert.AreEqual(0m, result.Overdraft.Used);
            Assert.AreEqual(1000m, result.Overdraft.Available);
        }

        [Test]
        public async Task FetchPixBiometricEnrollmentBalance_DeserializesNullOverdraft()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, @"{ ""balance"": 5.48, ""overdraft"": null }");

            var result = await CreateClient(handler).FetchPixBiometricEnrollmentBalance(ENROLLMENT_ID);

            Assert.AreEqual(5.48m, result.Balance);
            Assert.IsNull(result.Overdraft);
        }

        [Test]
        public async Task RevokePixBiometricEnrollment_PostsToRevoke()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, ENROLLMENT_JSON);

            var result = await CreateClient(handler).RevokePixBiometricEnrollment(ENROLLMENT_ID);

            Assert.AreEqual(HttpMethod.Post, handler.RequestMethods[1]);
            Assert.AreEqual($"/pix-biometric/enrollments/{ENROLLMENT_ID}/revoke", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(PixBiometricRevokedBy.CLIENT, result.RevokedBy);
        }

        [Test]
        public async Task CancelPixBiometricEnrollmentDataConsent_SendsDelete()
        {
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, ENROLLMENT_JSON);

            var result = await CreateClient(handler).CancelPixBiometricEnrollmentDataConsent(ENROLLMENT_ID);

            Assert.AreEqual(HttpMethod.Delete, handler.RequestMethods[1]);
            Assert.AreEqual($"/pix-biometric/enrollments/{ENROLLMENT_ID}/data-consent", handler.RequestUris[1].AbsolutePath);
            Assert.AreEqual(ENROLLMENT_ID, result.Id);
        }

        [Test]
        public async Task FetchPaymentIntents_WithEnrollmentId_SendsFilterAndDeserializesPaymentMethod()
        {
            var response = $@"{{ ""total"": 1, ""totalPages"": 1, ""page"": 1, ""results"": [ {{ ""id"": ""{Guid.NewGuid()}"", ""paymentMethod"": ""PIX_BIOMETRIC"", ""enrollmentId"": ""{ENROLLMENT_ID}"" }} ] }}";
            var handler = new MockHttpMessageHandler(AUTH_RESPONSE, response);

            var result = await CreateClient(handler).FetchPaymentIntents(new PaymentIntentParameters { EnrollmentId = ENROLLMENT_ID });

            Assert.AreEqual("/payments/intents", handler.RequestUris[1].AbsolutePath);
            StringAssert.Contains($"enrollmentId={ENROLLMENT_ID}", handler.RequestUris[1].Query);
            Assert.AreEqual(PaymentMethod.PIX_BIOMETRIC, result.Results[0].PaymentMethod);
            Assert.AreEqual(ENROLLMENT_ID, result.Results[0].EnrollmentId);
        }

        [Test]
        public void PaymentIntent_WithoutPaymentMethod_DeserializesNulls()
        {
            var intent = JsonConvert.DeserializeObject<PaymentIntent>($@"{{ ""id"": ""{Guid.NewGuid()}"", ""enrollmentId"": null }}");

            Assert.IsNull(intent.PaymentMethod);
            Assert.IsNull(intent.EnrollmentId);
        }

        [Test]
        public void ConnectorParameters_ToQueryStrings_IncludesSupportsPixBiometricOnlyWhenSet()
        {
            var with = new ConnectorParameters { SupportsPixBiometric = true }.ToQueryStrings();
            var without = new ConnectorParameters().ToQueryStrings();

            Assert.AreEqual("true", with["supportsPixBiometric"]);
            Assert.IsFalse(without.ContainsKey("supportsPixBiometric"));
        }

        [Test]
        public void Connector_DeserializesSupportsPixBiometric()
        {
            var connector = JsonConvert.DeserializeObject<Connector>(@"{ ""id"": 1, ""supportsPixBiometric"": true }");

            Assert.IsTrue(connector.SupportsPixBiometric);
        }

        [Test]
        public void WebhookEventPayload_DeserializesPixBiometricEnrollmentEvent()
        {
            var eventId = Guid.NewGuid();
            var paymentRequestId = Guid.NewGuid();
            var json = $@"{{ ""event"": ""pix_biometric_enrollment/revoked"", ""eventId"": ""{eventId}"", ""enrollmentId"": ""{ENROLLMENT_ID}"", ""customerId"": null, ""paymentRequestId"": ""{paymentRequestId}"" }}";

            var payload = JsonConvert.DeserializeObject<PixBiometricWebhookEventPayload>(json);

            Assert.AreEqual(WebhookEvent.PIX_BIOMETRIC_ENROLLMENT_REVOKED.Value, payload.Event.Value);
            Assert.AreEqual(eventId, payload.EventId);
            Assert.AreEqual(ENROLLMENT_ID, payload.EnrollmentId);
            Assert.IsNull(payload.CustomerId);
            Assert.AreEqual(paymentRequestId, payload.PaymentRequestId);
        }
    }
}
