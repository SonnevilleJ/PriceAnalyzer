using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Services;
using Sonneville.Utilities;

namespace Sonneville.PriceToolsTest
{
    [TestClass]
    public class PriceSeriesProviderTest
    {
        [TestMethod]
        public void YahooDownloadTest()
        {
            var provider = new YahooPriceSeriesProvider();
            PriceHistoryCsvFile target = GetPriceHistoryCsvFile(provider);
            TestUtilities.VerifyDailyPriceHistoryData(target);
        }

        [TestMethod]
        public void YahooDownloadPriceSeriesTest()
        {
            var provider = new YahooPriceSeriesProvider();
            IPriceSeries target = GetPriceSeries(provider);
            TestUtilities.VerifyDateRange(target);
        }

        [TestMethod]
        public void GoogleDownloadTest()
        {
            var provider = new GooglePriceSeriesProvider();
            PriceHistoryCsvFile target = GetPriceHistoryCsvFile(provider);
            TestUtilities.VerifyDailyPriceHistoryData(target);
        }

        [TestMethod]
        public void GoogleDownloadPriceSeriesTest()
        {
            var provider = new GooglePriceSeriesProvider();
            var priceSeries = GetPriceSeries(provider);
            TestUtilities.VerifyDateRange(priceSeries);
        }

        private static PriceHistoryCsvFile GetPriceHistoryCsvFile(PriceSeriesProvider provider)
        {
            return provider.GetPriceHistoryCsvFile(TestUtilities.TickerToVerify, TestUtilities.HeadToVerify, TestUtilities.TailToVerify);
        }

        private static IPriceSeries GetPriceSeries(PriceSeriesProvider provider)
        {
            return GetPriceHistoryCsvFile(provider).PriceSeries;
        }
    }
}
