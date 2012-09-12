using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A generic indicator used to transform <see cref="PriceTools.TimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying data series.
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
        protected Indicator(PriceSeries priceSeries, int lookback)
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
        /// <returns>The value of the TimePeriod as of the given DateTime.</returns>
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
        /// Gets the <see cref="PricePeriod"/> stored at a given index.
        /// </summary>
        /// <param name="index">The index of the <see cref="PricePeriod"/> to get.</param>
        /// <returns>The <see cref="PricePeriod"/> stored at the given index.</returns>
        public PricePeriod this[int index]
        {
            get
            {
                return PricePeriodFactory.ConstructTickedPricePeriod(PriceTickFactory.ConstructPriceTick(ConvertIndexToDateTime(index), IndexedPriceSeriesValues[index]));
            }
        }

        /// <summary>
        /// Gets the lookback of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        public int Lookback { get; private set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        public PriceSeries PriceSeries { get; private set; }

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
        /// Converts a DateTime to the index of the corresponding <see cref="PricePeriod"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> corresponding to the <see cref="PricePeriod"/> to index.</param>
        /// <returns>The index of the corresponding <see cref="PricePeriod"/>.</returns>
        private int ConvertDateTimeToIndex(DateTime dateTime)
        {
            var periods = PriceSeries.GetPricePeriods().Where(period => period.Head <= dateTime).ToList();
            if (!periods.Any())
                throw new ArgumentOutOfRangeException(String.Format("The underlying TimePeriod does not have a value for DateTime: {0}.", dateTime));
            return PriceSeries.GetPricePeriods().IndexOf(periods.Last());
        }

        /// <summary>
        /// Converts the index of the corresponding <see cref="PricePeriod"/> to a DateTime.
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
