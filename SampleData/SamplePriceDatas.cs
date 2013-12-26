using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SampleData
{
    public static class SamplePriceDatas
    {
        public static ISamplePriceData Deere
        {
            get
            {
                return new SamplePriceData
                {
                    Ticker = "DE",
                    PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceData.DE_1_1_2011_to_6_30_2011)),
                };
            }
        }

        public static ISamplePriceData IBM
        {
            get
            {
                return new SamplePriceData
                {
                    Ticker = "IBM",
                    PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(CsvPriceData.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo)),
                };
            }
        }

        public static ISamplePriceData MSFT
        {
            get
            {
                return new SamplePriceData
                {
                    Ticker = "MSFT",
                    PriceHistory = new GooglePriceHistoryCsvFile(new ResourceStream(CsvPriceData.MSFT_Apr_June_2011_Weekly_Google)),
                };
            }
        }
    }
}