using Newtonsoft.Json;
using NUnit.Framework;
using Pluggy.SDK.Model;

namespace Pluggy.Tests
{
    class InvestmentTypeResult
    {
        public InvestmentType NonNullableTypeWithValidStringValue { get; set; }
        public InvestmentType NonNullableTypeWithValidIntValue { get; set; }
        public InvestmentType NonNullableTypeWithInvalidStringValue { get; set; }
        public InvestmentType NonNullableTypeWithInvalidIntValue { get; set; }
        public InvestmentType NonNullableTypeWithNullValue { get; set; }

        public InvestmentType? NullableTypeWithValidStringValue { get; set; }
        public InvestmentType? NullableTypeWithValidIntValue { get; set; }
        public InvestmentType? NullableTypeWithInvalidStringValue { get; set; }
        public InvestmentType? NullableTypeWithInvalidIntValue { get; set; }
        public InvestmentType? NullableTypeWithNullValue { get; set; }
    }

    [TestFixture]
    public class TolerantEnumConverterTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TolerantEnumConverter_ShouldParseJson()
        {
            string json = @"
            {
                ""NonNullableTypeWithValidStringValue"" : ""SECURITY"",
                ""NonNullableTypeWithValidIntValue"" : 3,
                ""NonNullableTypeWithInvalidStringValue"" : ""Blah"",
                ""NonNullableTypeWithInvalidIntValue"" : 9,
                ""NonNullableTypeWithNullValue"" : null,
                ""NullableTypeWithValidStringValue"" : ""SECURITY"",
                ""NullableTypeWithValidIntValue"" : 3,
                ""NullableTypeWithNullValue"" : null,
                ""NullableTypeWithInvalidStringValue"" : ""Blah"",
                ""NullableTypeWithInvalidIntValue"" : 9
            }";
            InvestmentTypeResult result = JsonConvert.DeserializeObject<InvestmentTypeResult>(json);

            // Default is MUTUAL_FUND (first enum value)
            Assert.AreEqual(result.NonNullableTypeWithInvalidIntValue, InvestmentType.MUTUAL_FUND);
            Assert.AreEqual(result.NonNullableTypeWithInvalidStringValue, InvestmentType.MUTUAL_FUND);
            Assert.AreEqual(result.NonNullableTypeWithNullValue, InvestmentType.MUTUAL_FUND);
            Assert.AreEqual(result.NonNullableTypeWithValidIntValue, InvestmentType.FIXED_INCOME);
            Assert.AreEqual(result.NonNullableTypeWithValidStringValue, InvestmentType.SECURITY);
            Assert.AreEqual(result.NullableTypeWithInvalidIntValue, null);
            Assert.AreEqual(result.NullableTypeWithInvalidStringValue, null);
            Assert.AreEqual(result.NullableTypeWithNullValue, null);
            Assert.AreEqual(result.NullableTypeWithValidIntValue, InvestmentType.FIXED_INCOME);
            Assert.AreEqual(result.NullableTypeWithValidStringValue, InvestmentType.SECURITY);

        }

        [Test]
        public void CurrencyCode_ShouldFallBackToBRL_ForUnknownCode()
        {
            // "GHA" is not a valid ISO 4217 code (Ghana's currency is GHS) and is not
            // present in the CurrencyCode enum. It must not throw; it falls back to BRL.
            var account = JsonConvert.DeserializeObject<Account>(@"{ ""currencyCode"": ""GHA"" }");
            Assert.AreEqual(CurrencyCode.BRL, account.CurrencyCode);
        }

        [Test]
        public void CurrencyCode_ShouldParseKnownCode()
        {
            var account = JsonConvert.DeserializeObject<Account>(@"{ ""currencyCode"": ""USD"" }");
            Assert.AreEqual(CurrencyCode.USD, account.CurrencyCode);
        }

        [TestCase("ACCOUNTS", ProductType.Accounts)]
        [TestCase("CREDIT_CARDS", ProductType.CreditCards)]
        [TestCase("TRANSACTIONS", ProductType.Transactions)]
        [TestCase("PAYMENT_DATA", ProductType.PaymentData)]
        [TestCase("INVESTMENTS", ProductType.Investments)]
        [TestCase("INVESTMENTS_TRANSACTIONS", ProductType.InvestmentsTransactions)]
        [TestCase("BROKERAGE_NOTE", ProductType.BrokerageNote)]
        [TestCase("LOANS", ProductType.Loans)]
        [TestCase("EXCHANGE_OPERATIONS", ProductType.ExchangeOperations)]
        [TestCase("IDENTITY", ProductType.Identity)]
        [TestCase("MOVE_SECURITY", ProductType.MoveSecurity)]
        [TestCase("credit_cards", ProductType.CreditCards)]
        public void ProductType_ShouldParseWireValue(string wireValue, ProductType expected)
        {
            var item = JsonConvert.DeserializeObject<Item>($@"{{ ""products"": [""{wireValue}""] }}");
            Assert.AreEqual(expected, item.Products[0]);
        }

        [Test]
        public void ProductType_ShouldStillParseMemberName()
        {
            // Earlier SDK versions wrote the member name, so stored JSON may still contain it.
            var item = JsonConvert.DeserializeObject<Item>(@"{ ""products"": [""CreditCards""] }");
            Assert.AreEqual(ProductType.CreditCards, item.Products[0]);
        }

        [TestCase(ProductType.CreditCards, "CREDIT_CARDS")]
        [TestCase(ProductType.MoveSecurity, "MOVE_SECURITY")]
        public void ProductType_ShouldWriteWireValue(ProductType value, string expected)
        {
            Assert.AreEqual($"\"{expected}\"", JsonConvert.SerializeObject(value));
        }

        [Test]
        public void EnumWithoutEnumMember_ShouldWriteMemberName()
        {
            Assert.AreEqual("\"SECURITY\"", JsonConvert.SerializeObject(InvestmentType.SECURITY));
        }

        [Test]
        public void UndefinedEnumValue_ShouldWriteItsNumber()
        {
            Assert.AreEqual("\"99\"", JsonConvert.SerializeObject((ProductType)99));
        }

        [Test]
        public void Loan_ShouldParseInstallmentFields_OasSpelling()
        {
            var loan = JsonConvert.DeserializeObject<Loan>(@"
            {
                ""installmentPeriodicity"": ""MONTHLY"",
                ""installmentPeriodicityAdditionalInfo"": ""info"",
                ""firstInstallmentDueDate"": ""2024-01-15T00:00:00.000Z""
            }");
            Assert.AreEqual(LoanInstalmentPeriodicity.MONTHLY, loan.InstalmentPeriodicity);
            Assert.AreEqual("info", loan.InstalmentPeriodicityAdditionalInfo);
            Assert.IsNotNull(loan.FirstInstalmentDueDate);
        }

        [Test]
        public void Loan_ShouldParseInstallmentFields_LegacySpelling()
        {
            // Older payloads used the British "instalment" (single L). Must still bind.
            var loan = JsonConvert.DeserializeObject<Loan>(@"
            {
                ""instalmentPeriodicity"": ""MONTHLY"",
                ""instalmentPeriodicityAdditionalInfo"": ""info"",
                ""firstInstalmentDueDate"": ""2024-01-15T00:00:00.000Z""
            }");
            Assert.AreEqual(LoanInstalmentPeriodicity.MONTHLY, loan.InstalmentPeriodicity);
            Assert.AreEqual("info", loan.InstalmentPeriodicityAdditionalInfo);
            Assert.IsNotNull(loan.FirstInstalmentDueDate);
        }
    }
}
