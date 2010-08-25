using System;
using System.Runtime.Serialization;

namespace Sonneville.PriceTools
{
    public interface IPricePeriod : ISerializable
    {
        decimal Close { get; }
        DateTime Head { get; }
        decimal? High { get; }
        decimal? Low { get; }
        decimal? Open { get; }
        DateTime Tail { get; }
        TimeSpan TimeSpan { get; }
        ulong? Volume { get; }
    }
}
