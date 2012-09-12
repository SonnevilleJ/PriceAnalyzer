using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A statistical indicator used to transform <see cref="PriceTools.ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying data series.
    /// </summary>
    public interface IIndicator : ITimeSeries
    {
        /// <summary>
        /// Gets the lookback of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        int Lookback { get; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        IPriceSeries PriceSeries { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        IList<IPricePeriod> PricePeriods { get; }

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        void CalculateAll();

        /// <summary>
        /// Gets the <see cref="IPricePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="IPricePeriod"/> to get.</param>
        /// <returns>The <see cref="IPricePeriod"/> stored at the given index.</returns>
        IPricePeriod this[int index] { get; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods();

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution);

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="IPriceSeries.Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        void AddPriceData(IPricePeriod pricePeriod);

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        void AddPriceData(IEnumerable<IPricePeriod> pricePeriods);
    }
}