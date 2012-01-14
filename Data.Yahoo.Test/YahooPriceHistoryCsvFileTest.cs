using System;
using Data.Yahoo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.SamplePriceData;

namespace Sonneville.PriceTools.Data.Yahoo.Test
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
        public void PriceHistoryCsvFileWillCorrectWeekendTailTest()
        {
            var seriesHead = new DateTime(2011, 1, 3);                          // Monday
            var seriesTail = new DateTime(2011, 7, 2, 23, 59, 59);              // Saturday
            var priceSeries = new YahooPriceHistoryCsvFile("DE", new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail).PriceSeries;

            var expected = seriesTail.GetMostRecentClose();
            var actual = priceSeries.Tail;
            Assert.AreEqual(expected, actual);
        }
    }
}
