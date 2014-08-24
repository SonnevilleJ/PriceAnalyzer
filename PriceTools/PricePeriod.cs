using System;

namespace Sonneville.PriceTools
{
    public struct PricePeriod : IPricePeriod
    {
        private static readonly ResolutionUtility ResolutionUtility = new ResolutionUtility();

        public PricePeriod(DateTime head, DateTime tail, decimal value)
            : this(head, tail, value, null)
        {
        }

        public PricePeriod(DateTime head, DateTime tail, decimal value, long? volume)
            : this(head, tail, value, value, value, value, volume)
        {
        }

        public PricePeriod(DateTime head, Resolution resolution, decimal value)
            : this(head, resolution, value, null)
        {
        }

        public PricePeriod(DateTime head, Resolution resolution, decimal value, long? volume)
            : this(head, resolution, value, value, value, value, volume)
        {
        }

        public PricePeriod(DateTime head, Resolution resolution, decimal open, decimal high, decimal low, decimal close)
            : this(head, resolution, open, high, low, close, null)
        {
        }

        public PricePeriod(DateTime head, Resolution resolution, decimal open, decimal high, decimal low, decimal close, long? volume)
            : this(head, ResolutionUtility.ConstructTail(head, resolution), open, high, low, close, volume)
        {
        }

        public PricePeriod(DateTime head, DateTime tail, decimal open, decimal high, decimal low, decimal close, long? volume)
            : this()
        {
            Close = close;
            High = high;
            Low = low;
            Open = open;
            Volume = volume;
            Head = head;
            Tail = tail;
            Resolution = ResolutionUtility.CalculateResolution(this.TimeSpan());
        }

        public DateTime Head { get; private set; }

        public DateTime Tail { get; private set; }

        public decimal Open { get; private set; }

        public decimal High { get; private set; }

        public decimal Low { get; private set; }

        public decimal Close { get; private set; }

        public long? Volume { get; private set; }

        public Resolution Resolution { get; private set; }

        public decimal this[DateTime dateTime]
        {
            get
            {
                if (dateTime < Head)
                    throw new InvalidOperationException(
                        Strings.StaticPricePeriodImpl_this_Index_was_before_the_head_of_the_price_period_);

                return Close;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IPricePeriod && Equals((IPricePeriod)obj);
        }

        public bool Equals(IPricePeriod other)
        {
            return Close == other.Close
                   && High == other.High
                   && Low == other.Low
                   && Open == other.Open
                   && Volume == other.Volume
                   && Head.Equals(other.Head)
                   && Tail.Equals(other.Tail);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Close.GetHashCode();
                hashCode = (hashCode * 397) ^ High.GetHashCode();
                hashCode = (hashCode * 397) ^ Low.GetHashCode();
                hashCode = (hashCode * 397) ^ Open.GetHashCode();
                hashCode = (hashCode * 397) ^ Volume.GetHashCode();
                hashCode = (hashCode * 397) ^ Head.GetHashCode();
                hashCode = (hashCode * 397) ^ Tail.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("Head: {0}; Tail: {1}; Open: {2}; High: {3}; Low: {4}; Close: {5}; Volume: {6}",
                                 Head.ToShortDateString(), Tail.ToShortDateString(), Open, High, Low, Close, Volume);
        }
    }
}