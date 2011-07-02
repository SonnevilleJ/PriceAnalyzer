using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceAnalyzer;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;

namespace PriceAnalyzerTest
{
    /// <summary>
    ///This is a test class for PriceOverThresholdWatcherTest and is intended
    ///to contain all PriceOverThresholdWatcherTest Unit Tests
    ///</summary>
    [TestClass]
    public class AnalyzerTest
    {
        private static PriceSeries _priceSeries;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _priceSeries = new GenericPriceHistoryCsvFile(TestData.DE_1_1_2011_to_6_30_2011).PriceSeries;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PriceSeriesAnalyzerThrowsExceptionWhenTimeSeriesIsNotPriceSeries()
        {
            DateTime head = new DateTime(2011, 7, 1);
            DateTime tail = head;
            const decimal close = 5.0m;
            ITimeSeries period = PricePeriodFactory.CreateStaticPricePeriod(head, tail, close);

            new HigherThanYesterdayAnalyzer {TimeSeries = period};
        }

        [TestMethod]
        public void PriceOverThresholdWatcherTest()
        {
            Analyzer target = new PriceOverThresholdAnalyzer {TimeSeries = _priceSeries, Threshold = 99.0m};
            var days = new List<DateTime> {new DateTime(2011, 4, 1), new DateTime(2011, 4, 4), new DateTime(2011, 4, 5)};
            RunWatcherTest(target, days);
        }

        [TestMethod]
        public void PriceUnderThresholdWatcherTest()
        {
            Analyzer target = new PriceUnderThresholdAnalyzer {TimeSeries = _priceSeries, Threshold = 79.0m};
            var days = new List<DateTime> {new DateTime(2011, 6, 16), new DateTime(2011, 6, 17), new DateTime(2011, 6, 20), new DateTime(2011, 6, 23)};
            RunWatcherTest(target, days);
        }

        [TestMethod]
        public void HigherThanYesterdayWatcherTest()
        {
            Analyzer target = new HigherThanYesterdayAnalyzer {TimeSeries = _priceSeries};
            #region Days
            var days = new List<DateTime>
                           {
                               new DateTime(2011, 1, 5),
                               new DateTime(2011, 1, 6),
                               new DateTime(2011, 1, 7),
                               new DateTime(2011, 1, 10),
                               new DateTime(2011, 1, 11),
                               new DateTime(2011, 1, 12),
                               new DateTime(2011, 1, 13),
                               new DateTime(2011, 1, 14),
                               new DateTime(2011, 1, 18),
                               new DateTime(2011, 1, 24),
                               new DateTime(2011, 1, 26),
                               new DateTime(2011, 1, 31),
                               new DateTime(2011, 2, 1),
                               new DateTime(2011, 2, 2),
                               new DateTime(2011, 2, 7),
                               new DateTime(2011, 2, 9),
                               new DateTime(2011, 2, 10),
                               new DateTime(2011, 2, 11),
                               new DateTime(2011, 2, 16),
                               new DateTime(2011, 2, 24),
                               new DateTime(2011, 2, 25),
                               new DateTime(2011, 3, 2),
                               new DateTime(2011, 3, 3),
                               new DateTime(2011, 3, 8),
                               new DateTime(2011, 3, 11),
                               new DateTime(2011, 3, 15),
                               new DateTime(2011, 3, 17),
                               new DateTime(2011, 3, 18),
                               new DateTime(2011, 3, 21),
                               new DateTime(2011, 3, 23),
                               new DateTime(2011, 3, 24),
                               new DateTime(2011, 3, 25),
                               new DateTime(2011, 3, 29),
                               new DateTime(2011, 3, 30),
                               new DateTime(2011, 3, 31),
                               new DateTime(2011, 4, 1),
                               new DateTime(2011, 4, 4),
                               new DateTime(2011, 4, 13),
                               new DateTime(2011, 4, 14),
                               new DateTime(2011, 4, 19),
                               new DateTime(2011, 4, 20),
                               new DateTime(2011, 4, 21),
                               new DateTime(2011, 4, 26),
                               new DateTime(2011, 4, 29),
                               new DateTime(2011, 5, 9),
                               new DateTime(2011, 5, 10),
                               new DateTime(2011, 5, 16),
                               new DateTime(2011, 5, 24),
                               new DateTime(2011, 5, 25),
                               new DateTime(2011, 5, 26),
                               new DateTime(2011, 5, 27),
                               new DateTime(2011, 5, 31),
                               new DateTime(2011, 6, 2),
                               new DateTime(2011, 6, 9),
                               new DateTime(2011, 6, 14),
                               new DateTime(2011, 6, 20),
                               new DateTime(2011, 6, 21),
                               new DateTime(2011, 6, 23),
                               new DateTime(2011, 6, 27),
                               new DateTime(2011, 6, 28),
                               new DateTime(2011, 6, 29)
                           };
            #endregion
            RunWatcherTest(target, days);
        }

        #region Helper Methods

        private static void RunWatcherTest(Analyzer target, IEnumerable<DateTime> days)
        {
            var results = new List<DateTime>();
            AnalyzerTriggerDelegate analyzerTriggerDelegate = ((sender, e) => results.Add(e.DateTime));

            ExecuteTest(target, analyzerTriggerDelegate);

            Assert.IsTrue(ResultsContentsMatch(days, results));
        }

        private static void ExecuteTest(Analyzer target, AnalyzerTriggerDelegate analyzerTriggerDelegate)
        {
            try
            {
                target.TriggerEvent += analyzerTriggerDelegate;

                target.Execute();
            }
            finally
            {
                target.TriggerEvent -= analyzerTriggerDelegate;
            }
        }

        private static bool ResultsContentsMatch(IEnumerable<DateTime> days, IEnumerable<DateTime> list2)
        {
            var results = list2.Select(dateTime => dateTime.Date).OrderBy(result => result.Date).ToList();

            return days.Count() == results.Count && days.All(t => results.Contains(t.Date));
        }

        #endregion
    }
}
