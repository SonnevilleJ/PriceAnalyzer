using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.SampleData;
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
            var seriesTail = new DateTime(2011, 6, 30).CurrentPeriodClose(Resolution.Days);     // Thursday
            var stream = new ResourceStream(SamplePriceDatas.Deere.CsvString);
            var pricePeriods = new YahooPriceHistoryCsvFile(stream, seriesHead, seriesTail).PricePeriods;

            var expected = seriesHead.NextTradingPeriodOpen(Resolution.Days);
            var actual = pricePeriods.Min(p => p.Head);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceHistoryCsvFileWillCorrectOverestimatedTailTest()
        {
            var seriesHead = new DateTime(2011, 1, 3);                          // Monday
            var seriesTail = new DateTime(2011, 7, 2).CurrentPeriodClose(Resolution.Days);              // Saturday
            var stream = new ResourceStream(SamplePriceDatas.Deere.CsvString);
            var pricePeriods = new YahooPriceHistoryCsvFile(stream, seriesHead, seriesTail).PricePeriods;

            var expected = new DateTime(2011, 6, 29).CurrentPeriodClose(Resolution.Days);       // Thursday
            var actual = pricePeriods.Max(p => p.Tail);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void YahooDailyTestDates()
        {
            var head = new DateTime(2011, 1, 3);
            var tail = new DateTime(2011, 3, 15).CurrentPeriodClose(Resolution.Days);
            var target = SamplePriceDatas.IBM_Daily.PriceHistory;

            Assert.AreEqual(head, target.PricePeriods.Min(p => p.Head));
            Assert.AreEqual(tail, target.PricePeriods.Max(p => p.Tail));
        }

        [TestMethod]
        public void YahooDailyTestPeriods()
        {
            var target = SamplePriceDatas.IBM_Daily.PriceHistory;

            Assert.AreEqual(50, target.PricePeriods.Count);
        }

        [TestMethod]
        public void YahooDailyTestResolutionIsDays()
        {
            var target = SamplePriceDatas.IBM_Daily.PriceHistory;

            if (target.PricePeriods.Any(p => p.Resolution != Resolution.Days)) Assert.Fail();
        }

        [TestMethod]
        public void YahooDailyTestResolutionIsLessThan24Hours()
        {
            var target = SamplePriceDatas.IBM_Daily.PriceHistory;

            if (target.PricePeriods.Any(p => p.TimeSpan() > new TimeSpan(24, 0, 0))) Assert.Fail();
        }
    }
}
