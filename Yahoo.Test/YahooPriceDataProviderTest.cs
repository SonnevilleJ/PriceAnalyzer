using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Yahoo.Test
{
    [TestClass]
    public class YahooPriceDataProviderTest
    {
        [TestMethod]
        public void YahooDownloadDailyTestResolution()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);

            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Days);

            Assert.AreEqual(Resolution.Days, target.Resolution);
            foreach (var period in target.PricePeriods)
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

            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Days);

            Assert.AreEqual(50, target.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDownloadDailyTestDates()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);

            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Days);

            // verify dates
            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public void YahooDownloadWeeklyTestPeriods()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(11, target.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDownloadWeeklyTestResolution()
        {
            var provider = new YahooPriceDataProvider();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, target.Resolution);
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
            var target = provider.GetPriceSeries("DE", head, tail, Resolution.Weeks);

            Assert.AreEqual(head, target.Head);
            Assert.AreEqual(tail, target.Tail);
        }

        [TestMethod]
        public void AutoUpdateTest()
        {
            var priceSeries = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;
            var locker = new object();

            Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> action = delegate
                                                                                                 {
                                                                                                     Interlocked.Increment(ref updateCount);
                                                                                                     lock(locker) Monitor.Pulse(locker);
                                                                                                     return GetPricePeriods();
                                                                                                 };
            var provider = GetProvider(action);

            lock (locker)
            {
                provider.StartAutoUpdate(priceSeries);
                Monitor.Wait(locker);
            }
            provider.StopAutoUpdate(priceSeries);

            Assert.AreEqual(1, updateCount);
        }

        [TestMethod]
        public void AutoUpdateEmptyPriceSeriesTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            var updateCount = 0;
            var locker = new object();

            Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> action = delegate
                                                                                                 {
                                                                                                     Interlocked.Increment(ref updateCount);
                                                                                                     lock (locker) Monitor.Pulse(locker);
                                                                                                     return GetPricePeriods();
                                                                                                 };
            var provider = GetProvider(action);

            lock (locker)
            {
                provider.StartAutoUpdate(priceSeries);
                Monitor.Wait(locker);
            }
            provider.StopAutoUpdate(priceSeries);

            Assert.AreEqual(1, updateCount);
        }

        [TestMethod]
        public void AutoUpdateTwoTickersTest()
        {
            var deere = SamplePriceSeries.DE_1_1_2011_to_6_30_2011;
            var microsoft = PriceSeriesFactory.CreatePriceSeries("MSFT");
            var deereUpdates = 0;
            var microsoftUpdates = 0;
            var locker = new object();

            Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> action = (ticker, head, tail, resolution) =>
                                              {
                                                  if (ticker == deere.Ticker)
                                                      Interlocked.Increment(ref deereUpdates);
                                                  if (ticker == microsoft.Ticker)
                                                      Interlocked.Increment(ref microsoftUpdates);
                                                  lock(locker) Monitor.Pulse(locker);
                                                  return GetPricePeriods();
                                              };
            var provider = GetProvider(action);

            lock (locker)
            {
                provider.StartAutoUpdate(deere);
                provider.StartAutoUpdate(microsoft);
                Monitor.Wait(locker);
            }
            provider.StopAutoUpdate(deere);
            provider.StopAutoUpdate(microsoft);

            Assert.IsTrue(deereUpdates == 1 && microsoftUpdates == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoUpdateSamePriceSeriesTwiceTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            var updateCount = 0;

            Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> action = delegate
                                                                                                 {
                                                                                                     Interlocked.Increment(ref updateCount);
                                                                                                     return GetPricePeriods();
                                                                                                 };
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

        private static IEnumerable<IPricePeriod> GetPricePeriods()
        {
            return new List<IPricePeriod>
                       {
                           PricePeriodFactory.CreateStaticPricePeriod(
                               new DateTime(2011, 12, 27),
                               new DateTime(2011, 12, 27).GetFollowingClose(),
                               100.00m)
                       };
        }

        private static IPriceDataProvider GetProvider(Func<string, DateTime, DateTime, Resolution, IEnumerable<IPricePeriod>> action = null)
        {
            return new SecondsProvider {UpdateAction = action};
        }
    }
}
