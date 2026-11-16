using System;
using NUnit.Framework;
using Pluggy.SDK.Model;

namespace Pluggy.Tests.Transactions
{
    [TestFixture]
    public class TransactionParametersTest
    {
        [Test]
        public void UnsetDatesAreNotSent()
        {
            var query = new TransactionParameters { PageSize = 500 }.ToQueryStrings();

            Assert.IsNull(query["from"]);
            Assert.IsNull(query["to"]);
            Assert.AreEqual("500", query["pageSize"]);
        }

        [Test]
        public void SetDatesAreSentAsYyyyMmDd()
        {
            var query = new TransactionParameters
            {
                DateFrom = new DateTime(2026, 1, 1),
                DateTo = new DateTime(2026, 1, 31),
            }.ToQueryStrings();

            Assert.AreEqual("2026-01-01", query["from"]);
            Assert.AreEqual("2026-01-31", query["to"]);
        }
    }
}
