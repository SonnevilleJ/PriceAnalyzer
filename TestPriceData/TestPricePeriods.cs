using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.TestPriceData
{
    public static class TestPricePeriods
    {
        public static IEnumerable<IPricePeriod> IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS
        {
            get { return TestPriceSeries.IBM_1_1_2011_to_3_15_2011_Daily_Yahoo_PS.PricePeriods; }
        }

        public static IEnumerable<IPricePeriod> DE_1_1_2011_to_6_30_2011
        {
            get { return TestPriceSeries.DE_1_1_2011_to_6_30_2011.PricePeriods; }
        }

        public static IEnumerable<IPricePeriod> MSFT_Apr_June_2011_Weekly_Google
        {
            get { return TestPriceSeries.MSFT_Apr_June_2011_Weekly_Google.PricePeriods; }
        }

        public static IEnumerable<IPricePeriod> Build_DE_1_1_2011_to_6_30_2011(DateTime head, DateTime tail)
        {
            return TestPriceHistoryCsvFiles.Build_DE_1_1_2011_to_6_30_2011(head, tail).PricePeriods;
        }
    }
}
