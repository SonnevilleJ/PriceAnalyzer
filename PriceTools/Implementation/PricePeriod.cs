﻿using System;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    internal abstract class PricePeriod : IPricePeriod
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
        public virtual bool Equals(IPricePeriod  other)
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