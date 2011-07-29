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
            : this(Settings.PreferredPriceSeriesProvider.BestResolution)
        {
        }

        internal PriceSeries(PriceSeriesResolution priceSeriesResolution)
        {
            Resolution = priceSeriesResolution;
        }

        #endregion

        #region Overrides of IPriceSeries

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
        public override decimal this[DateTime index]
        {
            get
            {
                if (!HasValueInRange(index) && Settings.CanConnectToInternet)
                {
                    DownloadPriceData(Settings.PreferredPriceSeriesProvider, index);
                }

                return GetLatestPrice(index);
            }
        }

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Head
        {
            get
            {
                if(PricePeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return PricePeriods.Min(p => p.Head);
            }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get
            {
                if (PricePeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return PricePeriods.Max(p => p.Tail);
            }
        }

        /// <summary>
        /// Determines if the PriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PriceSeries has a valid value for the given date.</returns>
        public override bool HasValueInRange(DateTime settlementDate)
        {
            return PricePeriods.Count > 0 ? base.HasValueInRange(settlementDate) : false;
        }

        /// <summary>
        /// Downloads price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="head">The first date to retrieve price data for.</param>
        public void DownloadPriceData(DateTime head)
        {
            DownloadPriceData(Settings.PreferredPriceSeriesProvider, head);
        }

        /// <summary>
        /// Downloads price data for the period between the given dates.
        /// </summary>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        public void DownloadPriceData(DateTime head, DateTime tail)
        {
            DownloadPriceData(Settings.PreferredPriceSeriesProvider, head, tail);
        }

        /// <summary>
        /// Downloads price data from the given date until <see cref="DateTime.Now"/>.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        public void DownloadPriceData(PriceSeriesProvider provider, DateTime head)
        {
            DownloadPriceData(provider, head, DateTime.Now);
        }

        /// <summary>
        /// Downloads price data for the period between the given dates.
        /// </summary>
        /// <param name="provider">The <see cref="PriceSeriesProvider"/> to use for retrieving price data.</param>
        /// <param name="head">The first date to retrieve price data for.</param>
        /// <param name="tail">The last date to retrieve price data for.</param>
        public void DownloadPriceData(PriceSeriesProvider provider, DateTime head, DateTime tail)
        {
            DownloadPriceDataIncludingBuffer(provider, head, tail);
        }

        public IList<IPricePeriod> PricePeriods { get { return GetPricePeriods(); } }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        public IList<IPricePeriod> GetPricePeriods()
        {
            return GetPricePeriods(Resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceSeriesResolution"/>.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public IList<IPricePeriod> GetPricePeriods(PriceSeriesResolution resolution)
        {
            if (resolution < Resolution)
            {
                throw new InvalidOperationException(String.Format("Unable to get price periods using resolution {0}. Minimum supported resolution is {1}.",
                                                                  resolution, Resolution));
            }
            return DataPeriods.Cast<IPricePeriod>().OrderBy(period => period.Head).ToList();
        }

        /// <summary>
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        public PriceSeriesResolution Resolution { get; private set; }

        #endregion

        #region Private Methods

        private void DownloadPriceDataIncludingBuffer(PriceSeriesProvider provider, DateTime head, DateTime tail)
        {
            foreach (var pricePeriod in provider.GetPriceHistoryCsvFile(Ticker, head.Subtract(Settings.TimespanToDownload), tail).PricePeriods.OrderByDescending(period => period.Head))
            {
                DataPeriods.Add((PricePeriod) pricePeriod);
            }
        }

        /// <summary>
        /// Gets the most recent price quote before or at <paramref name="settlementDate"/>.
        /// </summary>
        /// <param name="settlementDate">The DateTime to price.</param>
        /// <returns></returns>
        private decimal GetLatestPrice(DateTime settlementDate)
        {
            var matchingPeriods = PricePeriods.Where(p => p.HasValueInRange(settlementDate));
            if (matchingPeriods.Count() > 0) return matchingPeriods.OrderBy(p => p.Tail).Last()[settlementDate];
            if (PricePeriods.Count > 0) return PricePeriods.Where(p => p.Tail <= settlementDate).OrderBy(p => p.Tail).Last().Close;
            throw new InvalidOperationException(String.Format("No price data available for settlement date: {0}", settlementDate));
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

            bool pricePeriodsMatch = false;
            if (left.DataPeriods.Count == right.DataPeriods.Count)
            {
                pricePeriodsMatch = left.DataPeriods.All(right.DataPeriods.Contains);
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
