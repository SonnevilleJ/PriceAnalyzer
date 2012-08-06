namespace Sonneville.PriceTools.SamplePriceData
{
    public static class SamplePriceSeries
    {
        public static PriceSeries DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS
        {
            get
            {
                var priceSeries = PriceSeriesFactory.CreatePriceSeries("IBM");
                var pricePeriods = SamplePriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static PriceSeries DE_1_1_2011_to_6_30_2011
        {
            get
            {
                var priceSeries = PriceSeriesFactory.CreatePriceSeries("DE");
                var pricePeriods = SamplePriceHistoryCsvFiles.DE_1_1_2011_to_6_30_2011.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static PriceSeries MSFT_Apr_June_2011_Weekly_Google
        {
            get
            {
                var priceSeries = PriceSeriesFactory.CreatePriceSeries("MSFT");
                var pricePeriods = SamplePriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }
    }
}
