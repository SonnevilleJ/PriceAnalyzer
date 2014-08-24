using System;

namespace Sonneville.PriceTools
{
    [Flags]
    public enum PricingType
    {
        Market      = 1,

        Limit       = 2,

        Stop        = 4,

        StopMarket  = Stop | Market,

        StopLimit   = Stop | Limit
    }
}
