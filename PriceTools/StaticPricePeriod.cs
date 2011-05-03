﻿using System;

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

        internal StaticPricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume)
        {
            // validate first
            if(head > tail) throw new InvalidOperationException();
            if(high < open) throw new InvalidOperationException();
            if(high < close) throw new InvalidOperationException();
            if(low > open) throw new InvalidOperationException();
            if(low > close) throw new InvalidOperationException();

            SetDefaultMarketTimes(ref head, ref tail);

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

        private static void SetDefaultMarketTimes(ref DateTime head, ref DateTime tail)
        {
            if (head.Hour == 0 && head.Minute == 0 && head.Second == 0)
            {
                head = head.Add(Settings.MarketOpen);
            }
            if (tail.Hour == 0 && tail.Minute == 0 && tail.Second == 0)
            {
                tail = tail.Add(Settings.MarketClose);
            }
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
        public override decimal? High
        {
            get { return EFHigh; }
        }

        /// <summary>
        /// Gets the lowest price that occurred during  the IPricePeriod.
        /// </summary>
        public override decimal? Low
        {
            get { return EFLow; }
        }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal? Open
        {
            get { return EFOpen; }
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
        public override decimal? this[DateTime index]
        {
            get
            {
                if (HasValue(index))
                {
                    return Close;
                }
                return null;
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
            return base.GetHashCode();
        }

        #endregion
    }
}