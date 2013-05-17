using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Data;
using TestUtilities.Sonneville.PriceTools;

namespace Test.Sonneville.PriceTools.Data
{
    [TestClass]
    public abstract class PriceDataProviderTest
    {
        private IPriceSeriesFactory _priceSeriesFactory;

        protected abstract PriceDataProvider GetTestObjectInstance();

        [TestInitialize]
        public void Initialize()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        [TestMethod]
        public void DailyDownloadSingleDay()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(1, priceSeries.PricePeriods.Count());
        }

        [TestMethod]
        public void DailyDownloadTestResolution()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(Resolution.Days, priceSeries.Resolution);
            foreach (var period in priceSeries.PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void DailyDownloadTestPeriods()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(50, priceSeries.PricePeriods.Count());
        }

        [TestMethod]
        public void DailyDownloadDates()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Days);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        [TestMethod]
        public void WeeklyDownloadPeriods()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(11, priceSeries.PricePeriods.Count());
        }

        [TestMethod]
        public void WeeklyDownloadResolution()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker, Resolution.Weeks);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(Resolution.Weeks, priceSeries.Resolution);
            var periods = priceSeries.PricePeriods.ToArray();
            for (var i = 1; i < periods.Count() - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void WeeklyDownloadDates()
        {
            var provider = GetTestObjectInstance();
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var ticker = TickerManager.GetUniqueTicker();
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(ticker);
            provider.UpdatePriceSeries(priceSeries, head, tail, Resolution.Weeks);

            Assert.AreEqual(head, priceSeries.Head);
            Assert.AreEqual(tail, priceSeries.Tail);
        }

        [TestMethod]
        public void AutoUpdatePopulatedPriceSeries()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
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

        [TestMethod]
        public void AutoUpdateEmptyPriceSeries()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
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

        [TestMethod]
        public void AutoUpdateTwoTickers()
        {
            var ps1 = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
            var ps2 = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoUpdateSamePriceSeriesTwice()
        {
            var priceSeries = _priceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());

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
