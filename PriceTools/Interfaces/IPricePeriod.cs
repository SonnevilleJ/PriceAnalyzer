using System;

namespace Sonneville.PriceTools
{
    public interface IPricePeriod : ITimePeriod<decimal>, IEquatable<IPricePeriod>
    {
        decimal Close { get; }

        decimal High { get; }

        decimal Low { get; }

        decimal Open { get; }

        long? Volume { get; }
    }
}