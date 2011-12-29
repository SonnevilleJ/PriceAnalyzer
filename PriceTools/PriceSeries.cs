using System;
using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a time series of price data.
    /// </summary>
    public class PriceSeries : PricePeriod, IPriceSeries
    {
        /// <summary>
        /// The default <see cref="Resolution"/> of a PriceSeries.
        /// </summary>
        public const Resolution DefaultResolution = Resolution.Days;

        #region Private Members

        private readonly Resolution _resolution;

        private readonly IList<IPricePeriod> _dataPeriods = new List<IPricePeriod>();

        #endregion

        #region Constructors

        internal PriceSeries()
            : this(DefaultResolution)
        {
        }

        internal PriceSeries(Resolution resolution)
        {
            _resolution = resolution;
        }

        #endregion

        #region Overrides of IPriceSeries

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
        /// Gets the values stored within the ITimeSeries.
        /// </summary>
        public override IDictionary<DateTime, decimal> Values
        {
            get
            {
                var pricePeriods = PricePeriods;
                var dictionary = new Dictionary<DateTime, decimal>(pricePeriods.Count);
                foreach (var pricePeriod in pricePeriods)
                {
                    dictionary.Add(pricePeriod.Head, pricePeriod.Close);
                }
                return dictionary;
            }
        }

        /// <summary>
        /// Determines if the PriceSeries has a valid value for a given date.
        /// </summary>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the PriceSeries has a valid value for the given date.</returns>
        public override bool HasValueInRange(DateTime settlementDate)
        {
            return _dataPeriods.Count > 0 ? base.HasValueInRange(settlementDate) : false;
        }

        /// <summary>
        /// Gets the ticker symbol priced by this IPriceSeries.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        public IList<IPricePeriod> PricePeriods { get { return GetPricePeriods(); } }

        /// <summary>
        /// Gets a collection of reaction moves observed in the IPriceSeries.
        /// </summary>
        public IEnumerable<ReactionMove> ReactionMoves
        {
            get
            {
                var moves = new List<ReactionMove>();

                var todayUp = false;
                var todayDown = false;
                var yesterdayUp = false;
                var yesterdayDown = false;

                var pricePeriods = PricePeriods;
                for (var i = 1; i < pricePeriods.Count; i++)
                {
                    var yesterday = pricePeriods[i - 1];
                    var today = pricePeriods[i];
                    if (i > 1)
                    {
                        yesterdayUp = todayUp;
                        yesterdayDown = todayDown;
                        //yesterdayConverging = todayConverging;
                        //yesterdayWidening = todayWidening;
                    }

                    // calculate change
                    var highChange = today.High - yesterday.High;
                    var higherHigh = highChange > 0;
                    var lowerHigh = highChange < 0;

                    var lowChange = today.Low - yesterday.Low;
                    var higherLow = lowChange > 0;
                    var lowerLow = lowChange < 0;

                    // calculate direction
                    todayUp = higherHigh && higherLow;
                    todayDown = lowerHigh && lowerLow;
                    //todayConverging = ((lowerHigh && !lowerLow) || (higherLow && !higherHigh));
                    var todayWidening = higherHigh && lowerLow;

                    if (i > 1)
                    {
                        if (yesterdayUp && !todayUp && !todayWidening)
                            moves.Add(new ReactionMove {DateTime = yesterday.Head, HighLow = HighLow.High, Reaction = yesterday.High});
                        if (yesterdayDown && !todayDown && !todayWidening)
                            moves.Add(new ReactionMove {DateTime = yesterday.Head, HighLow = HighLow.Low, Reaction = yesterday.Low});
                    }
                }
                return moves;
            }
        }

        /// <summary>
        /// Gets a collection of reaction highs observed in the PriceSeries.
        /// </summary>
        public IEnumerable<ReactionMove> ReactionHighs
        {
            get { return ReactionMoves.Where(rm => rm.HighLow == HighLow.High); }
        }

        /// <summary>
        /// Gets a collection of reaction lows observed in the PriceSeries.
        /// </summary>
        public IEnumerable<ReactionMove> ReactionLows
        {
            get { return ReactionMoves.Where(rm => rm.HighLow == HighLow.Low); }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods()
        {
            return GetPricePeriods(Resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution)
        {
            return _dataPeriods.Count > 0 ? GetPricePeriods(resolution, Head, Tail) : new List<IPricePeriod>();
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the PricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="Resolution"/> of this PriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this PriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail)
        {
            if (resolution < Resolution) throw new InvalidOperationException(String.Format("Unable to get price periods using resolution {0}. Best supported resolution is {1}.", resolution, Resolution));
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
                    select PricePeriodFactory.CreateStaticPricePeriod(periodHead, periodTail, open, high, low, close, volume)).Cast<IPricePeriod>().ToList();
        }

        /// <summary>
        /// Adds price data to the IPriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        public void AddPriceData(IPricePeriod pricePeriod)
        {
            AddPriceData(new [] {pricePeriod});
        }

        /// <summary>
        /// Adds price data to the IPriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        public void AddPriceData(IEnumerable<IPricePeriod> pricePeriods)
        {
            var orderedPeriods = pricePeriods.OrderByDescending(period => period.Head);
            foreach (var pricePeriod in orderedPeriods)
            {
                var period = pricePeriod;
                if (_dataPeriods.Where(p => p.HasValueInRange(period.Head) || p.HasValueInRange(period.Tail)).Count() > 0)
                    throw new InvalidOperationException("Cannot add a PricePeriod for a DateTime range which overlaps that of the PriceSeries.");

                _dataPeriods.Add(period);
            }
            var eventArgs = new NewPriceDataAvailableEventArgs();
            InvokeNewPriceDataAvailable(eventArgs);
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
        /// Gets or sets the resolution of PricePeriods to retrieve.
        /// </summary>
        public override Resolution Resolution
        {
            get { return _resolution; }
        }

        #endregion

        #region Private Methods

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

            if (PricePeriods.Count > 0) return PricePeriods.OrderBy(p => p.Tail).Last(p => p.Tail <= settlementDate).Close;

            throw new InvalidOperationException(String.Format("No price data available for settlement date: {0}", settlementDate));
        }

        #endregion
    }
}
