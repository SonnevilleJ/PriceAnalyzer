using Sonneville.PriceTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceQuoteTest and is intended
    ///to contain all PriceQuoteTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceQuoteTest
    {
        /// <summary>
        ///A test for Price
        ///</summary>
        [TestMethod]
        public void PriceTest()
        {
            const decimal expected = 100.00m;
            
            var target = new PriceQuote {Price = expected};
            
            var actual = target.Price;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SettlementDate
        ///</summary>
        [TestMethod]
        public void QuotedDateTimeTest()
        {
            var expected = new DateTime(2011, 2, 24);
            
            var target = new PriceQuote {SettlementDate = expected};

            var actual = target.SettlementDate;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Volume
        ///</summary>
        [TestMethod]
        public void VolumeTest()
        {
            const long expected = long.MaxValue;
            
            var target = new PriceQuote {Volume = expected};

            var actual = target.Volume;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var settlementDate = new DateTime(2011, 12, 28);
            const decimal price = 10.00m;
            const long volume = 300;

            var target = new PriceQuote {SettlementDate = settlementDate, Price = price, Volume = volume};

            var actual = target.ToString();

            Assert.IsTrue(actual.Contains(settlementDate.ToString()));
            Assert.IsTrue(actual.Contains(price.ToString()));
        }
    }
}
