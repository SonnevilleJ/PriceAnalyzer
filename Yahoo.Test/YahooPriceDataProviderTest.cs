using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools.Data;
using Sonneville.PriceTools.Data.Test;

namespace Sonneville.PriceTools.Yahoo.Test
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
    }
}
