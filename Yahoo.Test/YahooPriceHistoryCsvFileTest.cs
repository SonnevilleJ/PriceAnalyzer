using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.Test.PriceData;
using Sonneville.Utilities;

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
            var pricePeriods = new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PricePeriods;

            var expected = seriesHead.GetFollowingOpen();
            var actual = pricePeriods.Min(p => p.Head);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectOverestimatedTailTest()
        {
            var seriesHead = new DateTime(2011, 1, 3);                          // Monday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var pricePeriods = new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PricePeriods;

            var expected = new DateTime(2011, 6, 29).GetFollowingClose();       // Thursday
            var actual = pricePeriods.Max(p => p.Tail);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectWeekendTailWeeklyTest()
        {
            var seriesHead = new DateTime(2011, 4, 1);                          // Friday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var pricePeriods = new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google), seriesHead, seriesTail).PricePeriods;

            var expected = seriesTail.GetMostRecentWeeklyClose();               // Friday
            var actual = pricePeriods.Max(p => p.Tail);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void YahooDailyTestDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15, 23, 59, 59);
            var target = TestPriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(head, target.PricePeriods.Min(p => p.Head));
            Assert.AreEqual(tail, target.PricePeriods.Max(p => p.Tail));
        }

        [TestMethod]
        public void YahooDailyTestPeriods()
        {
            var target = TestPriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo;

            Assert.AreEqual(50, target.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDailyTestResolution()
        {
            var target = TestPriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo;

            foreach (var period in target.PricePeriods)
            {
                Assert.AreEqual(Resolution.Days, period.Resolution);
                Assert.IsTrue(period.Tail - period.Head < new TimeSpan(24, 0, 0));
            }
        }
    }
}
