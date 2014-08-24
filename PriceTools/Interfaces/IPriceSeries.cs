using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    public interface IPriceSeries : IPricePeriod, ITimeSeries<ITimePeriod<decimal>, decimal>, IEquatable<IPriceSeries>
    {
        string Ticker { get; }

        IEnumerable<IPricePeriod> PricePeriods { get; }
        
        void AddPriceData(IPricePeriod pricePeriod);

        void AddPriceData(IEnumerable<IPricePeriod> pricePeriods);
    }
}