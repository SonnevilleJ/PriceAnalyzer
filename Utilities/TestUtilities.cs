using System;
using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public static class TestUtilities
    {
        #region Price Period tools

        public static PricePeriod CreatePeriod1()
        {
            DateTime head = new DateTime(2011, 3, 11);
            DateTime tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;
            const long volume = 20000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        public static PricePeriod CreatePeriod2()
        {
            DateTime head = new DateTime(2011, 3, 12);
            DateTime tail = head.AddDays(1);
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 100.00m;
            const decimal close = 110.00m;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close);
        }

        public static PricePeriod CreatePeriod3()
        {
            DateTime head = new DateTime(2011, 3, 13);
            DateTime tail = head.AddDays(1);
            const decimal open = 110.00m;
            const decimal high = 110.00m;
            const decimal low = 80.00m;
            const decimal close = 90.00m;
            const long volume = 10000;

            return PricePeriodFactory.CreateStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        #endregion

        #region Price Quote tools

        public static PriceQuote CreateQuote1()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("2/28/2011 9:30 AM"),
                Price = 10,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote2()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/1/2011 10:00 AM"),
                Price = 9,
                Volume = 60
            };
        }

        public static PriceQuote CreateQuote3()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 2:00 PM"),
                Price = 14,
                Volume = 50
            };
        }

        public static PriceQuote CreateQuote4()
        {
            return new PriceQuote
            {
                SettlementDate = DateTime.Parse("3/2/2011 4:00 PM"),
                Price = 11,
                Volume = 30
            };
        }

        #endregion
    }
}
