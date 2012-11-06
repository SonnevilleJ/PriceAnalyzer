using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Extensions;

namespace Test.Sonneville.PriceTools.Data
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

        [TestMethod]
        public abstract void AutoUpdatePopulatedPriceSeries();

        [TestMethod]
        public abstract void AutoUpdateEmptyPriceSeries();

        [TestMethod]
        public abstract void AutoUpdateTwoTickers();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public abstract void AutoUpdateSamePriceSeriesTwice();

        protected void DailyDownloadSingleDayTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(1, priceSeries.PricePeriods.Count());
        }

        protected void DailyDownloadResolutionTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
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
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(50, priceSeries.PricePeriods.Count());
        }

        protected void DailyDownloadDatesTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        protected void WeeklyDownloadPeriodsTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(11, priceSeries.PricePeriods.Count());
        }

        protected void WeeklyDownloadResolutionTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker, Resolution.Weeks);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, priceSeries.Resolution);
            var periods = priceSeries.PricePeriods.ToArray();
            for (var i = 1; i < periods.Count() - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        protected void WeeklyDownloadDatesTest()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker();
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        protected void AutoUpdatePopulatedPriceSeriesTest()
        {
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
            {
                Interlocked.Increment(ref updateCount);
                resetEvent.Set();
            };
            IPriceDataProvider provider = GetTestObjectInstance();

            var head = new DateTime(2012, 6, 6);
            var tail = new DateTime(2012, 6, 9).CurrentPeriodClose(priceSeries.Resolution);
            provider.UpdatePriceSeries(priceSeries, head, tail);

            try
            {
                priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(priceSeries);

                resetEvent.WaitOne(new TimeSpan(0, 0, 30));
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
                priceSeries.NewDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount >= 1);
        }

        protected void AutoUpdateEmptyPriceSeriesTest()
        {
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
            {
                Interlocked.Increment(ref updateCount);
                resetEvent.Set();
            };
            IPriceDataProvider provider = GetTestObjectInstance();

            try
            {
                priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(priceSeries);

                resetEvent.WaitOne(new TimeSpan(0, 0, 30));
            }
            finally
            {
                provider.StopAutoUpdate(priceSeries);
                priceSeries.NewDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount >= 1);
        }

        protected void AutoUpdateTwoTickersTest()
        {
            var ps1 = PriceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var ps2 = PriceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());
            var deereUpdates = 0;
            var microsoftUpdates = 0;
            var countdown = new CountdownEvent(2);

            ps1.NewDataAvailable += (o, args) =>
                                        {
                                            if (deereUpdates == 0)
                                            {
                                                Interlocked.Increment(ref deereUpdates);
                                                countdown.Signal();
                                            }
                                        };
            ps2.NewDataAvailable += (o, args) =>
                                        {
                                            if (microsoftUpdates == 0)
                                            {
                                                Interlocked.Increment(ref microsoftUpdates);
                                                countdown.Signal();
                                            }
                                        };
            var provider = GetTestObjectInstance();

            provider.StartAutoUpdate(ps1);
            provider.StartAutoUpdate(ps2);
            countdown.Wait(new TimeSpan(0, 0, 30));

            provider.StopAutoUpdate(ps1);
            provider.StopAutoUpdate(ps2);

            Assert.IsTrue(deereUpdates >= 1 && microsoftUpdates >= 1);
        }

        protected void AutoUpdateSamePriceSeriesTwiceTest()
        {
            var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TestUtilities.Sonneville.PriceTools.TickerManager.GetUniqueTicker());

            var provider = GetTestObjectInstance();

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
    }
}
