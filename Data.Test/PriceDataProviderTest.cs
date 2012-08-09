using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TestPriceData;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Data.Test
{
    [TestClass]
    public abstract class PriceDataProviderTest
    {
        protected abstract PriceDataProvider GetTestObjectInstance();

        [TestMethod]
        public abstract void DailyDownloadSingleDay();

        [TestMethod]
        public abstract void DailyDownloadTestResolution();

        [TestMethod]
        public abstract void DailyDownloadTestPeriods();

        [TestMethod]
        public abstract void DailyDownloadDates();

        [TestMethod]
        public abstract void WeeklyDownloadPeriods();

        [TestMethod]
        public abstract void WeeklyDownloadResolution();

        [TestMethod]
        public abstract void WeeklyDownloadDates();

        protected void DailyDownloadSingleDayTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(1, priceSeries.PricePeriods.Count);
        }

        protected void DailyDownloadResolutionTest()
        {
            var provider = GetTestObjectInstance();
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

        protected void DailyDownloadPeriodsTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(50, priceSeries.PricePeriods.Count);
        }

        protected void DailyDownloadDatesTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        protected void WeeklyDownloadPeriodsTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(11, priceSeries.PricePeriods.Count);
        }

        protected void WeeklyDownloadResolutionTest()
        {
            var provider = GetTestObjectInstance();
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

        protected void WeeklyDownloadDatesTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var ticker = TestUtilities.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        [TestMethod]
        public void AutoUpdateTest()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;
            var locker = new object();

            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = delegate
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

            Assert.IsTrue(updateCount >= 1);
        }

        [TestMethod]
        public void AutoUpdateEmptyPriceSeriesTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var resetEvent = new AutoResetEvent(false);
            
            EventHandler<NewDataAvailableEventArgs> action = delegate { resetEvent.Set(); };
            var provider = GetTestObjectInstance();

            Assert.IsTrue(priceSeries.PricePeriods.Count == 0);

            priceSeries.NewDataAvailable += action;
            provider.StartAutoUpdate(priceSeries);
            resetEvent.WaitOne();

            priceSeries.NewDataAvailable -= action;
            provider.StopAutoUpdate(priceSeries);

            Assert.IsTrue(priceSeries.PricePeriods.Count > 0);
        }

        [TestMethod]
        public void AutoUpdateTwoTickersTest()
        {
            var deere = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var microsoft = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var deereUpdates = 0;
            var microsoftUpdates = 0;
            var countdown = new CountdownEvent(2);

            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = (ticker, head, tail, resolution) =>
            {
                if (ticker == deere.Ticker && deereUpdates == 0)
                {
                    Interlocked.Increment(ref deereUpdates);
                    countdown.Signal();
                }
                if (ticker == microsoft.Ticker && microsoftUpdates == 0)
                {
                    Interlocked.Increment(ref microsoftUpdates);
                    countdown.Signal();
                }
                return GetPricePeriods();
            };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(deere);
            provider.StartAutoUpdate(microsoft);
            countdown.Wait();

            provider.StopAutoUpdate(deere);
            provider.StopAutoUpdate(microsoft);

            Assert.IsTrue(deereUpdates >= 1 && microsoftUpdates >= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoUpdateSamePriceSeriesTwiceTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());

            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = delegate
            {
                return GetPricePeriods();
            };
            var provider = GetProvider(action);

            try
            {
                provider.StartAutoUpdate(priceSeries);
                provider.StartAutoUpdate(priceSeries);
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
            }
        }

        private static IEnumerable<PricePeriod> GetPricePeriods()
        {
            return new List<PricePeriod>
                       {
                           PricePeriodFactory.ConstructStaticPricePeriod(
                               new DateTime(2011, 12, 27),
                               new DateTime(2011, 12, 27).GetFollowingClose(),
                               100.00m)
                       };
        }

        [TestMethod]
        public void AutoUpdatePriceSeriesEventsTest()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
            {
                Interlocked.Increment(ref updateCount);
                resetEvent.Set();
            };
            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = delegate
            {
                return GetPricePeriods();
            };
            var provider = GetProvider(action);

            try
            {
                priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(priceSeries);

                resetEvent.WaitOne();
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
                priceSeries.NewDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount >= 1);
        }

        [TestMethod]
        public void AutoUpdateCancelDoesntSleepFullTimeoutTest()
        {
            var priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
            {
                Interlocked.Increment(ref updateCount);
                resetEvent.Set();
            };
            IPriceDataProvider provider = new WeeklyProvider {UpdateAction = delegate { return GetPricePeriods(); }};

            try
            {
                priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(priceSeries);

                resetEvent.WaitOne();
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
                priceSeries.NewDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount >= 1);
        }

        private static IPriceDataProvider GetProvider(Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = null)
        {
            return new SecondsProvider { UpdateAction = action };
        }
    }
}
