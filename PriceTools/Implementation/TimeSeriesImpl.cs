using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.Implementation
{
    internal class TimeSeriesImpl : ITimeSeries
    {
        private readonly IEnumerable<ITimePeriod> _periods = new List<ITimePeriod>();
 
        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Head { get; private set; }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Tail { get; private set; }

        /// <summary>
        /// Gets the <see cref="ITimePeriod.Resolution"/> of price data stored within the ITimePeriod.
        /// </summary>
        public Resolution Resolution { get; private set; }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod> TimePeriods
        {
            get { return _periods; }
        }

        /// <summary>
        /// Determines if the ITimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            throw new NotImplementedException();
        }
    }
}