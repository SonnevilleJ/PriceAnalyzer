using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Yahoo.Test
{
    [TestClass]
    public class YahooPriceHistoryCsvFileTest
    {
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
        public void PriceHistoryCsvFileWillCorrectOverestimatedTailTest()
        {
            var seriesHead = new DateTime(2011, 1, 3);                          // Monday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var priceSeries = new YahooPriceHistoryCsvFile("DE", new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PriceSeries;

            var expected = new DateTime(2011, 6, 29).GetFollowingClose();       // Thursday
            var actual = priceSeries.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectWeekendTailWeeklyTest()
        {
            var seriesHead = new DateTime(2011, 4, 1);                          // Friday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var priceSeries = new YahooPriceHistoryCsvFile("DE", new ResourceStream(CsvPriceHistory.DE_Apr_June_2011_Weekly_Google), seriesHead, seriesTail).PriceSeries;

            var expected = seriesTail.GetMostRecentWeeklyClose();               // Friday
            var actual = priceSeries.Tail;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void YahooDailyTestDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(head, target.PriceSeries.Head);
            Assert.AreEqual(tail, target.PriceSeries.Tail);
        }

        [TestMethod]
        public void YahooDailyTestPeriods()
        {
            var target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(50, target.PriceSeries.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDailyTestResolution()
        {
            var target = PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(Resolution.Days, target.PriceSeries.Resolution);
            foreach (var period in ((PriceSeries)target.PriceSeries).PricePeriods)
            {
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }
    }
}
