using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A generic indicator used to transform <see cref="PriceTools.ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying data series.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        /// <summary>
        /// Stores the calculated values for each period. These values are publically accessible through the <see cref="Indicator"/> indexer.
        /// </summary>
        private IDictionary<int, decimal?> Results { get; set; }

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="PriceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="PriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        protected Indicator(IPriceSeries priceSeries, int lookback)
        {
            if (priceSeries == null)
            {
                throw new ArgumentNullException("priceSeries");
            }
            if(priceSeries.TimeSpan() < new TimeSpan(lookback * (long)priceSeries.Resolution))
            {
                throw new InvalidOperationException("The TimeSpan of priceSeries is too narrow for the given lookback duration.");
            }

            PriceSeries = priceSeries;
            Lookback = lookback;
            Reset();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Head
        {
            get { return PriceSeries[Lookback - 1].Head; }
        }

        /// <summary>
        /// Gets the last DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return PriceSeries.Tail; }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// The indexed values from <see cref="PriceSeries"/>.
        /// </summary>
        protected IDictionary<int, decimal> IndexedPriceSeriesValues { get; private set; }

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected abstract decimal? Calculate(int index);

        #endregion

        #region Implementation of IIndicator

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get
            {
                var i = ConvertDateTimeToIndex(dateTime);
                if (!Results.ContainsKey(i)) CalculateAndCache(dateTime);
                var value = Results[i];
                if (value == null)
                    throw new InvalidOperationException(String.Format("Unable to calculate value for DateTime: {0}",
                                                                      dateTime.ToString(CultureInfo.CurrentCulture)));
                return value.Value;
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this PriceSeries.
        /// </summary>
        public IList<IPricePeriod> PricePeriods
        {
            get { return new List<IPricePeriod>(); }
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        public IList<ITimePeriod> TimePeriods
        {
            get { return PricePeriods.Cast<ITimePeriod>().ToList(); }
        }

        /// <summary>
        /// Gets the <see cref="ITimePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="ITimePeriod"/> to get.</param>
        /// <returns>The <see cref="ITimePeriod"/> stored at the given index.</returns>
        ITimePeriod ITimeSeries.this[int index]
        {
            get { return this[index]; }
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries.
        /// </summary>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public IList<ITimePeriod> GetTimePeriods()
        {
            return GetPricePeriods().Cast<ITimePeriod>().ToList();
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public IList<ITimePeriod> GetTimePeriods(Resolution resolution)
        {
            return GetPricePeriods(resolution).Cast<ITimePeriod>().ToList();
        }

        /// <summary>
        /// Gets a collection of the <see cref="ITimePeriod"/>s in this TimeSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the TimePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="ITimeSeries.Resolution"/> of this TimeSeries.</exception>
        /// <returns>A list of <see cref="ITimePeriod"/>s in the given resolution contained in this TimeSeries.</returns>
        public IList<ITimePeriod> GetTimePeriods(Resolution resolution, DateTime head, DateTime tail)
        {
            return GetPricePeriods(resolution, head, tail).Cast<ITimePeriod>().ToList();
        }

        /// <summary>
        /// Adds Time data to the TimeSeries.
        /// </summary>
        /// <param name="timePeriod"></param>
        public void AddTimeData(ITimePeriod timePeriod)
        {
            AddPriceData(timePeriod as IPricePeriod);
        }

        /// <summary>
        /// Adds Time data to the TimeSeries.
        /// </summary>
        /// <param name="timePeriods"></param>
        public void AddTimeData(IEnumerable<ITimePeriod> timePeriods)
        {
            AddPriceData(timePeriods.Cast<IPricePeriod>());
        }

        /// <summary>
        /// Gets the <see cref="IPricePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="IPricePeriod"/> to get.</param>
        /// <returns>The <see cref="IPricePeriod"/> stored at the given index.</returns>
        public IPricePeriod this[int index]
        {
            get
            {
                return PricePeriodFactory.ConstructTickedPricePeriod(PriceTickFactory.ConstructPriceTick(ConvertIndexToDateTime(index), IndexedPriceSeriesValues[index]));
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries.
        /// </summary>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods()
        {
            return GetPricePeriods(Resolution);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the IPricePeriods.</param>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution)
        {
            return GetPricePeriods(resolution, Head, Tail);
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this IPriceSeries, in a specified <see cref="PriceTools.Resolution"/>.
        /// </summary>
        /// <param name="resolution">The <see cref="PriceTools.Resolution"/> used to view the IPricePeriods.</param>
        /// <param name="head">The head of the periods to retrieve.</param>
        /// <param name="tail">The tail of the periods to retrieve.</param>
        /// <exception cref="InvalidOperationException">Throws if <paramref name="resolution"/> is smaller than the <see cref="PriceTools.IPriceSeries.Resolution"/> of this IPriceSeries.</exception>
        /// <returns>A list of <see cref="IPricePeriod"/>s in the given resolution contained in this IPriceSeries.</returns>
        public IList<IPricePeriod> GetPricePeriods(Resolution resolution, DateTime head, DateTime tail)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriod"></param>
        public void AddPriceData(IPricePeriod pricePeriod)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds price data to the PriceSeries.
        /// </summary>
        /// <param name="pricePeriods"></param>
        public void AddPriceData(IEnumerable<IPricePeriod> pricePeriods)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the lookback of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        public int Lookback { get; private set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        public IPriceSeries PriceSeries { get; private set; }

        /// <summary>
        /// The Resolution of this Indicator.
        /// </summary>
        public Resolution Resolution { get { return PriceSeries.Resolution; } }

        /// <summary>
        /// Determines if the Indicator has a valid value for a given date.
        /// </summary>
        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying PriceSeries.</remarks>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the Indicator has a valid value for the given date.</returns>
        public bool HasValueInRange(DateTime settlementDate)
        {
            return (settlementDate >= Head && settlementDate <= Tail);
        }

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        public virtual void CalculateAll()
        {
            Reset();
            foreach (var index in IndexedPriceSeriesValues.Select(pricePeriod => pricePeriod.Key))
            {
                CalculateAndCache(index);
            }
        }

        #endregion

        #region Private Methods

        private void Reset()
        {
            Results = new Dictionary<int, decimal?>();
            IndexTimeSeriesValues();
        }

        private void IndexTimeSeriesValues()
        {
            IndexedPriceSeriesValues = new Dictionary<int, decimal>();

            var array = PriceSeries.GetPricePeriods();
            for (var i = 0; i < array.Count; i++)
            {
                IndexedPriceSeriesValues.Add(i, array[i].Close);
            }
        }

        private void CalculateAndCache(DateTime index)
        {
            if (!HasValueInRange(index)) throw new ArgumentOutOfRangeException("index", index, Strings.IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator);

            CalculateAndCache(ConvertDateTimeToIndex(index));
        }

        private void CalculateAndCache(int index)
        {
            var result = Calculate(index);
            if (result.HasValue) Results[index] = result;
        }

        /// <summary>
        /// Converts a DateTime to the index of the corresponding <see cref="IPricePeriod"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> corresponding to the <see cref="IPricePeriod"/> to index.</param>
        /// <returns>The index of the corresponding <see cref="IPricePeriod"/>.</returns>
        private int ConvertDateTimeToIndex(DateTime dateTime)
        {
            var periods = PriceSeries.GetPricePeriods().Where(period => period.Head <= dateTime).ToList();
            if (!periods.Any())
                throw new ArgumentOutOfRangeException(String.Format("The underlying ITimePeriod does not have a value for DateTime: {0}.", dateTime));
            return PriceSeries.GetPricePeriods().IndexOf(periods.Last());
        }

        /// <summary>
        /// Converts the index of the corresponding <see cref="IPricePeriod"/> to a DateTime.
        /// </summary>
        /// <param name="index">The index to convert.</param>
        /// <returns>The DateTime of the corresponding <paramref name="index"/>.</returns>
        protected DateTime ConvertIndexToDateTime(int index)
        {
            var values = PriceSeries[index + Lookback];
            return values.Head;
        }

        #endregion
    }
}
