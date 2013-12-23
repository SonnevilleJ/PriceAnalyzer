using System;
using Sonneville.PriceTools.Data.Csv;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.TestPriceData
{
    public static class TestPriceHistoryCsvFiles
    {
        public static PriceHistoryCsvFile IBM_1_1_2011_to_3_15_2011_Daily_Yahoo
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo)); }
        }

        public static PriceHistoryCsvFile DE_1_1_2011_to_6_30_2011
        {
            get { return new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011)); }
        }

        public static PriceHistoryCsvFile MSFT_Apr_June_2011_Weekly_Google
        {
            get { return new GooglePriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google)); }
        }

        public static PriceHistoryCsvFile Build_DE_1_1_2011_to_6_30_2011(DateTime head, DateTime tail)
        {
            return new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), head, tail);
        }
    }
}
