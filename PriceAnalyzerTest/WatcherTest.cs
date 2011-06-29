using System.Collections.Generic;
using Sonneville.PriceAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sonneville.PriceTools;

namespace PriceAnalyzerTest
{
    /// <summary>
    ///This is a test class for PriceOverThresholdWatcherTest and is intended
    ///to contain all PriceOverThresholdWatcherTest Unit Tests
    ///</summary>
    [TestClass]
    public class WatcherTest
    {
        private static PriceSeries _priceSeries;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            _priceSeries.DownloadPriceData(new DateTime(2011, 1, 1), new DateTime(2011, 6, 24));
        }

        [TestMethod]
        public void PriceOverThresholdWatcherTest()
        {
            Watcher target = new PriceOverThresholdWatcher {PriceSeries = _priceSeries, Threshold = 99.0m};
            IList<DateTime> days = new List<DateTime> {new DateTime(2011, 4, 1), new DateTime(2011, 4, 4), new DateTime(2011, 4, 5)};
            int count = 0;
            WatcherTriggerDelegate watcherTriggerDelegate = ((sender, e) =>
                                                   {
                                                       count++;
                                                       Assert.IsTrue(days.Contains(new DateTime(e.DateTime.Year, e.DateTime.Month, e.DateTime.Day)));
                                                   });

            ExecuteTest(target, watcherTriggerDelegate);

            Assert.AreEqual(days.Count, count);
        }

        [TestMethod]
        public void PriceUnderThresholdWatcherTest()
        {
            Watcher target = new PriceUnderThresholdWatcher {PriceSeries = _priceSeries, Threshold = 79.0m};
            IList<DateTime> days = new List<DateTime>
                                       {new DateTime(2011, 6, 16), new DateTime(2011, 6, 17), new DateTime(2011, 6, 20), new DateTime(2011, 6, 23)};
            int count = 0;
            WatcherTriggerDelegate watcherTriggerDelegate = ((sender, e) =>
                                                                 {
                                                                     count++;
                                                                     Assert.IsTrue(days.Contains(new DateTime(e.DateTime.Year, e.DateTime.Month, e.DateTime.Day)));
                                                                 });

            ExecuteTest(target, watcherTriggerDelegate);

            Assert.AreEqual(days.Count, count);
        }

        private static void ExecuteTest(Watcher target, WatcherTriggerDelegate watcherTriggerDelegate)
        {
            try
            {
                target.TriggerEvent += watcherTriggerDelegate;

                target.Execute();
            }
            finally
            {
                target.TriggerEvent -= watcherTriggerDelegate;
            }
        }
    }
}
