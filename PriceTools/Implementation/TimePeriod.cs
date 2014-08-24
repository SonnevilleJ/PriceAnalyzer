using System;
using System.Globalization;

namespace Sonneville.PriceTools.Implementation
{
    public struct TimePeriod<T> : ITimePeriod<T>
    {
        internal TimePeriod(DateTime head, DateTime tail, T value) : this()
        {
            if (tail < head) throw new ArgumentOutOfRangeException("tail", Strings.SimplePeriod_SimplePeriod_Period_s_head_must_come_before_tail_);

            Head = head;
            Tail = tail;
            Value = value;
        }

        public T this[DateTime dateTime]
        {
            get
            {
                if(this.HasValueInRange(dateTime)) return Value;
                throw new IndexOutOfRangeException(String.Format(CultureInfo.InvariantCulture, Strings.TimePeriodImpl_this_Date_time___0__is_out_of_range_for_this_price_period_, dateTime));
            }
        }

        public DateTime Head { get; private set; }

        public DateTime Tail { get; private set; }

        public Resolution Resolution
        {
            get { return (Resolution) ((Tail - Head).Ticks); }
        }

        private T Value { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Head: {0}; Tail: {1}; Value: {2}", Head.ToShortDateString(), Tail.ToShortDateString(), this.Value());
        }
    }
}
