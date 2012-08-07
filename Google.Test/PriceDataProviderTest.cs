using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.TestPriceData;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.Google.Test
{
    [TestClass]
    public class PriceDataProviderTest
    {
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
            var updateCount = 0;
            var resetEvent = new AutoResetEvent(false);
            
            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = delegate
            {
                Interlocked.Increment(ref updateCount);
                resetEvent.Set();
                return GetPricePeriods();
            };
            var provider = GetProvider(action);

            provider.StartAutoUpdate(priceSeries);
            resetEvent.WaitOne();
            provider.StopAutoUpdate(priceSeries);

            Assert.IsTrue(updateCount >= 1);
        }

        [TestMethod]
        public void AutoUpdateTwoTickersTest()
        {
            var deere = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var microsoft = PriceSeriesFactory.CreatePriceSeries("MSFT");
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
