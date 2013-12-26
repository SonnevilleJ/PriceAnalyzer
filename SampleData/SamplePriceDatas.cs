using Sonneville.PriceTools.Google;
using Sonneville.PriceTools.Yahoo;
using Sonneville.Utilities;

namespace Sonneville.PriceTools.SampleData
{
    public static class SamplePriceDatas
    {
        public static SamplePriceData Deere
        {
            get
            {
                var csvString = CsvPriceData.DE_1_1_2011_to_6_30_2011;
                return new SamplePriceData
                {
                    Ticker = "DE",
                    CsvString = csvString,
                    PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(csvString)),
                };
            }
        }

        public static SamplePriceData IBM_Daily
        {
            get
            {
                var csvString = CsvPriceData.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo;
                return new SamplePriceData
                {
                    Ticker = "IBM",
                    CsvString = csvString,
                    PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(csvString)),
                };
            }
        }

        public static SamplePriceData IBM_Weekly
        {
            get
            {
                var csvString = CsvPriceData.IBM_1_1_2011_to_3_15_2011_Weekly_Yahoo;
                return new SamplePriceData
                {
                    Ticker = "IBM",
                    CsvString = csvString,
                    PriceHistory = new YahooPriceHistoryCsvFile(new ResourceStream(csvString)),
                };
            }
        }

        public static SamplePriceData IBM_SingleDay
        {
            get
            {
                var csvString = CsvPriceData.IBM_8_7_2012_to_8_7_2012_Daily_Yahoo;
                return new SamplePriceData
                {
                    Ticker = "IBM",
                    CsvString = csvString
                };
            }
        }

        public static SamplePriceData MSFT
        {
            get
            {
                var csvString = CsvPriceData.MSFT_Apr_June_2011_Weekly_Google;
                return new SamplePriceData
                {
                    Ticker = "MSFT",
                    CsvString = csvString,
                    PriceHistory = new GooglePriceHistoryCsvFile(new ResourceStream(csvString)),
                };
            }
        }
    }
}