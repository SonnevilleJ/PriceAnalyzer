using System.Collections.Generic;
using Sonneville.PriceTools.Data.Csv;

namespace Sonneville.PriceTools.SampleData
{
    public interface ISamplePriceData
    {
        string Ticker { get; }
        IPriceSeries PriceSeries { get; }
        IEnumerable<IPricePeriod> PricePeriods { get; }
        PriceHistoryCsvFile PriceHistory { get; }
    }
}