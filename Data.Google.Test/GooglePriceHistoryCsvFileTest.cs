using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Data.Google.Test
{
    [TestClass]
    public class GooglePriceHistoryCsvFileTest
    {
        [TestMethod]
        public void GoogleWeeklyTestPeriods()
        {
            PriceHistoryCsvFile target = new GooglePriceHistoryCsvFile("DE", CsvPriceHistory.DE_Apr_June_2011_Weekly_Google);

            Assert.AreEqual(14, target.PricePeriods.Count);
        }

        [TestMethod]
        public void GoogleWeeklyTestResolution()
        {
            PriceHistoryCsvFile target = new GooglePriceHistoryCsvFile("DE", CsvPriceHistory.DE_Apr_June_2011_Weekly_Google);

            Assert.AreEqual(Resolution.Weeks, target.PriceSeries.Resolution);
            var periods = target.PricePeriods;

            for (var i = 1; i < periods.Count - 1; i++) // skip check on first and last periods
            {
                Assert.IsTrue(periods[i].Tail - periods[i].Head >= new TimeSpan(23, 59, 59));
                Assert.IsTrue(periods[i].Tail - periods[i].Head < new TimeSpan(7, 0, 0, 0));
            }
        }

        [TestMethod]
        public void GoogleWeeklyTestDates()
        {
            var head = new DateTime(2011, 4, 1);
            var tail = new DateTime(2011, 7, 1, 23, 59, 59);
            PriceHistoryCsvFile target = new GooglePriceHistoryCsvFile("DE", CsvPriceHistory.DE_Apr_June_2011_Weekly_Google, head, tail);

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }
    }
}
