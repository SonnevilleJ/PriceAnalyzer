using System;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.TestUtilities
{
    public static class PriceQuoteUtilities
    {
        private static readonly IPriceTickFactory PriceTickFactory;

        static PriceQuoteUtilities()
        {
            PriceTickFactory = new PriceTickFactory();
        }

        public static PriceTick CreateTick1()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("2/28/2011 9:30 AM"), 10, 50);
        }

        public static PriceTick CreateTick2()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/1/2011 10:00 AM"), 9, 60);
        }

        public static PriceTick CreateTick3()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/2/2011 2:00 PM"), 14, 50);
        }

        public static PriceTick CreateTick4()
        {
            return PriceTickFactory.ConstructPriceTick(DateTime.Parse("3/2/2011 4:00 PM"), 11, 30);
        }
    }
}