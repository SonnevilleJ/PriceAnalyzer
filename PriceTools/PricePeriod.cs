using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a single price period. A price period could be any period of time (minute, hour, day, week, month, year, etc).
    /// </summary>
    [Serializable]
    public class PricePeriod : IPricePeriod
    {
        #region Private Members

        private decimal _close;
        private DateTime _head;
        private decimal? _high;
        private decimal? _low;
        private decimal? _open;
        private DateTime _tail;
        private Int64? _volume;

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
            _head = head;
            _tail = tail;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;

            Validate();
        }

        #endregion

        #region Serialization

        /// <summary>
        ///   Serialization constructor.
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected PricePeriod(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _head = (DateTime) info.GetValue("Head", typeof (DateTime));
            _tail = (DateTime) info.GetValue("Tail", typeof (DateTime));
            _open = (decimal?) info.GetValue("Open", typeof (decimal?));
            _high = (decimal?) info.GetValue("High", typeof (decimal?));
            _low = (decimal?) info.GetValue("Low", typeof (decimal?));
            _close = (decimal) info.GetValue("Close", typeof (decimal));
            _volume = (Int64?) info.GetValue("Volume", typeof (Int64?));
        }

        /// <summary>
        ///   Serializes a PricePeriod object.
        /// </summary>
        /// <param name = "info">SerializationInfo</param>
        /// <param name = "context">StreamingContext</param>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Head", _head);
            info.AddValue("Tail", _tail);
            info.AddValue("Open", _open);
            info.AddValue("High", _high);
            info.AddValue("Low", _low);
            info.AddValue("Close", _close);
            info.AddValue("Volume", _volume);
        }

        #endregion

        #region Accessors

        /// <summary>
        ///   Gets or sets the price at the open of this IPricePeriod.
        /// </summary>
        public decimal? Open
        {
            get { return _open; }
            protected set { _open = value; }
        }

        /// <summary>
        ///   Gets or sets the price at the close of this IPricePeriod.
        /// </summary>
        public decimal Close
        {
            get { return _close; }
            protected set { _close = value; }
        }

        /// <summary>
        ///   Gets or sets the highest transaction price during this IPricePeriod.
        /// </summary>
        public decimal? High
        {
            get { return _high; }
            protected set { _high = value; }
        }

        /// <summary>
        ///   Gets or sets the lowest transaction price during this IPricePeriod.
        /// </summary>
        public decimal? Low
        {
            get { return _low; }
            protected set { _low = value; }
        }

        /// <summary>
        ///   Gets or sets the total volume for this IPricePeriod.
        /// </summary>
        public Int64? Volume
        {
            get { return _volume; }
            protected set { _volume = value; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public decimal this[DateTime index]
        {
            get { return HasValue(index) ? Close : 0.00m; }
        }

        /// <summary>
        ///   Gets or sets the beginning DateTime for this IPricePeriod.
        /// </summary>
        public DateTime Head
        {
            get { return _head; }
            protected set { _head = value; }
        }

        /// <summary>
        ///   Gets or sets the ending DateTime for this IPricePeriod.
        /// </summary>
        public DateTime Tail
        {
            get { return _tail; }
            protected set { _tail = value; }
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
            get { return Tail.Subtract(Head); }
        }

        #endregion

        #region IPricePeriod Members

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

        #endregion

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
            return ((((((Head.GetHashCode() << 5)
                        ^ Tail.GetHashCode() << 5)
                       ^ Open.GetHashCode() << 5)
                      ^ High.GetHashCode() << 5)
                     ^ Low.GetHashCode() << 5)
                    ^ Close.GetHashCode() << 5)
                   ^ Volume.GetHashCode();
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