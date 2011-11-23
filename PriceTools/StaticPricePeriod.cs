using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a defined period of price data.
    /// </summary>
    public partial class StaticPricePeriod
    {
        #region Constructors

        private StaticPricePeriod()
        {
        }

        internal StaticPricePeriod(DateTime head, Resolution resolution, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
            : this(head, ConstructTail(head, resolution), open, high, low, close, volume)
        {
        }

        internal StaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
        {
            // validate first
            if(head > tail) throw new InvalidOperationException();
            if(high < open) throw new InvalidOperationException();
            if(high < close) throw new InvalidOperationException();
            if(low > open) throw new InvalidOperationException();
            if(low > close) throw new InvalidOperationException();

            EFHead = head;
            EFTail = tail;
            EFOpen = open;
            EFHigh = high;
            EFLow = low;
            EFClose = close;
            EFVolume = volume;
        }

        #endregion

        #region Private Methods

        private static DateTime ConstructTail(DateTime head, Resolution resolution)
        {
            var result = head;
            switch (resolution)
            {
                case Resolution.Days:
                    result = head.AddDays(1);
                    break;
                case Resolution.Weeks:
                    switch (result.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            result = result.AddDays(5);
                            break;
                        case DayOfWeek.Tuesday:
                            result = result.AddDays(4);
                            break;
                        case DayOfWeek.Wednesday:
                            result = result.AddDays(3);
                            break;
                        case DayOfWeek.Thursday:
                            result = result.AddDays(2);
                            break;
                        case DayOfWeek.Friday:
                            result = result.AddDays(1);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("resolution", resolution,
                                                          String.Format("Unable to identify the period tail for resolution {0}.", resolution));
            }
            return result.Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }

        #endregion

        #region Implementation of IPricePeriod

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return EFClose; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal High
        {
            get { return EFHigh ?? Close; }
        }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        public override decimal Low
        {
            get { return EFLow ?? Close; }
        }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal Open
        {
            get { return EFOpen ?? Close; }
        }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return EFVolume; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public override decimal this[DateTime index]
        {
            get
            {
                if (index < Head) throw new InvalidOperationException("Index was before the Head of the PricePeriod.");

                return Close;
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return EFHead; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return EFTail; }
        }

        /// <summary>
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public override IDictionary<DateTime, decimal> Values
        {
            get
            {
                return new Dictionary<DateTime, decimal> {{Head, Close}};
            }
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(StaticPricePeriod left, StaticPricePeriod right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return left.Close == right.Close &&
                left.Head == right.Head &&
                left.High == right.High &&
                left.Low == right.Low &&
                left.Open == right.Open &&
                left.Tail == right.Tail &&
                left.Volume == right.Volume;
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(StaticPricePeriod left, StaticPricePeriod right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPricePeriod>

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this == obj as StaticPricePeriod;
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
                var result = base.GetHashCode();
                result = (result*397) ^ (Open.GetHashCode());
                result = (result*397) ^ (High.GetHashCode());
                result = (result*397) ^ (Low.GetHashCode());
                result = (result*397) ^ (Volume.HasValue ? Volume.Value.GetHashCode() : 0);
                result = (result*397) ^ Close.GetHashCode();
                result = (result*397) ^ Head.GetHashCode();
                result = (result*397) ^ Tail.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}