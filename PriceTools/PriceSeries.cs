using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public class PriceSeries : PricePeriod, IEquatable<PriceSeries>
    {
        /// <summary>
        /// The default <see cref="Resolution"/> of a PriceSeries.
        /// </summary>
        private const Resolution DefaultResolution = Resolution.Days;

        #region Private Members

        private readonly Resolution _resolution;

        private readonly IList<PricePeriod> _dataPeriods = new List<PricePeriod>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructs a PriceSeries object.
        /// </summary>
        /// <param name="resolution"></param>
        protected internal PriceSeries(Resolution resolution = DefaultResolution)
        {
            _resolution = resolution;
        }

        #endregion

        #region Overrides of PriceSeries

        /// <summary>
        /// Gets the closing price for the PriceSeries.
        /// </summary>
        public override decimal Close
        {
            get { return _dataPeriods.OrderBy(p => p.Tail).Last().Close; }
        }

        /// <summary>
        /// Gets the highest price that occurred during the PriceSeries.
        /// </summary>
        public override decimal High
        {
            get { return _dataPeriods.Max(p => p.High); }
        }

        /// <summary>
        /// Gets the lowest price that occurred during the PriceSeries.
        /// </summary>
        public override decimal Low
        {
            get { return _dataPeriods.Min(p => p.Low); }
        }

        /// <summary>
        /// Gets the opening price for the PriceSeries.
        /// </summary>
        public override decimal Open
        {
            get { return _dataPeriods.OrderBy(p => p.Head).First().Open; }
        }

        /// <summary>
        /// Gets the total volume of trades during the PriceSeries.
        /// </summary>
        public override long? Volume
        {
            get { return _dataPeriods.Sum(p => p.Volume); }
        }

        /// <summary>
        /// Gets a value stored at a given DateTime index of the PriceSeries.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the PriceSeries as of the given DateTime.</returns>
        public override decimal this[DateTime index]
        {
            get
            {
                return index < Head ? 0.0m : GetLatestPrice(index);
            }
        }

        /// <summary>
        /// Gets the <see cref="PricePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PricePeriod"/> to get.</param>
        /// <returns>The <see cref="PricePeriod"/> stored at the given index.</returns>
        public virtual PricePeriod this[int index]
        {
            get { return PricePeriods[index]; }
        }

        /// <summary>
        /// Gets the first DateTime in the PriceSeries.
        /// </summary>
        public override DateTime Head
        {
            get
            {
                if(_dataPeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return _dataPeriods.Min(p => p.Head);
            }
        }

        /// <summary>
        /// Gets the last DateTime in the PriceSeries.
        /// </summary>
        public override DateTime Tail
        {
            get
            {
                if (_dataPeriods.Count == 0) throw new InvalidOperationException("PriceSeries contains no PricePeriods.");
                return _dataPeriods.Max(p => p.Tail);
            }
        }

        /// <summary>
        /// Determines if the PricePeriod has any data at all. PricePeriods with no data are not equal.
        /// </summary>
        protected override bool HasData
        {
            get { return _dataPeriods.Count > 0; }
        }

        /// <summary>
        /// Determines if the PriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PriceSeries has a valid value for the given date.</returns>
        public override bool HasValueInRange(DateTime settlementDate)
        {
            return _dataPeriods.Count > 0 && base.HasValueInRange(settlementDate);
        }

        /// <summary>
        /// Gets the ticker symbol priced by this PriceSeries.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries.
        /// </summary>
        public IList<PricePeriod> PricePeriods { get { return GetPricePeriods(); } }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<PricePeriod> GetPricePeriods()
        {
            return GetPricePeriods(Resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<PricePeriod> GetPricePeriods(Resolution resolution)
        {
            if (HasData) return GetPricePeriods(resolution, Head, Tail);
            if (resolution < Resolution) throw new InvalidOperationException(String.Format(Strings.PriceSeries_GetPricePeriods_Unable_to_get_price_periods_using_resolution__0___Best_supported_resolution_is__1__, resolution, Resolution));
            return new List<PricePeriod>();
        }

        /// <summary>
        /// Gets a collection of the <see cref="PricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="PricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<PricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail)
        {
            if (resolution < Resolution) throw new InvalidOperationException(String.Format(Strings.PriceSeries_GetPricePeriods_Unable_to_get_price_periods_using_resolution__0___Best_supported_resolution_is__1__, resolution, Resolution));
            var dataPeriods = _dataPeriods.Where(period => period.Head >= head && period.Tail <= tail).OrderBy(period => period.Head).ToList();
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
                    select PricePeriodFactory.ConstructStaticPricePeriod(periodHead, periodTail, open, high, low, close, volume)).ToList();
        }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        public void AddPriceData(PricePeriod pricePeriod)
        {
            AddPriceData(new [] {pricePeriod});
        }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        public void AddPriceData(IEnumerable<PricePeriod> pricePeriods)
        {
            var list = pricePeriods.Where(period => !HasValueInRange(period.Head) && !HasValueInRange(period.Tail)).ToList();

            if (list.Any())
            {
                foreach (var pricePeriod in list)
                {
                    _dataPeriods.Add(pricePeriod);
                }
                var eventArgs = new NewDataAvailableEventArgs
                                    {
                                        Head = list.Min(p => p.Head),
                                        Tail = list.Max(p => p.Tail)
                                    };
                InvokeNewDataAvailable(eventArgs);
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
                case Resolution.Days:
                    getPeriodClose = DateTimeExtensions.GetFollowingClose;
                    getNextOpen = DateTimeExtensions.GetFollowingOpen;
                    break;
                case Resolution.Weeks:
                    getPeriodClose = DateTimeExtensions.GetFollowingWeeklyClose;
                    getNextOpen = DateTimeExtensions.GetFollowingWeeklyOpen;
                    break;
                case Resolution.Months:
                    getPeriodClose = DateTimeExtensions.GetFollowingMonthlyClose;
                    getNextOpen = DateTimeExtensions.GetFollowingMonthlyOpen;
                    break;
                default:
                    throw new NotSupportedException(String.Format("Resolution {0} not supported.", resolution));
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
            if (matchingPeriods.Any()) return matchingPeriods.OrderBy(p => p.Tail).Last()[settlementDate];

            if (PricePeriods.Count > 0) return PricePeriods.OrderBy(p => p.Tail).Last(p => p.Tail <= settlementDate).Close;

            throw new InvalidOperationException(String.Format("No price data available for settlement date: {0}", settlementDate));
        }

        #endregion

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PriceSeries other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) &&
                   Ticker == other.Ticker &&
                   other.PricePeriods.All(pricePeriod => PricePeriods.Contains(pricePeriod));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">An object to compare with this object.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as PriceSeries);
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
            unchecked
            {
                return Ticker.GetHashCode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PriceSeries left, PriceSeries right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PriceSeries left, PriceSeries right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
