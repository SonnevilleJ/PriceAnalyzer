namespace Sonneville.PriceTools.SamplePriceData
{
    public static class SamplePriceSeries
    {
        public static IPriceSeries DE_1_1_2011_to_3_15_2011_Daily_Yahoo_PS
        {
            get { return PriceHistoryCsvFiles.DE_1_1_2011_to_3_15_2011_Daily_Yahoo.PriceSeries; }
        }

        public static IPriceSeries DE_1_1_2011_to_6_30_2011
        {
            get { return PriceHistoryCsvFiles.DE_1_1_2011_to_6_30_2011.PriceSeries; }
        }

        public static IPriceSeries DE_Apr_June_2011_Weekly_Google
        {
            get { return PriceHistoryCsvFiles.DE_Apr_June_2011_Weekly_Google.PriceSeries; }
        }

        public static IPriceSeries SPX_8_Dec_2010_to_10_Dec_2010
        {
            get { return PriceHistoryCsvFiles.SPX_8_Dec_2010_to_10_Dec_2010.PriceSeries; }
        }
    }
}
