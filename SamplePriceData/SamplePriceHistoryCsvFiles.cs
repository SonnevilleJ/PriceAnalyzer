using System;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SamplePriceData
{
    public static class SamplePriceHistoryCsvFiles
    {
        public static YahooPriceHistoryCsvFile IBM_1_1_2011_to_3_15_2011_Daily_Yahoo
        {
            get
            {
                var seriesHead = new DateTime(2011, 1, 1);
                var seriesTail = new DateTime(2011, 3, 15, 23, 59, 59);
                return new YahooPriceHistoryCsvFile(new ResourceStream(SampleCsvPriceHistory.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo), seriesHead, seriesTail);
            }
        }

        public static YahooPriceHistoryCsvFile DE_1_1_2011_to_6_30_2011
        {
            get
            {
                var seriesHead = new DateTime(2011, 1, 1);
                var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
                return new YahooPriceHistoryCsvFile(new ResourceStream(SampleCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail);
            }
        }

        public static GooglePriceHistoryCsvFile MSFT_Apr_June_2011_Weekly_Google
        {
            get
            {
                var seriesHead = new DateTime(2011, 4, 1);
                var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
                return new GooglePriceHistoryCsvFile(new ResourceStream(SampleCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google), seriesHead, seriesTail);
            }
        }
    }
}
