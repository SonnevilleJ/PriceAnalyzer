using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Yahoo;

namespace Sonneville.PriceTools.SamplePriceData
{
    public static class SamplePriceHistoryCsvFiles
    {
        public static YahooPriceHistoryCsvFile IBM_1_1_2011_to_3_15_2011_Daily_Yahoo
        {
            get { return new YahooPriceHistoryCsvFile("IBM", new ResourceStream(SampleCsvPriceHistory.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo)); }
        }

        public static YahooPriceHistoryCsvFile DE_1_1_2011_to_6_30_2011
        {
            get { return new YahooPriceHistoryCsvFile("DE", new ResourceStream(SampleCsvPriceHistory.DE_1_1_2011_to_6_30_2011)); }
        }

        public static GooglePriceHistoryCsvFile MSFT_Apr_June_2011_Weekly_Google
        {
            get { return new GooglePriceHistoryCsvFile("MSFT", new ResourceStream(SampleCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google)); }
        }
    }
}
