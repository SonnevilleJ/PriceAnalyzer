using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a single period in a <see cref="ITimeSeries"/>.
    /// </summary>
    public struct SimplePeriodImpl : ITimePeriod
    {
        private readonly DateTime _head;
        private readonly DateTime _tail;
        private readonly decimal _value;

        internal SimplePeriodImpl(DateTime head, DateTime tail, decimal value)
        {
            _head = head;
            if (tail < head) throw new ArgumentOutOfRangeException("tail", Strings.SimplePeriod_SimplePeriod_Period_s_head_must_come_before_tail_);
            _tail = tail;
            _value = value;
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get
            {
                if(HasValueInRange(dateTime)) return _value;
                throw new IndexOutOfRangeException(String.Format("DateTime: {0} is out of range for this Period.", dateTime));
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimePeriod.
        /// </summary>
        public DateTime Tail
        {
            get { return _tail; }
        }

        /// <summary>
        /// Gets the <see cref="ITimePeriod.Resolution"/> of price data stored within the ITimePeriod.
        /// </summary>
        public Resolution Resolution
        {
            get { return (Resolution) ((Tail - Head).Ticks); }
        }

        /// <summary>
        /// Determines if the ITimePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimePeriod has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            return Head <= settlementDate && Tail >= settlementDate;
        }
    }
}
