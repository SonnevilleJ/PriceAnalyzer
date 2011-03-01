using Sonneville.PriceTools;
using Sonneville.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceQuoteTest and is intended
    ///to contain all PriceQuoteTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PriceQuoteTest
    {
        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod()]
        public void PriceTest()
        {
            IPriceQuote target = new PriceQuote();

            const decimal expected = 100.00m;
            target.Price = expected;
            decimal actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod()]
        public void QuotedDateTimeTest()
        {
            IPriceQuote target = new PriceQuote();

            DateTime expected = new DateTime(2011, 2, 24);
            target.SettlementDate = expected;
            DateTime actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod()]
        public void VolumeTest()
        {
            IPriceQuote target = new PriceQuote();

            long? expected = long.MaxValue;
            target.Volume = expected;
            long? actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializePriceQuoteTest()
        {
            DateTime quotedDateTime = new DateTime(2011, 2, 28);
            const int price = 10;
            const int volume = 1000;

            IPriceQuote target = new PriceQuote
                                     {
                                         SettlementDate = quotedDateTime,
                                         Price = price,
                                         Volume = volume
                                     };

            IPriceQuote actual = ((IPriceQuote) TestUtilities.Serialize(target));
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void EntityPriceSeriesTest()
        {
            DateTime quotedDateTime = new DateTime(2011, 2, 28);
            const int price = 10;
            const int volume = 1000;

            IPriceQuote target = new PriceQuote
            {
                SettlementDate = quotedDateTime,
                Price = price,
                Volume = volume
            };

            TestUtilities.VerifyPriceQuoteEntity(target);
        }
    }
}
