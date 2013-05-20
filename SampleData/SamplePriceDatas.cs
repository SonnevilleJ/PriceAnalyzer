using System;
using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Test.PriceData;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace SampleData
{
    public static class SamplePriceDatas
    {
        public static ISamplePriceData Deere
        {
            get
            {
                var seriesHead = new DateTime(2011, 1, 1);
                var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
                var data = new SamplePriceData
                    {
                        Ticker = "DE",
                        PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.DE_1_1_2011_to_6_30_2011), seriesHead, seriesTail),
                    };
                return data;
            }
        }

        public static ISamplePriceData IBM
        {
            get
            {
                var seriesHead = new DateTime(2011, 1, 1);
                var seriesTail = new DateTime(2011, 3, 15, 23, 59, 59);
                var data = new SamplePriceData
                    {
                        Ticker = "IBM",
                        PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo), seriesHead, seriesTail),
                    };
                return data;
            }
        }

        public static ISamplePriceData MSFT
        {
            get
            {
                var seriesHead = new DateTime(2011, 4, 1);
                var seriesTail = new DateTime(2011, 6, 30, 23, 59, 59);
                var data = new SamplePriceData
                    {
                        Ticker = "MSFT",
                        PriceHistory = new GooglePriceHistoryCsvFile(new ResourceStream(TestCsvPriceHistory.MSFT_Apr_June_2011_Weekly_Google), seriesHead, seriesTail),
                    };
                return data;
            }
        }
    }
}