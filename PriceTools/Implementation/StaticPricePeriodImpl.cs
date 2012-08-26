using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal class StaticPricePeriodImpl : PricePeriod
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

        internal StaticPricePeriodImpl(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
            : this(head, ConstructTail(head, resolution), open, high, low, close, volume)
        {
        }

        internal StaticPricePeriodImpl(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
        {
            // validate first
            if (open.HasValue && open.Value < 0)
                throw new ArgumentOutOfRangeException("open", open, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_must_be_greater_than_or_equal_to_zero_);
            if (high.HasValue && high.Value < 0)
                throw new ArgumentOutOfRangeException("high", high, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_High_price_must_be_greater_than_or_equal_to_zero_);
            if (low.HasValue && low.Value < 0)
                throw new ArgumentOutOfRangeException("low", low, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Low_price_must_be_greater_than_or_equal_to_zero_);
            if (close < 0)
                throw new ArgumentOutOfRangeException("close", close, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_must_be_greater_than_or_equal_to_zero_);
            if (volume.HasValue && volume.Value <0)
                throw new ArgumentOutOfRangeException("volume", volume, Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Volume_must_be_greater_than_or_equal_to_zero_);
            if(head > tail) 
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Head_must_come_before_Tail_);
            if(high < open) 
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_cannot_be_higher_than_High_price_);
            if(high < close) 
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_cannot_be_higher_than_High_price_);
            if(low > open) 
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Opening_price_cannot_be_lower_than_Low_price_);
            if(low > close) 
                throw new InvalidOperationException(Strings.StaticPricePeriodImpl_StaticPricePeriodImpl_Closing_price_cannot_be_lower_than_Low_price_);

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

        #region Implementation of PricePeriod


        /// <summary>
        /// Gets the closing price for the PricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return _close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the PricePeriod.
        /// </summary>
        public override decimal High
        {
            get { return _high ?? Close; }
        }

        /// <summary>
        /// Gets the lowest price that occurred during  the PricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return _low ?? Close; }
        }

        /// <summary>
        /// Gets the opening price for the PricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return _open ?? Close; }
        }

        /// <summary>
        /// Gets the total volume of trades during the PricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return _volume; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the TimeSeries.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the TimeSeries as of the given DateTime.</returns>
        public override decimal this[DateTime dateTime]
        {
            get
            {
                if (dateTime < Head) throw new InvalidOperationException("Index was before the Head of the PricePeriod.");

                return Close;
            }
        }

        /// <summary>
        /// Gets the first DateTime in the TimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Gets the last DateTime in the TimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return _tail; }
        }

        /// <summary>
        /// Determines if the PricePeriod has any data at all. PricePeriods with no data are not equal.
        /// </summary>
        protected override bool HasData
        {
            get { return true; } // constructor assigns data
        }

        #endregion
    }
}