using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a single price period. A price period could be any period of time (minute, hour, day, week, month, year, etc).
    /// </summary>
    public partial class PricePeriod : IPricePeriod
    {
        #region Private Members

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs an empty PricePeriod.
        /// </summary>
        protected PricePeriod()
        {
        }

        /// <summary>
        ///   Constructs a PricePeriod object.
        /// </summary>
        /// <param name = "head">The beginning DateTime of this period.</param>
        /// <param name = "tail">The ending DateTime of this period.</param>
        /// <param name = "open">The price at the open of this period.</param>
        /// <param name = "high">The highest price during this period.</param>
        /// <param name = "low">The lowest price during this period.</param>
        /// <param name = "close">The price at the close of this period.</param>
        /// <param name = "volume">The total volume during this period.</param>
        public PricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, long? volume = null)
        {
            Head = head;
            Tail = tail;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;

            Validate();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public virtual decimal this[DateTime index]
        {
            get { return HasValue(index) ? Close : 0.00m; }
        }

        /// <summary>
        /// Determines if the ITimeSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the ITimeSeries has a valid value for the given date.</returns>
        public bool HasValue(DateTime settlementDate)
        {
            return settlementDate >= Head && settlementDate <= Tail;
        }

        /// <summary>
        ///   Gets a TimeSpan representing the duration of this period.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return Tail - Head; }
        }

        #endregion

        /// <summary>
        ///   Validates a PricePeriod.
        /// </summary>
        /// <returns>A value indicating if the PricePeriod is valid.</returns>
        protected void Validate()
        {
            List<string> errors = new List<string>();

            if (Head > Tail)
                errors.Add("Head must be earlier than Tail.");
            if (High < Open || High < Low ||
                High < Close)
                errors.Add("High must be greater than or equal to the period's open, low, and close.");
            if (Low > Open || Low > High ||
                Low > Close)
                errors.Add("Low must be less than or equal to the period's open, high, and close.");

            if (errors.Count != 0)
                throw new InvalidPricePeriodException(errors.ToString());
        }

        #region Overridden Equality Members

        /// <summary>
        ///   Determines whether the specified <see cref = "PricePeriod" /> is equal to the current <see cref = "PricePeriod" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "PricePeriod" /> is equal to the current <see cref = "PricePeriod" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "PricePeriod" /> to compare with the current <see cref = "PricePeriod" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return (this == obj as PricePeriod);
        }

        /// <summary>
        ///   Serves as a hash function for the PricePeriod type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "PricePeriod" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        ///<summary>
        ///</summary>
        ///<param name = "left"></param>
        ///<param name = "right"></param>
        ///<returns></returns>
        public static bool operator >(PricePeriod left, PricePeriod right)
        {
            return (left.Head.Ticks + 1 > right.Tail.Ticks);
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator <(PricePeriod left, PricePeriod right)
        {
            return (left.Tail.Ticks - 1 < right.Head.Ticks);
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(PricePeriod left, PricePeriod right)
        {
            return
                left.Head == right.Head &&
                left.Tail == right.Tail &&
                left.Open == right.Open &&
                left.High == right.High &&
                left.Low == right.Low &&
                left.Close == right.Close &&
                left.Volume == right.Volume;
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

        /// <summary>
        ///   Returns a <see cref = "T:System.String" /> that represents the current <see cref = "PricePeriod" />.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.String" /> that represents the current <see cref = "PricePeriod" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Head.ToShortDateString() + " close: " + Close;
        }
    }
}