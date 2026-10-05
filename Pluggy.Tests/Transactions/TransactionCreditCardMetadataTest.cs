using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK.Model;

namespace Pluggy.Tests.Transactions
{
    [TestFixture]
    public class TransactionCreditCardMetadataTest
    {
        [Test]
        public void Deserializes_PaymentType_BillPostDate_And_TransactionDateTime()
        {
            var json = @"{
                ""installmentNumber"": 2,
                ""totalInstallments"": 10,
                ""billForecastDate"": ""2026-11"",
                ""paymentType"": ""INSTALLMENT"",
                ""billPostDate"": ""2026-10-12"",
                ""transactionDateTime"": ""2026-04-09T16:43:35.203Z""
            }";

            var metadata = JsonConvert.DeserializeObject<TransactionCreditCardMetadata>(json);

            Assert.AreEqual(CreditCardAccountPaymentType.INSTALLMENT, metadata.PaymentType);
            Assert.AreEqual("2026-10-12", metadata.BillPostDate);
            // Kept verbatim: an ISO timestamp must not be reformatted into a culture-specific date string.
            Assert.AreEqual("2026-04-09T16:43:35.203Z", metadata.TransactionDateTime);
        }

        [Test]
        public void Null_BillPostDate_And_Missing_Fields_Deserialize_As_Null()
        {
            var json = @"{ ""installmentNumber"": 1, ""billPostDate"": null }";

            var metadata = JsonConvert.DeserializeObject<TransactionCreditCardMetadata>(json);

            Assert.IsNull(metadata.BillPostDate);
            Assert.IsNull(metadata.PaymentType);
            Assert.IsNull(metadata.TransactionDateTime);
        }

        [Test]
        public void Unknown_PaymentType_Does_Not_Break_Deserialization()
        {
            var json = @"{ ""paymentType"": ""SOMETHING_NEW"" }";

            var metadata = JsonConvert.DeserializeObject<TransactionCreditCardMetadata>(json);

            Assert.IsNull(metadata.PaymentType);
        }
    }
}
