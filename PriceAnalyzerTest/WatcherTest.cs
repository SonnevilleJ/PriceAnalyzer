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
        [TestMethod]
        public void PriceOverThresholdWatcherTest()
        {
            Watcher target = new PriceOverThresholdWatcher();
            var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
            priceSeries.DownloadPriceData(new DateTime(2011, 1, 1));
            target.PriceSeries = priceSeries;
            target.Threshold = 99.0m;

            IList<DateTime> days = new List<DateTime> {new DateTime(2011, 4, 1), new DateTime(2011, 4, 4), new DateTime(2011, 4, 5)};

            int count = 0;
            TriggerDelegate triggerDelegate = ((sender, e) =>
                                                   {
                                                       count++;
                                                       Assert.IsTrue(days.Contains(new DateTime(e.DateTime.Year, e.DateTime.Month, e.DateTime.Day)));
                                                   });
            ExecuteTest(target, triggerDelegate);
            Assert.AreEqual(days.Count, count);
        }

        private static void ExecuteTest(Watcher target, TriggerDelegate triggerDelegate)
        {
            try
            {
                target.TriggerEvent += triggerDelegate;

                target.Execute();
            }
            finally
            {
                target.TriggerEvent -= triggerDelegate;
            }
        }
    }
}
