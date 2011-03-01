using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public abstract partial class PricePeriod : IPricePeriod
    {
        #region Implementation of IPricePeriod

        /// <summary>
        /// Gets the last price for the IPricePeriod.
        /// </summary>
        public abstract decimal Last { get; }

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public abstract decimal Close { get; protected set; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public abstract decimal? High { get; protected set; }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        public abstract decimal? Low { get; protected set; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public abstract decimal? Open { get; protected set; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public abstract long? Volume { get; protected set; }

        #endregion

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets a value stored at a given DateTime index of the PricePeriod.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the PricePeriod as of the given DateTime.</returns>
        public abstract decimal this[DateTime index] { get; }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public abstract DateTime Head { get; protected set; }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public abstract DateTime Tail { get; protected set; }

        /// <summary>
        /// Determines if the PricePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PricePeriod has a valid value for the given date.</returns>
        public bool HasValue(DateTime settlementDate)
        {
            return Head <= settlementDate && Tail >= settlementDate;
        }

        #endregion
    }
}