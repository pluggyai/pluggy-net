using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK.Model;

namespace Pluggy.Tests.Models
{
    [TestFixture]
    public class ApiSyncFieldsTest
    {
        [Test]
        public void Connector_Deserializes_ProductCoverage()
        {
            var connector = JsonConvert.DeserializeObject<Connector>(@"{
                ""id"": 601,
                ""productCoverage"": [""INVESTMENTS:TREASURE_TITLES"", ""CREDIT_OPERATIONS:INVOICE_FINANCINGS""]
            }");

            Assert.AreEqual(2, connector.ProductCoverage.Count);
            Assert.AreEqual("INVESTMENTS:TREASURE_TITLES", connector.ProductCoverage[0]);
            Assert.AreEqual("CREDIT_OPERATIONS:INVOICE_FINANCINGS", connector.ProductCoverage[1]);
        }

        [Test]
        public void Connector_ProductCoverage_NullWhenAbsent()
        {
            var connector = JsonConvert.DeserializeObject<Connector>(@"{ ""id"": 2 }");

            Assert.IsNull(connector.ProductCoverage);
        }

        [Test]
        public void CreditAccount_Deserializes_AdditionalCards()
        {
            var creditData = JsonConvert.DeserializeObject<CreditAccount>(@"{
                ""brand"": ""VISA"",
                ""additionalCards"": [ { ""number"": ""1234"" }, { ""number"": ""5678"" } ]
            }");

            Assert.AreEqual(2, creditData.AdditionalCards.Count);
            var numbers = creditData.AdditionalCards.ToList();
            Assert.AreEqual("1234", numbers[0].Number);
            Assert.AreEqual("5678", numbers[1].Number);
        }

        [Test]
        public void BankAccount_Deserializes_OverdraftUsedLimit()
        {
            var bankData = JsonConvert.DeserializeObject<BankAccount>(@"{
                ""transferNumber"": ""0001/12345-0"",
                ""overdraftUsedLimit"": 250.5
            }");

            Assert.AreEqual(250.5, bankData.OverdraftUsedLimit);
        }

        [Test]
        public void TransactionPaymentData_Deserializes_AuthenticationCode()
        {
            var paymentData = JsonConvert.DeserializeObject<TransactionPaymentData>(@"{
                ""paymentMethod"": ""PIX"",
                ""authenticationCode"": ""A1B2C3D4E5F6""
            }");

            Assert.AreEqual("A1B2C3D4E5F6", paymentData.AuthenticationCode);
        }

        [Test]
        public void Investment_Deserializes_New_Fields()
        {
            var investment = JsonConvert.DeserializeObject<Investment>(@"{
                ""id"": ""a3e71d92-5b48-4c6f-8e20-1d9c3b7a4e50"",
                ""itemId"": ""d0e8448e-0156-4b4a-ae6c-3e2a6d9bff5c"",
                ""type"": ""FIXED_INCOME"",
                ""balance"": 1000,
                ""name"": ""CRA Example"",
                ""currencyCode"": ""BRL"",
                ""date"": ""2026-09-01T00:00:00.000Z"",
                ""couponPayment"": { ""hasCoupon"": true, ""periodicity"": ""IRREGULAR"", ""additionalInfo"": ""Paid on demand"" },
                ""debtor"": { ""name"": ""Agro Example SA"" },
                ""gracePeriodDate"": ""2027-01-15T00:00:00.000Z"",
                ""priceFactor"": 100,
                ""taxExempt"": true
            }");

            Assert.AreEqual(true, investment.CouponPayment.HasCoupon);
            Assert.AreEqual("IRREGULAR", investment.CouponPayment.Periodicity);
            Assert.AreEqual("Paid on demand", investment.CouponPayment.AdditionalInfo);
            Assert.AreEqual("Agro Example SA", investment.Debtor.Name);
            Assert.AreEqual(new DateTime(2027, 1, 15, 0, 0, 0, DateTimeKind.Utc), investment.GracePeriodDate.Value.ToUniversalTime());
            Assert.AreEqual(100, investment.PriceFactor);
            Assert.AreEqual(true, investment.TaxExempt);
        }

        [Test]
        public void Investment_New_Fields_NullWhenNull()
        {
            var investment = JsonConvert.DeserializeObject<Investment>(@"{
                ""id"": ""a3e71d92-5b48-4c6f-8e20-1d9c3b7a4e50"",
                ""couponPayment"": null,
                ""debtor"": null,
                ""gracePeriodDate"": null,
                ""priceFactor"": null,
                ""taxExempt"": null
            }");

            Assert.IsNull(investment.CouponPayment);
            Assert.IsNull(investment.Debtor);
            Assert.IsNull(investment.GracePeriodDate);
            Assert.IsNull(investment.PriceFactor);
            Assert.IsNull(investment.TaxExempt);
        }
    }
}
