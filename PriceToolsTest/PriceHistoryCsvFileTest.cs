using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;
using Sonneville.PriceTools.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void YahooDailyTestDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void YahooDailyTestPeriods()
        {
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;
            
            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDailyTestResolution()
        {
            PriceHistoryCsvFile target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;
            
            Assert.AreEqual(Resolution.Days, target.PriceSeries.Resolution);
            foreach (var period in ((PriceSeries)target.PriceSeries).PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectWeekendHeadTest()
        {
            var seriesHead = new DateTime(2011, 1, 1);                          // Saturday
            var seriesTail = new DateTime(2011, 6, 30).GetFollowingClose();     // Thursday
            var priceSeries = new YahooPriceHistoryCsvFile("DE", new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PriceSeries;

            var expected = seriesHead.GetFollowingOpen();
            var actual = priceSeries.Head;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectWeekendTailTest()
        {
            var seriesHead = new DateTime(2011, 1, 3);                          // Monday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var priceSeries = new YahooPriceHistoryCsvFile("DE", new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PriceSeries;

            var expected = seriesTail.GetMostRecentClose();
            var actual = priceSeries.Tail;
            Assert.AreEqual(expected, actual);
        }

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
