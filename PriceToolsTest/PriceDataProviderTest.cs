using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class PriceDataProviderTest
    {
        [TestMethod]
        public void YahooDownloadDailyTestResolution()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);

            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            Assert.AreEqual(Resolution.Days, target.PriceSeries.Resolution);
            foreach (var period in ((PriceSeries)target.PriceSeries).PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void YahooDownloadDailyTestPeriods()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);

            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDownloadDailyTestDates()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);

            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            // verify dates
            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void YahooDownloadWeeklyTestPeriods()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(11, target.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDownloadWeeklyTestResolution()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;
            for (var i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void YahooDownloadWeeklyTestDates()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void GoogleDownloadDailyTestResolution()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            Assert.AreEqual(Resolution.Days, target.PriceSeries.Resolution);
            foreach (var period in ((PriceSeries)target.PriceSeries).PricePeriods)
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
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadDailyTestDates()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Days);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestPeriods()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(11, target.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestResolution()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;
            for (var i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleDownloadWeeklyTestDates()
        {
            var provider = new GooglePriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceHistoryCsvFile("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void AutoUpdateTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            var updateCount = 0;

            Action<IPriceSeries> action = delegate { Interlocked.Increment(ref updateCount); };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(priceSeries);
            Thread.Sleep(new TimeSpan(((long)provider.BestResolution) * 5));
            provider.StopAutoUpdate(priceSeries);

            Assert.IsTrue(updateCount > 0);
        }

        [TestMethod]
        public void AutoUpdateTwoTickersTest()
        {
            const string de = "DE";
            const string msft = "MSFT";
            var deere = PriceSeriesFactory.CreatePriceSeries(de);
            var microsoft = PriceSeriesFactory.CreatePriceSeries(msft);
            var deereUpdates = 0;
            var microsoftUpdates = 0;

            Action<IPriceSeries> action = priceSeries =>
                                              {
                                                  if (priceSeries.Ticker == de)
                                                      Interlocked.Increment(ref deereUpdates);
                                                  if (priceSeries.Ticker == msft)
                                                      Interlocked.Increment(ref microsoftUpdates);
                                              };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(deere);
            provider.StartAutoUpdate(microsoft);
            Thread.Sleep(new TimeSpan(((long)provider.BestResolution) * 5));
            provider.StopAutoUpdate(deere);
            provider.StopAutoUpdate(microsoft);

            Assert.IsTrue(deereUpdates > 0 && microsoftUpdates > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoUpdateSamePriceSeriesTwiceTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            var updateCount = 0;

            Action<IPriceSeries> action = delegate { Interlocked.Increment(ref updateCount); };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(priceSeries);
            try
            {
                provider.StartAutoUpdate(priceSeries);
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
            }
        }

        private static IPriceDataProvider GetProvider(Action<IPriceSeries> action = null)
        {
            return new SecondsProvider {UpdateAction = action};
        }
    }
}
