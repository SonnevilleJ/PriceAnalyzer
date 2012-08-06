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
        public void AutoUpdateTwoTickersTest()
        {
            var deere = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
            var microsoft = PriceSeriesFactory.CreatePriceSeries("MSFT");
            var deereUpdates = 0;
            var microsoftUpdates = 0;
            var locker = new object();

            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = (ticker, head, tail, resolution) =>
            {
                if (ticker == deere.Ticker)
                    Interlocked.Increment(ref deereUpdates);
                if (ticker == microsoft.Ticker)
                    Interlocked.Increment(ref microsoftUpdates);
                lock (locker) Monitor.Pulse(locker);
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

            Assert.IsTrue(deereUpdates >= 1 && microsoftUpdates >= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoUpdateSamePriceSeriesTwiceTest()
        {
            var priceSeries = PriceSeriesFactory.CreatePriceSeries(TestUtilities.GetUniqueTicker());
            var updateCount = 0;

            Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = delegate
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
            var locker = new object();

            EventHandler<NewDataAvailableEventArgs> handler = (sender, args) =>
            {
                Interlocked.Increment(ref updateCount);
                lock (locker) Monitor.Pulse(locker);
            };

            try
            {
                priceSeries.NewDataAvailable += handler;
                var provider = new GooglePriceDataProvider();

                lock (locker)
                {
                    provider.StartAutoUpdate(priceSeries);

                    // wait until event has processed
                    Monitor.Wait(locker);
                }

                provider.StopAutoUpdate(priceSeries);

                Assert.IsTrue(updateCount >= 1);
            }
            finally
            {
                priceSeries.NewDataAvailable -= handler;
            }
        }

        private static IPriceDataProvider GetProvider(Func<string, DateTime, DateTime, Resolution, IEnumerable<PricePeriod>> action = null)
        {
            return new SecondsProvider { UpdateAction = action };
        }
    }
}
