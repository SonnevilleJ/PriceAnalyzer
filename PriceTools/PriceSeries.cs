using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    public partial class PriceSeries : IPriceSeries
    {
        #region Constructors

        internal PriceSeries()
        {
        }

        #endregion

        #region Overrides of PricePeriod

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return PricePeriods.Last().Close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal? High
        {
            get { return PricePeriods.Max(p => p.High); }
        }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal? Low
        {
            get { return PricePeriods.Min(p => p.Low); }
        }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal? Open
        {
            get { return PricePeriods.First().Open; }
        }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return PricePeriods.Sum(p => p.Volume); }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the ITimeSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimeSeries as of the given DateTime.</returns>
        public override decimal? this[DateTime index]
        {
            get
            {
                if (!HasValue(index))
                {
                    if (Settings.CanConnectToInternet)
                    {
                        DownloadPriceData(Settings.PreferredPriceSeriesProvider, index);
                    }
                    else
                    {
                        return null;
                    }
                }
                return GetLatestPrice(index);
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get { return PricePeriods.Min(p => p.Head); }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get { return PricePeriods.Max(p => p.Tail); }
        }

        /// <summary>
        /// Determines if the PriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PriceSeries has a valid value for the given date.</returns>
        public override bool HasValue(DateTime settlementDate)
        {
            return PricePeriods.Count > 0 ? base.HasValue(settlementDate) : false;
        }

        #endregion

        #region Private Methods
        
        private void DownloadPriceData(PriceSeriesProvider provider, DateTime index)
        {
            DateTime head = index.Subtract(new TimeSpan(7, 0, 0, 0));
            DateTime tail = index;

            foreach (var pricePeriod in provider.GetPricePeriods(Ticker, head, tail))
            {
                PricePeriods.Add(pricePeriod);
            }
        }

        /// <summary>
        /// Gets the most recent price quote before or at <paramref name="settlementDate"/>.
        /// </summary>
        /// <param name="settlementDate">The DateTime to price.</param>
        /// <returns></returns>
        private decimal? GetLatestPrice(DateTime settlementDate)
        {
            if (PricePeriods.Any(p => p.HasValue(settlementDate)))
            {
                return PricePeriods.Where(p => p.HasValue(settlementDate)).First()[settlementDate];
            }
            return PricePeriods.Where(p => p.Tail <= settlementDate).First().Close;
        }

        #endregion

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            bool pricePeriodsMatch = true;
            if (left.PricePeriods.Count == right.PricePeriods.Count)
            {
                IList<PricePeriod> leftList = left.PricePeriods.ToList();
                IList<PricePeriod> rightList = right.PricePeriods.ToList();

                for (int i = 0; i < leftList.Count; i++)
                {
                    if(leftList[i] != rightList[i])
                    {
                        pricePeriodsMatch = false;
                        break;
                    }
                }
            }

            return pricePeriodsMatch &&
                left.Close == right.Close &&
                left.Head == right.Head &&
                left.High == right.High &&
                left.Low == right.Low &&
                left.Open == right.Open &&
                left.Tail == right.Tail &&
                left.Volume == right.Volume;
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<IPricePeriod>

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this == obj as PriceSeries;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
