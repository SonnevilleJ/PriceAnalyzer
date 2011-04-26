using System.IO;
using Sonneville.PriceTools.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceHistoryCsvFileTest and is intended
    ///to contain all PriceHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceHistoryCsvFileTest
    {
        private const string Ticker = "DE";
        private static readonly DateTime Head = new DateTime(2011, 1, 3);
        private static readonly DateTime Tail = new DateTime(2011, 3, 15);
        private const int PeriodCount = 50;

        /// <summary>
        ///A test for GetPriceSeries
        ///</summary>
        [TestMethod]
        public void YahooResxTest()
        {
            var testData = TestData.DE_PriceData_Yahoo;
            using (var stream = new MemoryStream(testData))
            {
                PriceHistoryCsvFile target = new GenericPriceHistoryCsvFile(stream);
                TestPriceHistoryData(target);
            }
        }

        [TestMethod]
        public void YahooDownloadTest()
        {
            var provider = new YahooPriceSeriesProvider();
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile(Ticker, Head, Tail);
            TestPriceHistoryData(target);
        }

        [TestMethod]
        public void YahooPriceSeriesTest()
        {
            var provider = new YahooPriceSeriesProvider();
            var priceSeries = provider.GetPriceSeries(Ticker, Head, Tail);
            TestDateRange(priceSeries);
        }

        [TestMethod]
        public void GoogleDownloadTest()
        {
            var provider = new GooglePriceSeriesProvider();
            PriceHistoryCsvFile target = provider.GetPriceHistoryCsvFile(Ticker, Head, Tail);
            TestPriceHistoryData(target);
        }

        [TestMethod]
        public void GooglePriceSeriesTest()
        {
            var provider = new GooglePriceSeriesProvider();
            var priceSeries = provider.GetPriceSeries(Ticker, Head, Tail);
            TestDateRange(priceSeries);
        }

        private static void TestPriceHistoryData(PriceHistoryCsvFile target)
        {
            Assert.AreEqual(PeriodCount, target.PricePeriods.Count);

            IPriceSeries series = PriceSeriesFactory.CreatePriceSeries(Ticker);
            foreach (var period in target.PricePeriods)
            {
                series.PricePeriods.Add(period);
            }

            TestDateRange(series);
        }

        private static void TestDateRange(IPriceSeries series)
        {
            Assert.AreEqual(Head, series.Head);
            Assert.AreEqual(Tail.AddDays(1), series.Tail);
        }
    }
}
