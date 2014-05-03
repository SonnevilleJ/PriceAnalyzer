using System;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal class PricePeriod : IPricePeriod
    {
        public PricePeriod(DateTime head, DateTime tail, decimal open, decimal high, decimal low, decimal close, long? volume)
        {
            Close = close;
            High = high;
            Low = low;
            Open = open;
            Volume = volume;
            Head = head;
            Tail = tail;
        }

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public decimal Close { get; private set; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public decimal High { get; private set; }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public decimal Low { get; private set; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public decimal Open { get; private set; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public long? Volume { get; private set; }

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

        /// <summary>
        /// Gets the first DateTime in the IPricePeriod.
        /// </summary>
        public DateTime Head { get; private set; }

        /// <summary>
        /// Gets the last DateTime in the IPricePeriod.
        /// </summary>
        public DateTime Tail { get; private set; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the IPricePeriod.
        /// </summary>
        public Resolution Resolution
        {
            get
            {
                var resolutions = Enum.GetValues(typeof (Resolution)).Cast<long>().OrderBy(ticks => ticks);
                return (Resolution) Enum.ToObject(typeof (Resolution), resolutions.First(ticks => this.TimeSpan().Ticks <= ticks));
            }
        }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPricePeriod  other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Resolution == other.Resolution &&
                   Head == other.Head &&
                   Tail == other.Tail &&
                   Open == other.Open &&
                   High == other.High &&
                   Low == other.Low &&
                   Close == other.Close &&
                   Volume == other.Volume;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return Equals(obj as IPricePeriod);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = Resolution.GetHashCode();
                result = (result * 397) ^ Head.GetHashCode();
                result = (result * 397) ^ Tail.GetHashCode();
                result = (result * 397) ^ Open.GetHashCode();
                result = (result * 397) ^ High.GetHashCode();
                result = (result * 397) ^ Low.GetHashCode();
                result = (result * 397) ^ Close.GetHashCode();
                result = (result * 397) ^ Volume.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PricePeriod left, PricePeriod right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PricePeriod left, PricePeriod right)
        {
            return !Equals(left, right);
        }

        #endregion

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