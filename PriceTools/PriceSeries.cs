using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a set of PricePeriods.
    /// </summary>
    [Serializable]
    public class PriceSeries : IPriceSeries
    {
        #region Private Members

        private DateTime _head;
        private DateTime _tail;
        private decimal? _open;
        private decimal _close;
        private decimal? _high;
        private decimal? _low;
        private UInt64? _volume;
        private readonly long _resolution;

        private List<PricePeriod> _periods;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a PriceSeries object from several PricePeriods.
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="periods"></param>
        public PriceSeries(PriceSeriesResolution resolution, params IPricePeriod[] periods)
        {
            if (periods == null)
                throw new ArgumentNullException(
                    "periods", "Argument periods must be a single-dimension array of one or more IPricePeriods.");

            _resolution = (long)resolution;
            _periods = new List<PricePeriod>(periods.Length);
            _head = periods[0].Head;
            _open = periods[0].Open;

            foreach (IPricePeriod p in periods)
            {
                if (p.TimeSpan.Ticks > _resolution)
                {
                    throw new ArgumentException(String.Format("Period {0} has an unexpected resolution. Expected {1}.", p, _resolution));
                }
                InsertPeriod(p);
            }
            _periods.Sort((x, y) => DateTime.Compare(x.Head, y.Head));
        }


        #endregion

        #region Serialization

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PriceSeries(SerializationInfo info, StreamingContext context)
        {
            _head = (DateTime)info.GetValue("Head", typeof(DateTime));
            _tail = (DateTime)info.GetValue("Tail", typeof(DateTime));
            _open = (decimal?)info.GetValue("Open", typeof(decimal?));
            _high = (decimal?)info.GetValue("High", typeof(decimal?));
            _low = (decimal?)info.GetValue("Low", typeof(decimal?));
            _close = (decimal)info.GetValue("Close", typeof(decimal));
            _volume = (UInt64?)info.GetValue("Volume", typeof(UInt64?));
            _periods = (List<PricePeriod>)info.GetValue("Periods", typeof(List<PricePeriod>));

            Validate();
        }

        /// <summary>
        /// Serializes a PriceSeries object.
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
            info.AddValue("Periods", _periods);
        }

        /// <summary>
        /// Performs a binary serialization of an IPriceSeries to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="period">The IPriceSeries to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to use for serialization.</param>
        public static void BinarySerialize(IPriceSeries period, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, period);
        }

        /// <summary>
        /// Performs a binary deserialization of an IPriceSeries from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to use for deserialization.</param>
        /// <returns>An <see cref="IPriceSeries"/>.</returns>
        public static IPriceSeries BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (PriceSeries)formatter.Deserialize(stream);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets a collection of PricePeriod objects stored in this PriceSeries.
        /// </summary>
        public IPricePeriod[] Periods
        {
            get
            {
                return _periods.ToArray();
            }
        }

        /// <summary>
        /// Gets an IPricePeriod within this PriceSeries.
        /// </summary>
        /// <param name="index">The index of the IPricePeriod to retrieve.</param>
        /// <returns>The IPricePeriod at <para>index</para>.</returns>
        public IPricePeriod this[int index]
        {
            get
            {
                return _periods[index];
            }
        }

        /// <summary>
        /// Gets the length of this PriceSeries.
        /// </summary>
        public int Length
        {
            get
            {
                return _periods.Count;
            }
        }

        /// <summary>
        /// Gets or sets the price at the open of this PriceSeries.
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
        /// Gets or sets the price at the close of this PriceSeries.
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
        /// Gets or sets the highest transaction price during this PriceSeries.
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
        /// Gets or sets the lowest transaction price during this PriceSeries.
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
        /// Gets or sets the total volume for this PriceSeries.
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
        /// Gets or sets the beginning DateTime for this PriceSeries.
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

        /// <summary>
        /// Gets or sets the ending DateTime for this PriceSeries.
        /// </summary>
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
        /// Gets a TimeSpan representing the duration of this PriceSeries.
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
        /// Inserts price data from a given PricePeriod into this PriceSeries.
        /// </summary>
        /// <param name="period">A PricePeriod to be added to this PriceSeries.</param>
        public void InsertPeriod(IPricePeriod period)
        {
            _periods.Add(period as PricePeriod);
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
        /// Returns an IPricePeriod with the OHLC data for the entire PriceSeries.
        /// Note that resolution is lost, because a PricePeriod cannot be broken down into smaller PricePeriods.
        /// </summary>
        /// <returns>An IPricePeriod representing the cumulative price history for the duration of this PriceSeries.</returns>
        public IPricePeriod ToPricePeriod()
        {
            return new PricePeriod(this);
        }

        ///<summary>
        ///</summary>
        ///<param name="series"></param>
        ///<returns></returns>
        public static implicit operator PricePeriod(PriceSeries series)
        {
            return new PricePeriod(series._head, series._tail, series._open, series._high, series._low, series._close, series._volume);
        }

        /// <summary>
        /// Determines whether the specified <see cref="PriceSeries"/> is equal to the current <see cref="PriceSeries"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="PriceSeries"/> is equal to the current <see cref="PriceSeries"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="PriceSeries"/> to compare with the current <see cref="PriceSeries"/>. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return (obj is PriceSeries) && (this == (PriceSeries)obj);
        }

        /// <summary>
        /// Serves as a hash function for the PriceSeries type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ((PricePeriod)this).GetHashCode();
        }

        ///<summary>
        ///</summary>
        ///<param name="lhs"></param>
        ///<param name="rhs"></param>
        ///<returns></returns>
        public static bool operator ==(PriceSeries lhs, PriceSeries rhs)
        {
            bool periodsMatch = !lhs._periods.Where((t, i) => t != rhs._periods[i]).Any();
            // Same as below code:
            //for (int i = 0; i < lhs._periods.Count; i++)
            //{
            //    if(lhs._periods[i] != rhs._periods[i])
            //    {
            //        periodsMatch = false;
            //        break;
            //    }
            //}
            return (PricePeriod) lhs == (PricePeriod) rhs && periodsMatch;
        }

        ///<summary>
        ///</summary>
        ///<param name="lhs"></param>
        ///<param name="rhs"></param>
        ///<returns></returns>
        public static bool operator !=(PriceSeries lhs, PriceSeries rhs)
        {
            return (PricePeriod)lhs != (PricePeriod)rhs;
        }


        private void Validate()
        {
        }
    }
}