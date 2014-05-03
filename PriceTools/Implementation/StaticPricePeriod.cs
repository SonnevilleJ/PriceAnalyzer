using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal class StaticPricePeriod : PricePeriod
    {
        private readonly decimal _open;
        private readonly decimal _high;
        private readonly decimal _low;
        private readonly decimal _close;
        private readonly long? _volume;
        private readonly DateTime _head;
        private readonly DateTime _tail;

        internal StaticPricePeriod(DateTime head, DateTime tail, decimal open, decimal high, decimal low, decimal close, long? volume)
        {
            _head = head;
            _tail = tail;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;
        }

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
            get { return _high; }
        }

        /// <summary>
        /// Gets the lowest price that occurred during  the PricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return _low; }
        }

        /// <summary>
        /// Gets the opening price for the PricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return _open; }
        }

        /// <summary>
        /// Gets the total volume of trades during the PricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return _volume; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public override decimal this[DateTime dateTime]
        {
            get
            {
                if (dateTime < Head) throw new InvalidOperationException(Strings.StaticPricePeriodImpl_this_Index_was_before_the_head_of_the_price_period_);

                return Close;
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
        /// </summary>
        public override DateTime Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public override DateTime Tail
        {
            get { return _tail; }
        }
    }
}