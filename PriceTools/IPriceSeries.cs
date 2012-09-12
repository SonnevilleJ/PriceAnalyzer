using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time-series of price data.
    /// </summary>
    public interface IPriceSeries : IPricePeriod, ITimeSeries
    {
        /// <summary>
        /// Gets the ticker symbol priced by this PriceSeries.
        /// </summary>
        string Ticker { get; set; }
    }
}