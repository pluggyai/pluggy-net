using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Model;

namespace Pluggy.Tests.Webhooks
{
    [TestFixture]
    public class WebhookModelTest
    {
        [Test]
        public void Webhook_Deserializes_MaskedHeaders()
        {
            var webhook = JsonConvert.DeserializeObject<Webhook>(@"{
                ""id"": ""8f1e1c9a-3b2d-4c5e-9f6a-7b8c9d0e1f2a"",
                ""event"": ""item/updated"",
                ""url"": ""https://example.com/hook"",
                ""headers"": { ""Authorization"": ""****"" }
            }");

            Assert.AreEqual(1, webhook.Headers.Count);
            Assert.AreEqual("****", webhook.Headers["Authorization"]);
        }

        [Test]
        public void Webhook_Headers_NullWhenAbsent()
        {
            var webhook = JsonConvert.DeserializeObject<Webhook>(@"{ ""url"": ""https://example.com/hook"" }");

            Assert.IsNull(webhook.Headers);
        }

        [Test]
        public void ApiError_Deserializes_CodeDescription()
        {
            var apiError = JsonConvert.DeserializeObject<ApiError>(@"{
                ""code"": 403,
                ""codeDescription"": ""WEBHOOKS_API_FEATURE_NOT_ENABLED"",
                ""message"": ""Webhook management through the API is not enabled for this team""
            }");

            Assert.AreEqual("WEBHOOKS_API_FEATURE_NOT_ENABLED", apiError.CodeDescription);
            Assert.AreEqual("Webhook management through the API is not enabled for this team", apiError.Message);
        }
    }
}
