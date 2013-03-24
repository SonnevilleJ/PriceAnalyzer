using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Yahoo;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Yahoo
{
    [TestClass]
    public class PriceRetrievalExtensionsTest
    {
        private readonly IPriceSeriesFactory _priceSeriesFactory;

        public PriceRetrievalExtensionsTest()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        private YahooPriceDataProvider GetTestObject()
        {
            return new YahooPriceDataProvider();
        }

        [TestMethod]
        public void TestDownloadPriceDataHead()
        {
            var dateTime = new DateTime(2011, 4, 1);
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());

            var provider = GetTestObject();
            target.UpdatePriceData(provider, dateTime);

            Assert.IsNotNull(target[dateTime.AddHours(12)]);    // add 12 hours because no price is available at midnight.
        }

        [TestMethod]
        public void TestDownloadPriceDataHeadTail()
        {
            var head = new DateTime(2011, 4, 1);
            var tail = head.AddMonths(1);
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());

            var provider = new YahooPriceDataProvider();
            target.UpdatePriceData(provider, head, tail);

            Assert.IsNotNull(target[tail]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDownloadPriceDataProvidersResolutionIsChecked()
        {
            var provider = new MockProvider();
            var head = new DateTime(2011, 4, 1);
            var tail = head.AddMonths(1);
            var target = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());

            target.UpdatePriceData(provider, head, tail);
        }
    }
}