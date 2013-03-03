using TestUtilities.Sonneville.PriceTools;

namespace Sonneville.PriceTools.Test.PriceData
{
    public static class TestPriceSeries
    {
        public static IPriceSeries IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS
        {
            get
            {
                var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
                var pricePeriods = TestPriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static IPriceSeries DE_1_1_2011_to_6_30_2011
        {
            get
            {
                var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
                var pricePeriods = TestPriceHistoryCsvFiles.DE_1_1_2011_to_6_30_2011.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static IPriceSeries MSFT_Apr_June_2011_Weekly_Google
        {
            get
            {
                var priceSeries = PriceSeriesFactory.ConstructPriceSeries(TickerManager.GetUniqueTicker());
                var pricePeriods = TestPriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }
    }
}
