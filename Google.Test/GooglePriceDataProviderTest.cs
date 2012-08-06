using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Google.Test
{
    [TestClass]
    public class GooglePriceDataProviderTest
    {

        [TestMethod]
        public void GoogleDownloadDailyTestResolution()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(Resolution.Days, priceSeries.Resolution);
            foreach (var period in priceSeries.PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleDownloadDailyTestPeriods()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(50, priceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadDailyTestDates()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestPeriods()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(11, priceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestResolution()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker, Resolution.Weeks);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, priceSeries.Resolution);
            var periods = priceSeries.PricePeriods;
            for (var i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleDownloadWeeklyDoesNotReturnAfterImplied()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }
    }
}
