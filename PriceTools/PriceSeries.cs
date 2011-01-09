﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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

        public bool HasValue(DateTime date)
        {
            return (date >= Head && date <= Tail);
        }

        #endregion

        #region Overridden Equality Members

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

        #endregion
    }
}