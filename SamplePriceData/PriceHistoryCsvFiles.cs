using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools.SamplePriceData
{
    public static class PriceHistoryCsvFiles
    {
        public static YahooPriceHistoryCsvFile DE_1_1_2011_to_3_15_2011_Daily_Yahoo
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_3_15_2011_Daily_Yahoo)); }
        }

        public static YahooPriceHistoryCsvFile DE_1_1_2011_to_6_30_2011
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceHistory.DE_1_1_2011_to_6_30_2011)); }
        }

        public static YahooPriceHistoryCsvFile DE_Apr_June_2011_Weekly_Google
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceHistory.DE_Apr_June_2011_Weekly_Google)); }
        }

        public static YahooPriceHistoryCsvFile SPX_8_Dec_2010_to_10_Dec_2010
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceHistory.SPX_8_Dec_2010_to_10_Dec_2010)); }
        }
    }
}
