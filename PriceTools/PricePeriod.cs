using System;
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
        #region Private Members

        private DateTime _head;
        private DateTime _tail;
        private decimal? _open;
        private decimal _close;
        private decimal? _high;
        private decimal? _low;
        private UInt64? _volume;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new PricePeriod object without volume.
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
        /// Constructs a new PricePeriod object.
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

        private PricePeriod(SerializationInfo info, StreamingContext context)
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

        public static void BinarySerialize(IPricePeriod period, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, period);
        }

        public static PricePeriod BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PricePeriod p = (PricePeriod)formatter.Deserialize(stream);
            return p;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the price at the open of this period.
        /// </summary>
        public decimal? Open
        {
            get
            {
                return _open;
            }
            set
            {
                _open = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets or sets the price at the close of this period.
        /// </summary>
        public decimal Close
        {
            get
            {
                return _close;
            }
            set
            {
                _close = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets or sets the highest transaction price during this period.
        /// </summary>
        public decimal? High
        {
            get
            {
                return _high;
            }
            set
            {
                _high = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets or sets the lowest transaction price during this period.
        /// </summary>
        public decimal? Low
        {
            get
            {
                return _low;
            }
            set
            {
                _low = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets or sets the total volume for this period.
        /// </summary>
        public UInt64? Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets or sets the beginning DateTime for this period.
        /// </summary>
        public DateTime Head
        {
            get
            {
                return _head;
            }
            set
            {
                _head = value;
                Validate();
            }
        }

        public DateTime Tail
        {
            get
            {
                return _tail;
            }
            set
            {
                _tail = value;
                Validate();
            }
        }

        /// <summary>
        /// Gets a TimeSpan representing the duration of this period.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return _tail.Subtract(_head);
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            return (obj is PricePeriod) && (this == (PricePeriod)obj);
        }

        public override int GetHashCode()
        {
            return ((((((_head.GetHashCode() << 5)
                        ^ _tail.GetHashCode() << 5)
                       ^ _open.GetHashCode() << 5)
                      ^ _high.GetHashCode() << 5)
                     ^ _low.GetHashCode() << 5)
                    ^ _close.GetHashCode() << 5)
                   ^ _volume.GetHashCode();
        }

        public static bool operator >(PricePeriod lhs, PricePeriod rhs)
        {
            return (lhs._head > rhs._tail);
        }

        public static bool operator <(PricePeriod lhs, PricePeriod rhs)
        {
            return (lhs._tail < rhs._head);
        }

        public static bool operator ==(PricePeriod lhs, PricePeriod rhs)
        {
            return
                lhs._head == rhs._head &&
                lhs.Tail == rhs.Tail &&
                lhs._open == rhs._open &&
                lhs._high == rhs._high &&
                lhs._low == rhs._low &&
                lhs._close == rhs._close &&
                lhs._volume == rhs._volume;
        }

        public static bool operator !=(PricePeriod lhs, PricePeriod rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return _head.ToShortDateString() + " close: " + _close;
        }

        private void Validate()
        {
            List<string> errors = new List<string>();

            if (_head > _tail)
                errors.Add("Head must be earlier than Tail.");
            if (_high < _open || _high < _low || _high < _close)
                errors.Add("High must be greater than or equal to the period's open, low, and close.");
            if (_low > _open || _low > _high || _low > _close)
                errors.Add("Low must be less than or equal to the period's open, high, and close.");

            if (errors.Count != 0)
                throw new InvalidPricePeriodException(errors.ToString());
        }
    }
}
