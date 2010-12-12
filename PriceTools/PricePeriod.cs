﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single price period. A price period could be any period of time (minute, hour, day, week, month, year, etc).
    /// </summary>
    [Serializable]
    public class PricePeriod : IPricePeriod, ISerializable
    {
        #region Protected Members

        /// <summary>
        /// The internal storage for the Head of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected DateTime _head;

        /// <summary>
        /// The internal storage for the Tail of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected DateTime _tail;

        /// <summary>
        /// The internal storage for the Open of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected decimal? _open;

        /// <summary>
        /// The internal storage for the Close of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected decimal _close;

        /// <summary>
        /// The internal storage for the High of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected decimal? _high;

        /// <summary>
        /// The internal storage for the Low of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected decimal? _low;

        /// <summary>
        /// The internal storage for the Volume of this <see cref="IPricePeriod"/>.
        /// </summary>
        protected UInt64? _volume;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty PricePeriod.
        /// </summary>
        protected PricePeriod()
        {
        }

        /// <summary>
        /// Constructs a PricePeriod object from an existing IPricePeriod.
        /// </summary>
        /// <param name="existing">The IPricePeriod to clone.</param>
        public PricePeriod(IPricePeriod existing)
        {
            _head = existing.Head;
            _tail = existing.Tail;
            _open = existing.Open;
            _high = existing.High;
            _low = existing.Low;
            _close = existing.Close;
            _volume = existing.Volume;

            Validate();
        }

        /// <summary>
        /// Constructs a PricePeriod object without volume.
        /// </summary>
        /// <param name="head">The beginning of the PricePeriod.</param>
        /// <param name="tail">The end of the PricePeriod.</param>
        /// <param name="open">The price at the open of this period.</param>
        /// <param name="high">The highest price during this period.</param>
        /// <param name="low">The lowest price during this period.</param>
        /// <param name="close">The price at the close of this period.</param>
        public PricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close)
            : this(head, tail, open, high, low, close, null)
        {
        }

        /// <summary>
        /// Constructs a PricePeriod object.
        /// </summary>
        /// <param name="head">The beginning DateTime of this period.</param>
        /// <param name="tail">The ending DateTime of this period.</param>
        /// <param name="open">The price at the open of this period.</param>
        /// <param name="high">The highest price during this period.</param>
        /// <param name="low">The lowest price during this period.</param>
        /// <param name="close">The price at the close of this period.</param>
        /// <param name="volume">The total volume during this period.</param>
        public PricePeriod(DateTime head, DateTime tail, decimal? open, decimal? high, decimal? low, decimal close, UInt64? volume)
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
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PricePeriod(SerializationInfo info, StreamingContext context)
        {
            _head = (DateTime)info.GetValue("Head", typeof(DateTime));
            _tail = (DateTime)info.GetValue("Tail", typeof(DateTime));
            _open = (decimal?)info.GetValue("Open", typeof(decimal?));
            _high = (decimal?)info.GetValue("High", typeof(decimal?));
            _low = (decimal?)info.GetValue("Low", typeof(decimal?));
            _close = (decimal)info.GetValue("Close", typeof(decimal));
            _volume = (UInt64?)info.GetValue("Volume", typeof(UInt64?));

            Validate();
        }

        /// <summary>
        /// Serializes a PricePeriod object.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Head", _head);
            info.AddValue("Tail", _tail);
            info.AddValue("Open", _open);
            info.AddValue("High", _high);
            info.AddValue("Low", _low);
            info.AddValue("Close", _close);
            info.AddValue("Volume", _volume);
        }

        /// <summary>
        /// Performs a binary serialization of an IPricePeriod to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="period">The IPricePeriod to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        public static void BinarySerialize(IPricePeriod period, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, period);
        }

        /// <summary>
        /// Performs a binary deserialization of an IPricePeriod from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to deserialize from.</param>
        /// <returns>The IPricePeriod object that was deserialized.</returns>
        public static IPricePeriod BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (IPricePeriod)formatter.Deserialize(stream);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the price at the open of this IPricePeriod.
        /// </summary>
        public decimal? Open
        {
            get { return _open; }
        }

        /// <summary>
        /// Gets or sets the price at the close of this IPricePeriod.
        /// </summary>
        public decimal Close
        {
            get { return _close; }
        }

        /// <summary>
        /// Gets or sets the highest transaction price during this IPricePeriod.
        /// </summary>
        public decimal? High
        {
            get { return _high; }
        }

        /// <summary>
        /// Gets or sets the lowest transaction price during this IPricePeriod.
        /// </summary>
        public decimal? Low
        {
            get { return _low; }
        }

        /// <summary>
        /// Gets or sets the total volume for this IPricePeriod.
        /// </summary>
        public UInt64? Volume
        {
            get { return _volume; }
        }

        /// <summary>
        /// Gets or sets the beginning DateTime for this IPricePeriod.
        /// </summary>
        public DateTime Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Gets or sets the ending DateTime for this IPricePeriod.
        /// </summary>
        public DateTime Tail
        {
            get { return _tail; }
        }

        /// <summary>
        /// Gets a TimeSpan representing the duration of this period.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return Tail.Subtract(Head); }
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="PricePeriod"/> is equal to the current <see cref="PricePeriod"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="PricePeriod"/> is equal to the current <see cref="PricePeriod"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="PricePeriod"/> to compare with the current <see cref="PricePeriod"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return (obj is PricePeriod) && (this == (PricePeriod)obj);
        }

        /// <summary>
        /// Serves as a hash function for the PricePeriod type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="PricePeriod"/>.
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
        /// 
        ///</summary>
        ///<param name="lhs"></param>
        ///<param name="rhs"></param>
        ///<returns></returns>
        public static bool operator >(PricePeriod lhs, PricePeriod rhs)
        {
            return (lhs.Head.Ticks + 1 > rhs.Tail.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(PricePeriod lhs, PricePeriod rhs)
        {
            return (lhs.Tail.Ticks - 1 < rhs.Head.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(PricePeriod lhs, PricePeriod rhs)
        {
            return
                lhs.Head == rhs.Head &&
                lhs.Tail == rhs.Tail &&
                lhs.Open == rhs.Open &&
                lhs.High == rhs.High &&
                lhs.Low == rhs.Low &&
                lhs.Close == rhs.Close &&
                lhs.Volume == rhs.Volume;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(PricePeriod lhs, PricePeriod rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="PricePeriod"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="PricePeriod"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Head.ToShortDateString() + " close: " + Close;
        }

        /// <summary>
        /// Performs validation for the PricePeriod.
        /// </summary>
        protected virtual void Validate()
        {
            List<string> errors = new List<string>();

            if (Head > Tail)
                errors.Add("Head must be earlier than Tail.");
            if (High < Open || High < Low || High < Close)
                errors.Add("High must be greater than or equal to the period's open, low, and close.");
            if (Low > Open || Low > High || Low > Close)
                errors.Add("Low must be less than or equal to the period's open, high, and close.");

            if (errors.Count != 0)
                throw new InvalidPricePeriodException(errors.ToString());
        }
    }
}
