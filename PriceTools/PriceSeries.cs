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
    public class PriceSeries : PricePeriod, IPriceSeries
    {
        #region Private Members

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
            : base()
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
            : base(info, context)
        {
            _periods = (List<PricePeriod>)info.GetValue("Periods", typeof(List<PricePeriod>));
            
            Validate();
        }

        /// <summary>
        /// Serializes a PriceSeries object.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
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
        public new static IPriceSeries BinaryDeserialize(Stream stream)
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
            get { return _periods[index]; }
        }

        decimal ITimeSeries.this[int index]
        {
            get { return _periods[index].Close; }
        }

        /// <summary>
        /// Gets the length of this PriceSeries.
        /// </summary>
        public int Span
        {
            get { return _periods.Count; }
        }

        #endregion

        /// <summary>
        /// Inserts price data from a given PricePeriod into this PriceSeries.
        /// </summary>
        /// <param name="period">A PricePeriod to be added to this PriceSeries.</param>
        public void InsertPeriod(IPricePeriod period)
        {
            _periods.Add(period as PricePeriod);
            if (period.Head < Head || Head == DateTime.MinValue)
            {
                _head = period.Head;
                _open = period.Open;
            }
            if (period.Tail > Tail)
            {
                _tail = period.Tail;
                _close = period.Close;
            }
            if (High == null || period.High > High)
            {
                _high = period.High;
            }
            if (Low == null || period.Low < Low)
            {
                _low = period.Low;
            }
            if (Volume == null)
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
            return base.GetHashCode();
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
