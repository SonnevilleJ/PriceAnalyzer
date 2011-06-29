using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var days = new List<DateTime> {new DateTime(2011, 4, 1), new DateTime(2011, 4, 4), new DateTime(2011, 4, 5)};
            var results = new List<DateTime>();
            WatcherTriggerDelegate watcherTriggerDelegate = ((sender, e) => results.Add(e.DateTime));

            ExecuteTest(target, watcherTriggerDelegate);

            Assert.IsTrue(ResultsContentsMatch(days, results));
        }

        private static bool ResultsContentsMatch(IEnumerable<DateTime> list1, IEnumerable<DateTime> list2)
        {
            var newlist1 = list1.Select(dateTime => dateTime.Date).ToList();
            var newlist2 = list2.Select(dateTime => dateTime.Date).ToList();
            
            return newlist1.Count == newlist2.Count && newlist1.All(t => newlist2.Contains(t.Date));
        }

        [TestMethod]
        public void PriceUnderThresholdWatcherTest()
        {
            Watcher target = new PriceUnderThresholdWatcher {PriceSeries = _priceSeries, Threshold = 79.0m};
            var days = new List<DateTime> {new DateTime(2011, 6, 16), new DateTime(2011, 6, 17), new DateTime(2011, 6, 20), new DateTime(2011, 6, 23)};
            var results = new List<DateTime>();
            WatcherTriggerDelegate watcherTriggerDelegate = ((sender, e) => results.Add(e.DateTime));

            ExecuteTest(target, watcherTriggerDelegate);

            Assert.IsTrue(ResultsContentsMatch(days, results));
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
