using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.SamplePriceData;
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
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;

            Action<IPriceSeries, DateTime, DateTime> action = delegate { Interlocked.Increment(ref updateCount); };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(priceSeries);
            Thread.Sleep(new TimeSpan(((long)provider.BestResolution) * 5));
            provider.StopAutoUpdate(priceSeries);

            Assert.IsTrue(updateCount > 0);
        }

        [TestMethod]
        public void AutoUpdateEmptyPriceSeriesTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            var updateCount = 0;

            Action<IPriceSeries, DateTime, DateTime> action = delegate { Interlocked.Increment(ref updateCount); };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(priceSeries);
            Thread.Sleep(new TimeSpan(((long)provider.BestResolution) * 5));
            provider.StopAutoUpdate(priceSeries);

            Assert.IsTrue(updateCount > 0);
        }

        [TestMethod]
        public void AutoUpdateTwoTickersTest()
        {
            var deere = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var microsoft = PriceSeriesFactory.CreatePriceSeries("MSFT");
            var deereUpdates = 0;
            var microsoftUpdates = 0;

            Action<IPriceSeries, DateTime, DateTime> action = (priceSeries, head, tail) =>
                                              {
                                                  if (priceSeries.Ticker == deere.Ticker)
                                                      Interlocked.Increment(ref deereUpdates);
                                                  if (priceSeries.Ticker == microsoft.Ticker)
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

            Action<IPriceSeries, DateTime, DateTime> action = delegate { Interlocked.Increment(ref updateCount); };
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

        [TestMethod]
        public void AutoUpdatePriceSeriesEventsTest()
        {
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;
            var locker = new object();

            EventHandler<NewPriceDataAvailableEventArgs> handler = (sender, args) =>
                                                                       {
                                                                           Interlocked.Increment(ref updateCount);
                                                                           lock (locker) Monitor.Pulse(locker);
                                                                       };

            try
            {
                priceSeries.NewPriceDataAvailable += handler;
                var provider = new GooglePriceDataProvider();

                provider.StartAutoUpdate(priceSeries);
                
                // wait until event has processed
                lock (locker) Monitor.Wait(locker);

                provider.StopAutoUpdate(priceSeries);

                const int expected = 1;
                var actual = updateCount;
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                priceSeries.NewPriceDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount > 0);
        }

        private static IPriceDataProvider GetProvider(Action<IPriceSeries, DateTime, DateTime> action = null)
        {
            return new SecondsProvider {UpdateAction = action};
        }
    }
}
