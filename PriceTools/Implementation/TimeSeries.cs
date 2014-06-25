using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    internal class TimeSeries<TPeriodValue> : ITimeSeries<ITimePeriod<TPeriodValue>, TPeriodValue>
    {
        private readonly IEnumerable<ITimePeriod<TPeriodValue>> _periods = new List<ITimePeriod<TPeriodValue>>();

        internal TimeSeries(IEnumerable<ITimePeriod<TPeriodValue>> list)
        {
            _periods = list;
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public TPeriodValue this[DateTime dateTime]
        {
            get { return TimePeriods.First(p => dateTime >= p.Head && dateTime <= p.Tail)[dateTime]; }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Head { get { return TimePeriods.First().Head; } }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Tail { get { return TimePeriods.Last().Tail; } }

        /// <summary>
        /// Gets the <see cref="ITimePeriod.Resolution"/> of price data stored within the ITimeSeries.
        /// </summary>
        public Resolution Resolution { get { return TimePeriods.Min(p => p.Resolution); } }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod<TPeriodValue>> TimePeriods
        {
            get { return _periods; }
        }
    }
}