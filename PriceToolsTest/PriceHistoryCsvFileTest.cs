using System;
using System.Collections.Generic;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    /// <summary>
    ///This is a test class for PriceHistoryCsvFileTest and is intended
    ///to contain all PriceHistoryCsvFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class PriceHistoryCsvFileTest
    {
        [TestMethod]
        public void YahooDailyTest()
        {
            PriceHistoryCsvFile target = new GenericPriceHistoryCsvFile(TestData.DE_1_1_2011_to_3_15_2011_Daily_Yahoo);
            
            // verify dates
            Assert.AreEqual(new DateTime(2011, 1, 3), target.PriceSeries.Head);
            Assert.AreEqual(new DateTime(2011, 3, 15).AddHours(23).AddMinutes(59).AddSeconds(59), target.PriceSeries.Tail);

            // verify periods
            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);

            // verify resolution
            Assert.AreEqual(PriceSeriesResolution.Days, target.PriceSeries.Resolution);
            foreach (var period in target.PriceSeries.DataPeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleWeeklyTest()
        {
            var head = new DateTime(2011, 4, 1);
            var tail = new DateTime(2011, 7, 1).AddHours(23).AddMinutes(59).AddSeconds(59);
            PriceHistoryCsvFile target = new GenericPriceHistoryCsvFile(TestData.DE_Apr_June2011_Weekly_Google);

            // verify periods
            Assert.AreEqual(14, target.PricePeriods.Count);

            // verify resolution
            Assert.AreEqual(PriceSeriesResolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;

            for (int i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }

            // verify dates
            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }
    }
}
