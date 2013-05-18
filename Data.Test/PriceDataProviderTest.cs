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
        private IPriceSeries _priceSeries;
        private string _ticker;

        protected abstract PriceDataProvider GetTestObjectInstance();

        [TestInitialize]
        public void Initialize()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
            _ticker = TickerManager.GetUniqueTicker();
            _priceSeries = _priceSeriesFactory.ConstructPriceSeries(_ticker);
        }

        [TestMethod]
        public void DailyDownloadSingleDay()
        {
            var head = new DateTime(2012, 8, 7);
            var tail = new DateTime(2012, 8, 7).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 1, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadResolution()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(0, 0, 1);
            var maxTimeSpan = new TimeSpan(24, 0, 0);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 50, Resolution.Days);
        }

        [TestMethod]
        public void DailyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Days);
        }

        [TestMethod]
        public void WeeklyDownloadPeriods()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadPeriodsTest(head, tail, 11, Resolution.Weeks);
        }

        [TestMethod]
        public void WeeklyDownloadResolution()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var minTimeSpan = new TimeSpan(7, 0, 0, 0);
            var maxTimeSpan = new TimeSpan(23, 59, 59);

            DownloadResolutionTest(head, tail, minTimeSpan, maxTimeSpan, Resolution.Weeks);
        }

        [TestMethod]
        public void WeeklyDownloadDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);

            DownloadDatesTest(head, tail, Resolution.Weeks);
        }

        private void DownloadPeriodsTest(DateTime head, DateTime tail, int expected, Resolution resolution)
        {
            var provider = GetTestObjectInstance();
            provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);
            Assert.AreEqual(expected, _priceSeries.PricePeriods.Count());
        }

        private void DownloadResolutionTest(DateTime head, DateTime tail, TimeSpan minTimeSpan, TimeSpan maxTimeSpan, Resolution resolution)
        {
            var provider = GetTestObjectInstance();
            provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);

            Assert.AreEqual(resolution, _priceSeries.Resolution);
            var periods = _priceSeries.PricePeriods.ToArray();
            for (var i = 1; i < periods.Count() - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= minTimeSpan);
                Assert.IsTrue(periods[i].Tail - periods[i].Head < maxTimeSpan);
            }
        }

        private void DownloadDatesTest(DateTime head, DateTime tail, Resolution resolution)
        {
            var provider = GetTestObjectInstance();
            provider.UpdatePriceSeries(_priceSeries, head, tail, resolution);

            Assert.AreEqual(head, _priceSeries.Head);
            Assert.AreEqual(tail, _priceSeries.Tail);
        }

        [TestMethod]
        public void AutoUpdatePopulatedPriceSeries()
        {
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
                {
                    Interlocked.Increment(ref updateCount);
                    resetEvent.Set();
                };
            IPriceDataProvider provider = GetTestObjectInstance();

            var head = new DateTime(2012, 6, 6);
            var tail = new DateTime(2012, 6, 9).CurrentPeriodClose(_priceSeries.Resolution);
            provider.UpdatePriceSeries(_priceSeries, head, tail);

            try
            {
                _priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(_priceSeries);

                resetEvent.WaitOne(new TimeSpan(0, 0, 30));
            }
            finally
            {
                provider.StopAutoUpdate(_priceSeries);
                _priceSeries.NewDataAvailable -= handler;
            }

            Assert.IsTrue(updateCount >= 1);
        }

        [TestMethod]
        public void AutoUpdateEmptyPriceSeries()
        {
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
                _priceSeries.NewDataAvailable += handler;
                provider.StartAutoUpdate(_priceSeries);

                resetEvent.WaitOne(new TimeSpan(0, 0, 30));
            }
            finally
            {
                provider.StopAutoUpdate(_priceSeries);
                _priceSeries.NewDataAvailable -= handler;
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
            var provider = GetTestObjectInstance();

            try
            {
                provider.StartAutoUpdate(_priceSeries);
                provider.StartAutoUpdate(_priceSeries);
            }
            finally
            {
                provider.StopAutoUpdate(_priceSeries);
            }
        }
    }
}
