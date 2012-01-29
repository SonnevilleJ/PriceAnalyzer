using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A generic indicator used to transform <see cref="ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="PriceSeries"/>.
        /// </summary>
        /// <param name="timeSeries">The <see cref="PriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        protected Indicator(ITimeSeries timeSeries, int lookback)
        {
            if (timeSeries == null)
            {
                throw new ArgumentNullException("timeSeries");
            }
            if(timeSeries.TimeSpan() < new TimeSpan(lookback * (long)timeSeries.Resolution))
            {
                throw new InvalidOperationException("The TimeSpan of timeSeries is too narrow for the given lookback duration.");
            }

            TimeSeries = timeSeries;
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
            get { return TimeSeries.Values.ToArray()[Lookback - 1].Key; }
        }

        /// <summary>
        /// Gets the last DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return TimeSeries.Tail; }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// The indexed values from <see cref="TimeSeries"/>.
        /// </summary>
        protected IDictionary<int, decimal> IndexedTimeSeriesValues { get; private set; }

        /// <summary>
        /// Contains boolean values indicating whether or not all required data is available for calculation of later periods.
        /// </summary>
        /// <remarks>Note that inheriting classes must store this data separately.</remarks>
        protected IDictionary<int, bool> PreCalculatedPeriods { get; private set; }

        /// <summary>
        /// Stores the calculated values for each period. These values are publically accessible through the <see cref="Indicator"/> indexer.
        /// </summary>
        protected IDictionary<int, decimal?> Results { get; private set; }

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected abstract void Calculate(int index);

        #endregion

        #region Implementation of IIndicator

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>THe value of the ITimeSeries as of the given DateTime.</returns>
        public virtual decimal this[DateTime index]
        {
            get
            {
                var i = ConvertDateTimeToIndex(index);
                if (!Results.ContainsKey(i)) CalculatePeriod(index);
                return Results[i].Value;
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
        public ITimeSeries TimeSeries { get; private set; }

        /// <summary>
        /// The Resolution of this Indicator.
        /// </summary>
        public Resolution Resolution { get { return TimeSeries.Resolution; } }

        /// <summary>
        /// Gets the calculated values of the Indicator.
        /// </summary>
        public IDictionary<DateTime, decimal> Values
        {
            get
            {
                var dictionary = new Dictionary<DateTime, decimal>(Results.Count);
                foreach (var result in Results)
                {
                    var value = result.Value;
                    if(value.HasValue) dictionary.Add(ConvertIndexToDateTime(result.Key), value.Value);
                }
                return dictionary;
            }
        }

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
            foreach (var pricePeriod in IndexedTimeSeriesValues)
            {
                Calculate(pricePeriod.Key);
            }
        }

        #endregion

        #region Private Methods

        private void Reset()
        {
            PreCalculatedPeriods = new Dictionary<int, bool>();
            Results = new Dictionary<int, decimal?>(TimeSeries.Values.Count - Lookback);
            IndexTimeSeriesValues();
        }

        private void IndexTimeSeriesValues()
        {
            IndexedTimeSeriesValues = new Dictionary<int, decimal>();

            var array = TimeSeries.Values.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                IndexedTimeSeriesValues.Add(i, array[i].Value);
            }
        }

        private void CalculatePeriod(DateTime index)
        {
            if (!HasValueInRange(index)) throw new ArgumentOutOfRangeException("index", index, Strings.IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator);

            Calculate(ConvertDateTimeToIndex(index));
        }

        /// <summary>
        /// Converts a DateTime to the index of the corresponding <see cref="PricePeriod"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> corresponding to the <see cref="PricePeriod"/> to index.</param>
        /// <returns>The index of the corresponding <see cref="PricePeriod"/>.</returns>
        private int ConvertDateTimeToIndex(DateTime dateTime)
        {
            var values = TimeSeries.Values.ToList();
            var periods = values.Where(kvp => kvp.Key <= dateTime);
            if (periods.Count() < 1)
                throw new ArgumentOutOfRangeException(String.Format("The underlying TimeSeries does not have a value for DateTime: {0}.", dateTime));
            return values.IndexOf(periods.Last());
        }

        /// <summary>
        /// Converts the index of the corresponding <see cref="PricePeriod"/> to a DateTime.
        /// </summary>
        /// <param name="index">The index to convert.</param>
        /// <returns>The DateTime of the corresponding <paramref name="index"/>.</returns>
        protected DateTime ConvertIndexToDateTime(int index)
        {
            var values = TimeSeries.Values.ToArray();
            return values[index].Key;
        }

        #endregion
    }
}
