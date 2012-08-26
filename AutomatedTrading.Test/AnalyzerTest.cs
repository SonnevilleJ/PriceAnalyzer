using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.AutomatedTrading;
using Sonneville.PriceTools.Test.PriceData;

namespace Test.Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///This is a test class for PriceOverThresholdAnalyzerTest and is intended
    ///to contain all PriceOverThresholdAnalyzerTest Unit Tests
    ///</summary>
    [TestClass]
    public class AnalyzerTest
    {
        private static PriceSeries _priceSeries;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _priceSeries = TestPriceSeries.DE_1_1_2011_to_6_30_2011;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PriceSeriesAnalyzerThrowsExceptionWhenTimeSeriesIsNotPriceSeries()
        {
            var head = new DateTime(2011, 7, 1);
            var tail = head;
            const decimal close = 5.0m;
            TimeSeries period = PricePeriodFactory.ConstructStaticPricePeriod(head, tail, close);

            new HigherThanYesterdayAnalyzer {TimeSeries = period};
        }

        [TestMethod]
        public void PriceOverThresholdAnalyzerTest()
        {
            Analyzer target = new PriceOverThresholdAnalyzer {TimeSeries = _priceSeries, Threshold = 99.0m};
            var days = new List<DateTime> {new DateTime(2011, 4, 1), new DateTime(2011, 4, 4), new DateTime(2011, 4, 5)};
            RunAnalyzerTest(target, days);
        }

        [TestMethod]
        public void PriceUnderThresholdAnalyzerTest()
        {
            Analyzer target = new PriceUnderThresholdAnalyzer {TimeSeries = _priceSeries, Threshold = 79.0m};
            var days = new List<DateTime> {new DateTime(2011, 6, 16), new DateTime(2011, 6, 17), new DateTime(2011, 6, 20), new DateTime(2011, 6, 23)};
            RunAnalyzerTest(target, days);
        }

        [TestMethod]
        public void HigherThanYesterdayAnalyzerTest()
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
            RunAnalyzerTest(target, days);
        }

        [TestMethod]
        public void LowerThanYesterdayAnalyzerTest()
        {
            Analyzer target = new LowerThanYesterdayAnalyzer { TimeSeries = _priceSeries };
            #region Days

            var days = new List<DateTime>
                           {
                               new DateTime(2011, 1, 4),
                               new DateTime(2011, 1, 19),
                               new DateTime(2011, 1, 20),
                               new DateTime(2011, 1, 21),
                               new DateTime(2011, 1, 25),
                               new DateTime(2011, 1, 27),
                               new DateTime(2011, 1, 28),
                               new DateTime(2011, 2, 3),
                               new DateTime(2011, 2, 4),
                               new DateTime(2011, 2, 8),
                               new DateTime(2011, 2, 14),
                               new DateTime(2011, 2, 15),
                               new DateTime(2011, 2, 17),
                               new DateTime(2011, 2, 18),
                               new DateTime(2011, 2, 22),
                               new DateTime(2011, 2, 23),
                               new DateTime(2011, 2, 28),
                               new DateTime(2011, 3, 1),
                               new DateTime(2011, 3, 4),
                               new DateTime(2011, 3, 7),
                               new DateTime(2011, 3, 9),
                               new DateTime(2011, 3, 10),
                               new DateTime(2011, 3, 14),
                               new DateTime(2011, 3, 16),
                               new DateTime(2011, 3, 22),
                               new DateTime(2011, 3, 28),
                               new DateTime(2011, 4, 5),
                               new DateTime(2011, 4, 6),
                               new DateTime(2011, 4, 7),
                               new DateTime(2011, 4, 8),
                               new DateTime(2011, 4, 11),
                               new DateTime(2011, 4, 12),
                               new DateTime(2011, 4, 15),
                               new DateTime(2011, 4, 18),
                               new DateTime(2011, 4, 25),
                               new DateTime(2011, 4, 27),
                               new DateTime(2011, 4, 28),
                               new DateTime(2011, 5, 2),
                               new DateTime(2011, 5, 3),
                               new DateTime(2011, 5, 4),
                               new DateTime(2011, 5, 5),
                               new DateTime(2011, 5, 6),
                               new DateTime(2011, 5, 11),
                               new DateTime(2011, 5, 12),
                               new DateTime(2011, 5, 13),
                               new DateTime(2011, 5, 17),
                               new DateTime(2011, 5, 18),
                               new DateTime(2011, 5, 19),
                               new DateTime(2011, 5, 20),
                               new DateTime(2011, 5, 23),
                               new DateTime(2011, 6, 1),
                               new DateTime(2011, 6, 3),
                               new DateTime(2011, 6, 6),
                               new DateTime(2011, 6, 7),
                               new DateTime(2011, 6, 8),
                               new DateTime(2011, 6, 10),
                               new DateTime(2011, 6, 13),
                               new DateTime(2011, 6, 15),
                               new DateTime(2011, 6, 16),
                               new DateTime(2011, 6, 17),
                               new DateTime(2011, 6, 22),
                               new DateTime(2011, 6, 24)
                           };
            #endregion
            RunAnalyzerTest(target, days);
        }

        #region Helper Methods

        private static void RunAnalyzerTest(Analyzer target, IEnumerable<DateTime> days)
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
