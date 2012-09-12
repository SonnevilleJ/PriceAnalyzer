using System;
using Sonneville.PriceTools;
using Sonneville.PriceTools.Extensions;

namespace TestUtilities.Sonneville.PriceTools
{
    public static class PricePeriodUtilities
    {
        public static IPricePeriod CreatePeriod1()
        {
            var head = new DateTime(2011, 3, 14);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 110.00m;
            const decimal low = 90.00m;
            const decimal close = 100.00m;
            const long volume = 20000;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }

        public static IPricePeriod CreatePeriod2()
        {
            var head = new DateTime(2011, 3, 15);
            var tail = head.GetFollowingClose();
            const decimal open = 100.00m;
            const decimal high = 120.00m;
            const decimal low = 100.00m;
            const decimal close = 110.00m;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close);
        }

        public static IPricePeriod CreatePeriod3()
        {
            var head = new DateTime(2011, 3, 16);
            var tail = head.GetFollowingClose();
            const decimal open = 110.00m;
            const decimal high = 110.00m;
            const decimal low = 80.00m;
            const decimal close = 90.00m;
            const long volume = 10000;

            return PricePeriodFactory.ConstructStaticPricePeriod(head, tail, open, high, low, close, volume);
        }
    }
}