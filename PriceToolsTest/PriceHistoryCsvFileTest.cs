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
            TestUtilities.VerifyDailyPriceHistoryData(target);
        }

        [TestMethod]
        public void GoogleWeeklyTest()
        {
            PriceHistoryCsvFile target = new GenericPriceHistoryCsvFile(TestData.DE_Apr_June2011_Weekly_Google);
            TestUtilities.VerifyWeeklyPriceHistoryData(target);
        }
    }
}
