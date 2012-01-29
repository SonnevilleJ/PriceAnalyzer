using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal abstract class PricePeriodImpl : PricePeriod
    {
        #region Implementation of PricePeriod

        /// <summary>
        /// Gets the closing price for the PricePeriod.
        /// </summary>
        public abstract decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the PricePeriod.
        /// </summary>
        public abstract decimal High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during the PricePeriod.
        /// </summary>
        public abstract decimal Low { get; }

        /// <summary>
        /// Gets the opening price for the PricePeriod.
        /// </summary>
        public abstract decimal Open { get; }

        /// <summary>
        /// Gets the total volume of trades during the PricePeriod.
        /// </summary>
        public abstract long? Volume { get; }
        
        /// <summary>
        ///   Event which is invoked when new price data is available for the PricePeriod.
        /// </summary>
        public event EventHandler<NewPriceDataAvailableEventArgs> NewPriceDataAvailable;

        /// <summary>
        /// Gets a value stored at a given DateTime index of the TimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the TimeSeries as of the given DateTime.</returns>
        public abstract decimal this[DateTime index] { get; }

        /// <summary>
        /// Gets the first DateTime in the TimeSeries.
        /// </summary>
        public abstract DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the TimeSeries.
        /// </summary>
        public abstract DateTime Tail { get; }

        /// <summary>
        /// Gets the <see cref="TimeSeries.Resolution"/> of price data stored within the TimeSeries.
        /// </summary>
        public virtual Resolution Resolution
        {
            get
            {
                foreach (var resolution in
                    Enum.GetValues(typeof (Resolution)).Cast<long>().OrderBy(ticks => ticks).Where(ticks => this.TimeSpan() <= new TimeSpan(ticks)))
                {
                    return (Resolution)Enum.ToObject(typeof(Resolution), new TimeSpan(resolution).Ticks);
                }

                throw new OverflowException();
            }
        }

        /// <summary>
        /// Gets the values stored within the TimeSeries.
        /// </summary>
        public abstract IDictionary<DateTime, decimal> Values { get; }

        /// <summary>
        /// Determines if the PricePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PricePeriod has a valid value for the given date.</returns>
        public virtual bool HasValueInRange(DateTime settlementDate)
        {
            return Head <= settlementDate && Tail >= settlementDate;
        }

        #endregion

        /// <summary>
        /// Invokes the NewPriceDataAvailable event.
        /// </summary>
        /// <param name="e">The NewPriceDataEventArgs to pass.</param>
        protected void InvokeNewPriceDataAvailable(NewPriceDataAvailableEventArgs e)
        {
            if (NewPriceDataAvailable != null)
            {
                NewPriceDataAvailable(this, e);
            }
        }
    }
}