﻿namespace Sonneville.PriceTools.Test.PriceData
{
    public static class SamplePriceSeries
    {
        private static readonly IPriceSeriesFactory _priceSeriesFactory;

        static SamplePriceSeries()
        {
            _priceSeriesFactory = new PriceSeriesFactory();
        }

        public static IPriceSeries DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS
        {
            get
            {
                var priceSeries = _priceSeriesFactory.ConstructPriceSeries("IBM");
                var pricePeriods = SamplePriceHistoryCsvFiles.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static IPriceSeries DE_1_1_2011_to_6_30_2011
        {
            get
            {
                var priceSeries = _priceSeriesFactory.ConstructPriceSeries("DE");
                var pricePeriods = SamplePriceHistoryCsvFiles.DE_1_1_2011_to_6_30_2011.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }

        public static IPriceSeries MSFT_Apr_June_2011_Weekly_Google
        {
            get
            {
                var priceSeries = _priceSeriesFactory.ConstructPriceSeries("MSFT");
                var pricePeriods = SamplePriceHistoryCsvFiles.MSFT_Apr_June_2011_Weekly_Google.PricePeriods;
                priceSeries.AddPriceData(pricePeriods);
                return priceSeries;
            }
        }
    }
}