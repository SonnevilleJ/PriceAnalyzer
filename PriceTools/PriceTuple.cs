using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a set of PricePeriods.
    /// </summary>
    [Serializable]
    public class PriceTuple : IPriceTuple
    {
        #region Private Members

        private DateTime _head;
        private DateTime _tail;
        private decimal? _open;
        private decimal _close;
        private decimal? _high;
        private decimal? _low;
        private UInt64? _volume;
        private long _resolution;

        private List<PricePeriod> _periods;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a PriceTuple object from several PricePeriods.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="periods"></param>
        public PriceTuple(PriceTupleResolution resolution, params IPricePeriod[] periods)
        {
            if (periods == null)
                throw new ArgumentNullException(
                    "periods", "Argument periods must be a single-dimension array of one or more PricePeriods.");

            _resolution = (long)resolution;
            _periods = new List<PricePeriod>(periods.Length);
            _head = periods[0].Head;
            _open = periods[0].Open;

            foreach (PricePeriod p in periods)
            {
                if (p.TimeSpan.Ticks > _resolution)
                {
                    throw new ArgumentException(String.Format("Period {0} has an unexpected resolution. Expected {1}.", p, _resolution));
                }
                AddPeriod(p);
            }
            _periods.Sort((x, y) => DateTime.Compare(x.Head, y.Head));
        }


        #endregion

        #region Serialization

        /// <summary>
        /// Deserializes a PriceTuple object.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public PriceTuple(SerializationInfo info, StreamingContext context)
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
        /// Serializes a PriceTuple object.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Head", _head);
            info.AddValue("Tail", _tail);
            info.AddValue("Open", _open);
            info.AddValue("High", _high);
            info.AddValue("Low", _low);
            info.AddValue("Close", _close);
            info.AddValue("Volume", _volume);
        }

        public static void BinarySerialize(IPriceTuple period, string filename)
        {
            string dir = Path.GetDirectoryName(filename);
            Directory.CreateDirectory(dir);
            Stream s = File.Open(filename, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(s, period);
            s.Close();
        }

        public static PriceTuple BinaryDeserialize(string filename)
        {
            Stream s = File.Open(filename, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            PriceTuple p = (PriceTuple)formatter.Deserialize(s);
            s.Close();
            return p;
        }

        #endregion

        #region Accessors

        public IPricePeriod[] Periods
        {
            get
            {
                return _periods.ToArray();
            }
        }

        public IPricePeriod this[int index]
        {
            get
            {
                return _periods[index];
            }
        }

        /// <summary>
        /// Gets or sets the price at the open of this tuple.
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
        /// Gets or sets the price at the close of this tuple.
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
        /// Gets or sets the highest transaction price during this tuple.
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
        /// Gets or sets the lowest transaction price during this tuple.
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
        /// Gets or sets the total volume for this tuple.
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
        /// Gets or sets the beginning DateTime for this tuple.
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
        /// Gets a TimeSpan representing the duration of this tuple.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return _tail.Subtract(_head);
            }
        }

        #endregion

        /// <summary>
        /// Adds price data from a given PricePeriod into this PriceTuple.
        /// </summary>
        /// <param name="period">A PricePeriod to be added to this PriceTuple.</param>
        public void AddPeriod(PricePeriod period)
        {
            _periods.Add(period);
            if (period.Head < _head || _head == DateTime.MinValue)
            {
                _head = period.Head;
                _open = period.Open;
            }
            if (period.Tail > _tail)
            {
                _tail = period.Tail;
                _close = period.Close;
            }
            if (period.High > _high || _high == null)
            {
                _high = period.High;
            }
            if (period.Low < _low || _low == null)
            {
                _low = period.Low;
            }
            if (_volume == null)
            {
                _volume = period.Volume;
            }
            else
            {
                _volume += period.Volume;
            }
        }

        /// <summary>
        /// Returns a PricePeriod with the OHLC data for the entire tuple.
        /// Note that resolution is lost, because a PricePeriod cannot be broken down into smaller PricePeriods.
        /// </summary>
        /// <returns>A PricePeriod representing the cumulative price history for the duration of this tuple.</returns>
        public IPricePeriod ToPricePeriod()
        {
            return (PricePeriod)this;
        }

        public static implicit operator PricePeriod(PriceTuple tuple)
        {
            return new PricePeriod(tuple._head, tuple._tail, tuple._open, tuple._high, tuple._low, tuple._close, tuple._volume);
        }

        public override bool Equals(object obj)
        {
            return (obj is PriceTuple) && (this == (PriceTuple)obj);
        }

        public override int GetHashCode()
        {
            return ((PricePeriod)this).GetHashCode();
        }

        public static bool operator ==(PriceTuple lhs, PriceTuple rhs)
        {
            return (PricePeriod)lhs == (PricePeriod)rhs;
        }

        public static bool operator !=(PriceTuple lhs, PriceTuple rhs)
        {
            return (PricePeriod)lhs != (PricePeriod)rhs;
        }

        private void Validate()
        {
        }
    }

    /// <summary>
    /// Specifies the resolution of a PriceTuple.
    /// </summary>
    public enum PriceTupleResolution : long
    {
        Seconds = TimeSpan.TicksPerSecond,
        Minutes = TimeSpan.TicksPerMinute,
        Hours = TimeSpan.TicksPerMinute,
        Days = TimeSpan.TicksPerDay,
        Weeks = TimeSpan.TicksPerDay * 7
    }
}
