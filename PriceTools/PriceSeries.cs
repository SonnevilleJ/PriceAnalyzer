using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a set of PricePeriods.
    /// </summary>
    public partial class PriceSeries : PricePeriod, IPriceSeries
    {
        #region Private Members

        private readonly List<PricePeriod> _periods;

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructs a PriceSeries object.
        /// </summary>
        public PriceSeries()
        {
        }

        /// <summary>
        ///   Constructs a PriceSeries object from several PricePeriods.
        /// </summary>
        /// <param name = "periods"></param>
        public PriceSeries(params IPricePeriod[] periods)
        {
            if (periods == null)
                throw new ArgumentNullException(
                    "periods", "Argument periods must be one or more IPricePeriods.");

            foreach (IPricePeriod p in periods)
            {
                InsertPeriod(p);
            }

            Validate();
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
            return YahooPriceSeriesProvider.Instance.ParsePriceSeries(csvStream);
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
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        decimal ITimeSeries.this[DateTime index]
        {
            get { return ((IPriceSeries)this)[index].Close; }
        }

        /// <summary>
        /// Gets the <see cref="IPricePeriod"/> at a given index within this PriceSeries.
        /// </summary>
        /// <param name="index">The index of the <see cref="IPricePeriod"/> to retrieve.</param>
        /// <returns>The <see cref="IPricePeriod"/> stored at the given index.</returns>
        IPricePeriod IPriceSeries.this[DateTime index]
        {
            get
            {
                foreach (PricePeriod period in Periods.Where(period => period.Head <= index && period.Tail >= index))
                {
                    return period;
                }
                throw new ArgumentOutOfRangeException("index", index, String.Format(CultureInfo.CurrentCulture, "DateTime {0} was not found in this PriceSeries.", index));
            }
        }

        #endregion

        #region IPriceSeries Members

        /// <summary>
        ///   Inserts price data from a given PricePeriod into this PriceSeries.
        /// </summary>
        /// <param name = "period">A PricePeriod to be added to this PriceSeries.</param>
        public void InsertPeriod(IPricePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }

            Periods.Add(period as PricePeriod);
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

        #endregion

        #region Equality Checks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            return left.Periods.All(period => right.Periods.Contains(period));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return !(left == right);
        }

        #endregion
    }
}