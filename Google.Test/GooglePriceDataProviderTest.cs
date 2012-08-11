﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data.Test;

namespace Sonneville.PriceTools.Google.Test
{
    [TestClass]
    public class GooglePriceDataProviderTest : PriceDataProviderTest
    {
        protected override Data.PriceDataProvider GetTestObjectInstance()
        {
            return new GooglePriceDataProvider();
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
