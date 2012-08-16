using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Yahoo;
using Test.Sonneville.PriceTools.Data;

namespace Test.Sonneville.PriceTools.Yahoo
{
    [TestClass]
    public class YahooPriceDataProviderTest : PriceDataProviderTest
    {
        protected override PriceDataProvider GetTestObjectInstance()
        {
            return new YahooPriceDataProvider();
        }

        [TestMethod]
        public override void DailyDownloadSingleDay()
        {
            DailyDownloadSingleDayTest();
        }

        [TestMethod]
        public override void DailyDownloadTestResolution()
        {
            DailyDownloadResolutionTest();
        }

        [TestMethod]
        public override void DailyDownloadTestPeriods()
        {
            DailyDownloadPeriodsTest();
        }

        [TestMethod]
        public override void DailyDownloadDates()
        {
            DailyDownloadDatesTest();
        }

        [TestMethod]
        public override void WeeklyDownloadPeriods()
        {
            WeeklyDownloadPeriodsTest();
        }

        [TestMethod]
        public override void WeeklyDownloadResolution()
        {
            WeeklyDownloadResolutionTest();
        }

        [TestMethod]
        public override void WeeklyDownloadDates()
        {
            WeeklyDownloadDatesTest();
        }

        [TestMethod]
        public override void AutoUpdatePopulatedPriceSeries()
        {
            AutoUpdatePopulatedPriceSeriesTest();
        }

        [TestMethod]
        public override void AutoUpdateEmptyPriceSeries()
        {
            AutoUpdateEmptyPriceSeriesTest();
        }

        [TestMethod]
        public override void AutoUpdateTwoTickers()
        {
            AutoUpdateTwoTickersTest();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public override void AutoUpdateSamePriceSeriesTwice()
        {
            AutoUpdateSamePriceSeriesTwiceTest();
        }
    }
}
