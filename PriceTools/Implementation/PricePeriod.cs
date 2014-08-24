using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
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

        /// <summary>
        /// Gets the first DateTime in the IPricePeriod.
        /// </summary>
        public DateTime Head { get; private set; }

        /// <summary>
        /// Gets the last DateTime in the IPricePeriod.
        /// </summary>
        public DateTime Tail { get; private set; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public decimal Open { get; private set; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public decimal High { get; private set; }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public decimal Low { get; private set; }

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public decimal Close { get; private set; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public long? Volume { get; private set; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the IPricePeriod.
        /// </summary>
        public Resolution Resolution { get; private set; }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the IPricePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the IPricePeriod as of the given DateTime.</returns>
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("Head: {0}; Tail: {1}; Open: {2}; High: {3}; Low: {4}; Close: {5}; Volume: {6}",
                                 Head.ToShortDateString(), Tail.ToShortDateString(), Open, High, Low, Close, Volume);
        }
    }
}