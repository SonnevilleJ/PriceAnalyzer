using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a set of PricePeriods.
    /// </summary>
    [Serializable]
    public class PriceSeries : PricePeriod, IPriceSeries
    {
        #region Private Members

        private readonly List<PricePeriod> _periods;

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs a PriceSeries object from several PricePeriods.
        /// </summary>
        /// <param name = "periods"></param>
        public PriceSeries(params IPricePeriod[] periods)
        {
            if (periods == null)
                throw new ArgumentNullException(
                    "periods", "Argument periods must be a single-dimension array of one or more IPricePeriods.");

            _periods = new List<PricePeriod>(periods.Length);
            Head = periods[0].Head;
            Open = periods[0].Open;

            foreach (IPricePeriod p in periods)
            {
                InsertPeriod(p);
            }
            _periods.Sort((x, y) => DateTime.Compare(x.Head, y.Head));

            Validate(this);
        }

        #endregion

        #region Serialization

        /// <summary>
        ///   Serialization constructor.
        /// </summary>
        /// <param name = "info"></param>
        /// <param name = "context"></param>
        protected PriceSeries(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _periods = (List<PricePeriod>) info.GetValue("Periods", typeof (List<PricePeriod>));

            Validate(this);
        }

        /// <summary>
        ///   Serializes a PriceSeries object.
        /// </summary>
        /// <param name = "info">SerializationInfo</param>
        /// <param name = "context">StreamingContext</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Periods", _periods);
        }

        /// <summary>
        ///   Performs a binary serialization of an IPriceSeries to a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "period">The IPriceSeries to serialize.</param>
        /// <param name = "stream">The <see cref = "Stream" /> to use for serialization.</param>
        public static void BinarySerialize(IPriceSeries period, Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, period);
        }

        /// <summary>
        ///   Performs a binary deserialization of an IPriceSeries from a <see cref = "Stream" />.
        /// </summary>
        /// <param name = "stream">The <see cref = "Stream" /> to use for deserialization.</param>
        /// <returns>An <see cref = "IPriceSeries" />.</returns>
        public new static IPriceSeries BinaryDeserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (PriceSeries) formatter.Deserialize(stream);
        }

        #endregion

        #region CSV Loader

        /// <summary>
        ///   Loads an <see cref = "IPriceSeries" /> from a given CSV stream.
        /// </summary>
        /// <param name = "csvStream">A <see cref = "Stream" /> to the CSV data.</param>
        /// <returns>An <see cref = "IPriceSeries" /> created from the CSV data.</returns>
        public static IPriceSeries LoadFromCsv(Stream csvStream)
        {
            return YahooDataManager.PriceParser.ParsePriceSeries(csvStream);
        }

        /// <summary>
        ///   Loads an <see cref = "IPriceSeries" /> from a given CSV stream.
        /// </summary>
        /// <param name = "csvFilePath">The path to the CSV data.</param>
        /// <returns>An <see cref = "IPriceSeries" /> created from the CSV data.</returns>
        public static IPriceSeries LoadFromCsv(string csvFilePath)
        {
            using (FileStream file = File.OpenRead(csvFilePath))
            {
                return LoadFromCsv(file);
            }
        }

        #endregion

        #region Accessors

        /// <summary>
        ///   Gets a readonly collection of PricePeriod objects stored in this PriceSeries.
        /// </summary>
        public ReadOnlyCollection<PricePeriod> Periods
        {
            get { return _periods.AsReadOnly(); }
        }

        /// <summary>
        ///   Gets the price within this PriceSeries.
        /// </summary>
        /// <param name = "index">The index of the IPricePeriod to retrieve.</param>
        /// <returns>The IPricePeriod at <para>index</para>.</returns>
        public decimal this[int index]
        {
            get { return _periods[index].Close; }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        decimal ITimeSeries.this[DateTime index]
        {
            get { return ((IPriceSeries)this)[index].Close; }
        }

        public IPricePeriod this[DateTime index]
        {
            get
            {
                foreach (PricePeriod period in Periods.Where(period => period.Head <= index && period.Tail >= index))
                {
                    return period;
                }
                throw new ArgumentOutOfRangeException("index", index, String.Format("DateTime {0} was not found in this PriceSeries.", index));
            }
        }

        /// <summary>
        ///   Gets the length of this PriceSeries.
        /// </summary>
        public int Span
        {
            get { return Periods.Count; }
        }

        #endregion

        #region IPriceSeries Members

        /// <summary>
        ///   Inserts price data from a given PricePeriod into this PriceSeries.
        /// </summary>
        /// <param name = "period">A PricePeriod to be added to this PriceSeries.</param>
        public void InsertPeriod(IPricePeriod period)
        {
            _periods.Add(period as PricePeriod);
            if (period.Head < Head || Head == DateTime.MinValue)
            {
                Head = period.Head;
                Open = period.Open;
            }
            if (period.Tail > Tail)
            {
                Tail = period.Tail;
                Close = period.Close;
            }
            if (High == null || period.High > High)
            {
                High = period.High;
            }
            if (Low == null || period.Low < Low)
            {
                Low = period.Low;
            }
            if (Volume == null)
            {
                Volume = period.Volume;
            }
            else
            {
                Volume += period.Volume;
            }
        }

        /// <summary>
        ///   Returns an IPricePeriod with the OHLC data for the entire PriceSeries.
        ///   Note that resolution is lost, because a PricePeriod cannot be broken down into smaller PricePeriods.
        /// </summary>
        /// <returns>An IPricePeriod representing the cumulative price history for the duration of this PriceSeries.</returns>
        public IPricePeriod ToPricePeriod()
        {
            return new PricePeriod(this);
        }

        #endregion

        /// <summary>
        ///   Determines whether the specified <see cref = "PriceSeries" /> is equal to the current <see cref = "PriceSeries" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "PriceSeries" /> is equal to the current <see cref = "PriceSeries" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "PriceSeries" /> to compare with the current <see cref = "PriceSeries" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return (obj is PriceSeries) && (this == (PriceSeries) obj);
        }

        /// <summary>
        ///   Serves as a hash function for the PriceSeries type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
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
        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            bool periodsMatch = !left._periods.Where((t, i) => t != right._periods[i]).Any();
            // Same as below code:
            //for (int i = 0; i < left._periods.Count; i++)
            //{
            //    if(left._periods[i] != right._periods[i])
            //    {
            //        periodsMatch = false;
            //        break;
            //    }
            //}
            return left == (PricePeriod) right && periodsMatch;
        }

        ///<summary>
        ///</summary>
        ///<param name = "left"></param>
        ///<param name = "right"></param>
        ///<returns></returns>
        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return left != (PricePeriod) right;
        }

        /// <summary>
        ///   Performs validation for the PriceSeries.
        /// </summary>
        public override void Validate()
        {
            base.Validate();
        }
    }
}