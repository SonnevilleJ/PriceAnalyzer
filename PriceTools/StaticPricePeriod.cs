using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public class StaticPricePeriod : PricePeriod
    {
        #region Private Members

        private readonly decimal? _open;
        private readonly decimal? _high;
        private readonly decimal? _low;
        private readonly decimal _close;
        private readonly long? _volume;
        private readonly DateTime _head;
        private readonly DateTime _tail;

        #endregion

        #region Constructors

        internal StaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
            : this(head, ConstructTail(head, resolution), open, high, low, close, volume)
        {
        }

        internal StaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
        {
            // validate first
            if(head > tail) 
                throw new InvalidOperationException();
            if(high < open) 
                throw new InvalidOperationException();
            if(high < close) 
                throw new InvalidOperationException();
            if(low > open) 
                throw new InvalidOperationException();
            if(low > close) 
                throw new InvalidOperationException();

            _head = head;
            _tail = tail;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;
        }

        #endregion

        #region Private Methods

        private static DateTime ConstructTail(DateTime head, Resolution resolution)
        {
            var result = head;
            switch (resolution)
            {
                case Resolution.Days:
                    result = head.AddDays(1);
                    break;
                case Resolution.Weeks:
                    switch (result.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            result = result.AddDays(5);
                            break;
                        case DayOfWeek.Tuesday:
                            result = result.AddDays(4);
                            break;
                        case DayOfWeek.Wednesday:
                            result = result.AddDays(3);
                            break;
                        case DayOfWeek.Thursday:
                            result = result.AddDays(2);
                            break;
                        case DayOfWeek.Friday:
                            result = result.AddDays(1);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("resolution", resolution,
                                                          String.Format("Unable to identify the period tail for resolution {0}.", resolution));
            }
            return result.Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }

        #endregion

        #region Implementation of IPricePeriod


        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return _close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal High
        {
            get { return _high ?? Close; }
        }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return _low ?? Close; }
        }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return _open ?? Close; }
        }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return _volume; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public override decimal this[DateTime index]
        {
            get
            {
                if (index < Head) throw new InvalidOperationException("Index was before the Head of the PricePeriod.");

                return Close;
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return _tail; }
        }

        /// <summary>
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public override IDictionary<DateTime, decimal> Values
        {
            get
            {
                return new Dictionary<DateTime, decimal> {{Head, Close}};
            }
        }

        #endregion
    }
}