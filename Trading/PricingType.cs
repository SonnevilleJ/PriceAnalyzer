using System;

namespace Sonneville.PriceTools.Trading
{
    [Flags]
    public enum PricingType
    {
        Market      = 1,
        Limit       = 2,
        Stop        = 4
    }
}
