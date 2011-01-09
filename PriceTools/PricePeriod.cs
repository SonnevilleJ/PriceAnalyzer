using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a single price period. A price period could be any period of time (minute, hour, day, week, month, year, etc).
    /// </summary>
    [Serializable]
    public class PricePeriod : IPricePeriod, ISerializable
    {
        #region Private Members

        private decimal _close;
        private DateTime _head;
        private decimal? _high;
        private decimal? _low;
        private decimal? _open;
        private DateTime _tail;
        private UInt64? _volume;

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs an empty PricePeriod.
        /// </summary>
        protected PricePeriod()
        {
        }

        /// <summary>
        ///   Constructs a PricePeriod object without volume.
        /// </summary>
        /// <param name = "head">The beginning of the PricePeriod.</param>
        /// <param name = "tail">The end of the PricePeriod.</param>
        /// <param name = "open">The price at the open of this period.</param>
        /// <param name = "high">The highest price during this period.</param>
        /// <param name = "low">The lowest price during this period.</param>
        /// <param name = "close">The price at the close of this period.</param>
        public PricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close)
            : this(head, tail, open, high, low, close, null)
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
        public PricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close,
                           UInt64? volume)
        {
            _head = head;
            _tail = tail;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _volume = volume;

            Validate(this);
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
            _head = (DateTime) info.GetValue("Head", typeof (DateTime));
            _tail = (DateTime) info.GetValue("Tail", typeof (DateTime));
            _open = (decimal?) info.GetValue("Open", typeof (decimal?));
            _high = (decimal?) info.GetValue("High", typeof (decimal?));
            _low = (decimal?) info.GetValue("Low", typeof (decimal?));
            _close = (decimal) info.GetValue("Close", typeof (decimal));
            _volume = (UInt64?) info.GetValue("Volume", typeof (UInt64?));

            Validate(this);
        }

        /// <summary>
        ///   Serializes a PricePeriod object.
        /// </summary>
        /// <param name = "info">SerializationInfo</param>
        /// <param name = "context">StreamingContext</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Head", _head);
            info.AddValue("Tail", _tail);
            info.AddValue("Open", _open);
            info.AddValue("High", _high);
            info.AddValue("Low", _low);
            info.AddValue("Close", _close);
            info.AddValue("Volume", _volume);

            Validate(this);
        }

        /// <summary>
        ///   Performs a binary serialization of an IPricePeriod to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "period">The IPricePeriod to serialize.</param>
        /// <param name = "stream">The <see cref = "Stream" /> to serialize to.</param>
        public static void BinarySerialize(IPricePeriod period, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, period);
        }

        /// <summary>
        ///   Performs a binary deserialization of an IPricePeriod from a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "stream">The <see cref = "Stream" /> to deserialize from.</param>
        /// <returns>The IPricePeriod object that was deserialized.</returns>
        public static IPricePeriod BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (IPricePeriod) formatter.Deserialize(stream);
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
        public UInt64? Volume
        {
            get { return _volume; }
            protected set { _volume = value; }
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
        ///   Gets a TimeSpan representing the duration of this period.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return Tail.Subtract(Head); }
        }

        #endregion

        #region IPricePeriod Members

        /// <summary>
        ///   Compares the current instance with another object of the same type.
        /// </summary>
        /// <returns>
        ///   A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name = "obj" />. Zero This instance is equal to <paramref name = "obj" />. Greater than zero This instance is greater than <paramref name = "obj" />. 
        /// </returns>
        /// <param name = "obj">An object to compare with this instance. </param>
        /// <exception cref = "T:System.ArgumentException"><paramref name = "obj" /> is not the same type as this instance. </exception>
        /// <filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            PricePeriod target = obj as PricePeriod;
            if (target == null)
            {
                throw new InvalidOperationException("obj is not a PricePeriod.");
            }
            return Head.CompareTo(target.Head);
        }

        /// <summary>
        ///   Validates an IPricePeriod.
        /// </summary>
        /// <param name = "pricePeriod">The IPricePeriod to validate.</param>
        /// <returns>A value indicating if <paramref name = "pricePeriod" /> is valid.</returns>
        public void Validate(PricePeriod pricePeriod)
        {
            List<string> errors = new List<string>();

            if (pricePeriod.Head > pricePeriod.Tail)
                errors.Add("Head must be earlier than Tail.");
            if (pricePeriod.High < pricePeriod.Open || pricePeriod.High < pricePeriod.Low ||
                pricePeriod.High < pricePeriod.Close)
                errors.Add("High must be greater than or equal to the period's open, low, and close.");
            if (pricePeriod.Low > pricePeriod.Open || pricePeriod.Low > pricePeriod.High ||
                pricePeriod.Low > pricePeriod.Close)
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
            return (obj is PricePeriod) && (this == (PricePeriod) obj);
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