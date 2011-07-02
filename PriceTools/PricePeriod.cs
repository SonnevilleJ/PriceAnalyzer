using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public abstract partial class PricePeriod : IPricePeriod
    {
        #region Implementation of IPricePeriod

        /// <summary>
        /// Gets the last price for the IPricePeriod.
        /// </summary>
        public decimal Last { get { return Close; } }

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public abstract decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public abstract decimal? High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public abstract decimal? Low { get; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public abstract decimal? Open { get; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public abstract long? Volume { get; }

        /// <summary>
        ///   Gets a <see cref = "IPricePeriod.TimeSpan" /> value indicating the length of time covered by this IPricePeriod.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return Tail - Head; }
        }

        /// <summary>
        ///   Event which is invoked when new price data is available for the IPricePeriod.
        /// </summary>
        public event EventHandler<NewPriceDataAvailableEventArgs> NewPriceDataAvailable;

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public abstract decimal this[DateTime index] { get; }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public abstract DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public abstract DateTime Tail { get; }

        /// <summary>
        /// Determines if the PricePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PricePeriod has a valid value for the given date.</returns>
        public virtual bool HasValueInRange(DateTime settlementDate)
        {
            return Head <= settlementDate && Tail >= settlementDate;
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(PricePeriod left, PricePeriod right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return left.Head == right.Head &&
                   left.Tail == right.Tail &&
                   left.Open == right.Open &&
                   left.High == right.High &&
                   left.Low == right.Low &&
                   left.Close == right.Close;
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(PricePeriod left, PricePeriod right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPricePeriod>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPricePeriod other)
        {
            return Equals((object)other);
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
            return this == obj as PricePeriod;
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
                int result = 0;
                result = (result * 397) ^ (Open.HasValue ? Open.Value.GetHashCode() : 0);
                result = (result * 397) ^ (High.HasValue ? High.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Low.HasValue ? Low.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Volume.HasValue ? Volume.Value.GetHashCode() : 0);
                result = (result * 397) ^ Close.GetHashCode();
                result = (result * 397) ^ Head.GetHashCode();
                result = (result * 397) ^ Tail.GetHashCode();
                return result;
            }
        }

        #endregion


        /// <summary>
        /// Invokes the NewPriceDataAvailable event.
        /// </summary>
        /// <param name="e">The NewPriceDataEventArgs to pass.</param>
        protected void InvokeNewPriceDataAvailable(NewPriceDataAvailableEventArgs e)
        {
            if (NewPriceDataAvailable != null)
            {
                NewPriceDataAvailable(this, e);
            }
        }
    }
}