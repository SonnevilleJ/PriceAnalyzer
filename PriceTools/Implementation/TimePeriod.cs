using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a single period in a <see cref="ITimeSeries"/>.
    /// </summary>
    public struct TimePeriod : ITimePeriod
    {
        internal TimePeriod(DateTime head, DateTime tail, decimal value) : this()
        {
            if (tail < head) throw new ArgumentOutOfRangeException("tail", Strings.SimplePeriod_SimplePeriod_Period_s_head_must_come_before_tail_);

            Head = head;
            Tail = tail;
            Value = value;
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
                if(this.HasValueInRange(dateTime)) return Value;
                throw new IndexOutOfRangeException(String.Format(CultureInfo.InvariantCulture, Strings.TimePeriodImpl_this_Date_time___0__is_out_of_range_for_this_price_period_, dateTime));
            }
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
        public Resolution Resolution
        {
            get { return (Resolution) ((Tail - Head).Ticks); }
        }

        private decimal Value { get; set; }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Head: {0}; Tail: {1}; Value: {2}", Head.ToShortDateString(), Tail.ToShortDateString(), this.Value<decimal>());
        }
    }
}
