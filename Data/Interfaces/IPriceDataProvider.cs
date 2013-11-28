using System;

namespace Sonneville.PriceTools.Data
{
    /// <summary>
    ///   Provides price data.
    /// </summary>
    public interface IPriceDataProvider
    {
        /// <summary>
        /// Gets a <see cref="IPriceSeries"/> containing price history.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> containing price history to be updated.</param>
        /// <param name="head">The first date to price.</param>
        /// <param name="tail">The last date to price.</param>
        /// <param name="resolution">The <see cref="Resolution"/> of <see cref="IPricePeriod"/>s to retrieve.</param>
        /// <returns></returns>
        void UpdatePriceSeries(IPriceSeries priceSeries, DateTime head, DateTime tail, Resolution resolution);

        /// <summary>
        /// Gets the smallest <see cref="Resolution"/> available from this PriceDataProvider.
        /// </summary>
        Resolution BestResolution { get; }
    }
}