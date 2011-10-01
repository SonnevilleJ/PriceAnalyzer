using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Extensions;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public partial class PriceSeries : IPriceSeries
    {
        #region Private Members

        private readonly Resolution _resolution;

        #endregion

        #region Constructors

        internal PriceSeries()
            : this(Settings.PreferredPriceSeriesProvider.BestResolution)
        {
        }

        internal PriceSeries(Resolution resolution)
        {
            _resolution = resolution;
        }

        #endregion

        #region Overrides of IPriceSeries

        /// <summary>
        /// Gets the closing price for the IPricePeriod.
        /// </summary>
        public override decimal Close
        {
            get { return DataPeriods.OrderBy(p => p.Tail).Last().Close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal? High
        {
            get { return DataPeriods.Max(p => p.High); }
        }

        /// <summary>
        /// Gets the lowest price that occurred during the IPricePeriod.
        /// </summary>
        public override decimal? Low
        {
            get { return DataPeriods.Min(p => p.Low); }
        }

        /// <summary>
        /// Gets the opening price for the IPricePeriod.
        /// </summary>
        public override decimal? Open
        {
            get { return DataPeriods.OrderBy(p => p.Head).First().Open; }
        }

        /// <summary>
        /// Gets the total volume of trades during the IPricePeriod.
        /// </summary>
        public override long? Volume
        {
            get { return DataPeriods.Sum(p => p.Volume); }
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
                    var dateTime = DataPeriods.Count != 0 ? Tail : index;
                    DownloadPriceData(Settings.PreferredPriceSeriesProvider, dateTime);
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
                if(DataPeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return DataPeriods.Min(p => p.Head);
            }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public override DateTime Tail
        {
            get
            {
                if (DataPeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return DataPeriods.Max(p => p.Tail);
            }
        }

        /// <summary>
        /// Determines if the PriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PriceSeries has a valid value for the given date.</returns>
        public override bool HasValueInRange(DateTime settlementDate)
        {
            return DataPeriods.Count > 0 ? base.HasValueInRange(settlementDate) : false;
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

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        public IList<IPricePeriod> PricePeriods { get { return GetPricePeriods(); } }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods()
        {
            return GetPricePeriods(Resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution)
        {
            return GetPricePeriods(resolution, Head, Tail);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this IPriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail)
        {
            if (resolution < Resolution)
            {
                throw new InvalidOperationException(String.Format("Unable to get price periods using resolution {0}. Best supported resolution is {1}.",
                                                                  resolution, Resolution));
            }
            var dataPeriods = DataPeriods.Where(period => period.Head >= head && period.Tail <= tail).Cast<IPricePeriod>().OrderBy(period => period.Head).ToList();
            if (resolution == Resolution) return dataPeriods;

            var pairs = GetPairs(resolution, head, tail);

            return (from pair in pairs
                    let periodHead = pair.Key
                    let periodTail = pair.Value
                    let periodsInRange = dataPeriods.Where(period => period.Head >= periodHead && period.Tail <= periodTail).ToList()
                    let open = periodsInRange.First().Open
                    let high = periodsInRange.Max(p => p.High)
                    let low = periodsInRange.Min(p => p.Low)
                    let close = periodsInRange.Last().Close
                    let volume = periodsInRange.Sum(p => p.Volume)
                    select PricePeriodFactory.CreateStaticPricePeriod(periodHead, periodTail, open, high, low, close, volume)).Cast<IPricePeriod>().ToList();
        }

        private delegate DateTime GetDateTime(DateTime date);

        private static IEnumerable<KeyValuePair<DateTime, DateTime>> GetPairs(Resolution resolution, DateTime head, DateTime tail)
        {
            if (head.DayOfWeek == DayOfWeek.Saturday || head.DayOfWeek == DayOfWeek.Sunday)
            {
                head = head.GetFollowingOpen();
            }
            
            GetDateTime getPeriodClose = null;
            GetDateTime getNextOpen = null;
            GetDelegates(resolution, ref getPeriodClose, ref getNextOpen);

            var list = new List<KeyValuePair<DateTime, DateTime>>();
            while (head < tail)
            {
                var periodClose = getPeriodClose(head);
                var lastDay = periodClose > tail ? tail : periodClose;
                list.Add(new KeyValuePair<DateTime, DateTime>(head, lastDay));
                head = getNextOpen(lastDay);
            }
            return list;
        }

        private static void GetDelegates(Resolution resolution, ref GetDateTime getPeriodClose, ref GetDateTime getNextOpen)
        {
            switch (resolution)
            {
                case PriceTools.Resolution.Days:
                    getPeriodClose = DateTimeExtensions.GetFollowingClose;
                    getNextOpen = DateTimeExtensions.GetFollowingOpen;
                    break;
                case PriceTools.Resolution.Weeks:
                    getPeriodClose = DateTimeExtensions.GetFollowingWeeklyClose;
                    getNextOpen = DateTimeExtensions.GetFollowingWeeklyOpen;
                    break;
                case PriceTools.Resolution.Months:
                    getPeriodClose = DateTimeExtensions.GetFollowingMonthlyClose;
                    getNextOpen = DateTimeExtensions.GetFollowingMonthlyOpen;
                    break;
                default:
                    throw new NotSupportedException(String.Format("Resolution {0} not supported.", resolution));
            }
        }

        /// <summary>
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        public override Resolution Resolution
        {
            get { return _resolution; }
        }

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
        /// Gets the most recent price at or before <paramref name="settlementDate"/>.
        /// </summary>
        /// <param name="settlementDate">The DateTime to price.</param>
        /// <exception cref="InvalidOperationException">Throws if no price is available at or before <paramref name="settlementDate"/>.</exception>
        /// <returns>The most recent price at or before <paramref name="settlementDate"/>.</returns>
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

            var pricePeriodsMatch = false;
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
