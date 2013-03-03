using System;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal abstract class PricePeriodImpl : IPricePeriod, IEquatable<PricePeriodImpl>
    {
        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public abstract decimal Close { get; }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public abstract decimal High { get; }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public abstract decimal Low { get; }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public abstract decimal Open { get; }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public abstract long? Volume { get; }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the IPricePeriod.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the IPricePeriod as of the given DateTime.</returns>
        public abstract decimal this[DateTime dateTime] { get; }

        /// <summary>
        /// Gets the first DateTime in the IPricePeriod.
        /// </summary>
        public abstract DateTime Head { get; }

        /// <summary>
        /// Gets the last DateTime in the IPricePeriod.
        /// </summary>
        public abstract DateTime Tail { get; }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of price data stored within the IPricePeriod.
        /// </summary>
        public virtual Resolution Resolution
        {
            get
            {
                foreach (var resolution in
                    Enum.GetValues(typeof(Resolution)).Cast<long>().Where(ticks => this.TimeSpan().Ticks <= ticks).OrderBy(ticks => ticks))
                {
                    return (Resolution)Enum.ToObject(typeof(Resolution), new TimeSpan(resolution).Ticks);
                }

                throw new OverflowException();
            }
        }

        /// <summary>
        /// Determines if the IPricePeriod has any data at all. IPricePeriods with no data are not equal.
        /// </summary>
        protected abstract bool HasData { get; }

        /// <summary>
        /// Determines if the IPricePeriod has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the IPricePeriod has a valid value for the given date.</returns>
        public virtual bool HasValueInRange(DateTime settlementDate)
        {
            return Head <= settlementDate && Tail >= settlementDate;
        }

        #region Events and Invokers

        /// <summary>
        /// Invokes the NewDataAvailable event.
        /// </summary>
        /// <param name="e">The NewPriceDataEventArgs to pass.</param>
        protected void InvokeNewDataAvailable(NewDataAvailableEventArgs e)
        {
            var eventHandler = NewDataAvailable;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        /// <summary>
        ///   Event which is invoked when new data is available for the IPricePeriod.
        /// </summary>
        public event EventHandler<NewDataAvailableEventArgs> NewDataAvailable;

        #endregion
        
        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PricePeriodImpl other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return HasData && HasData == other.HasData &&
                   Resolution == other.Resolution &&
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
            return Equals(obj as PricePeriodImpl);
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
        public static bool operator ==(PricePeriodImpl left, PricePeriodImpl right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PricePeriodImpl left, PricePeriodImpl right)
        {
            return !Equals(left, right);
        }

        #endregion

#if DEBUG

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

#endif
    }
}