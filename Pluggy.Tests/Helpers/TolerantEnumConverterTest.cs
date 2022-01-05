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
    }
}
