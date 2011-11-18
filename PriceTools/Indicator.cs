using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform <see cref="ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="IPriceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        protected Indicator(IPriceSeries priceSeries, int lookback)
        {
            if (priceSeries == null)
            {
                throw new ArgumentNullException("priceSeries");
            }
            PriceSeries = priceSeries;
            if(priceSeries.TimeSpan < new TimeSpan(lookback * (long)Resolution))
            {
                throw new InvalidOperationException("The TimeSpan of priceSeries is too narrow for the given Resolution.");
            }
            Lookback = lookback;
            Reset();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Head
        {
            get { return PricePeriods[Lookback - 1].Head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return PriceSeries.Tail; }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// The underlying list of <see cref="IPricePeriod"/>s.
        /// </summary>
        protected IList<IPricePeriod> PricePeriods { get { return PriceSeries.PricePeriods; } }

        /// <summary>
        /// Contains boolean values indicating whether or not all required data is available for calculation of later periods.
        /// </summary>
        /// <remarks>Note that inheriting classes must store this data separately.</remarks>
        protected IDictionary<int, bool> PreCalculatedPeriods { get; private set; }

        /// <summary>
        /// Contains the calculated values for each period. These values are publically accessible through the <see cref="Indicator"/> indexer.
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
        public IPriceSeries PriceSeries { get; private set; }

        /// <summary>
        /// The Resolution of this Indicator. Used when splitting the PriceSeries into periods.
        /// </summary>
        public Resolution Resolution { get { return PriceSeries.Resolution; } }

        /// <summary>
        /// Determines if the Indicator has a valid value for a given date.
        /// </summary>
        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying IPriceSeries.</remarks>
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
            foreach (var pricePeriod in PricePeriods)
            {
                CalculatePeriod(pricePeriod);
            }
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<decimal> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Methods

        private void Reset()
        {
            PreCalculatedPeriods = new Dictionary<int, bool>();
            Results = new Dictionary<int, decimal?>(PriceSeries.PricePeriods.Count - Lookback);
        }

        private void CalculatePeriod(IPricePeriod period)
        {
            Calculate(GetPeriodIndex(period));
        }

        private void CalculatePeriod(DateTime index)
        {
            if (!HasValueInRange(index)) throw new ArgumentOutOfRangeException("index", index, Strings.IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator);

            Calculate(ConvertDateTimeToIndex(index));
        }

        /// <summary>
        /// Converts a DateTime to the index of the corresponding <see cref="IPricePeriod"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> corresponding to the <see cref="IPricePeriod"/> to index.</param>
        /// <returns>The index of the corresponding <see cref="IPricePeriod"/>.</returns>
        private int ConvertDateTimeToIndex(DateTime dateTime)
        {
            var period = GetCorrespondingPeriod(dateTime);
            return GetPeriodIndex(period);
        }

        /// <summary>
        /// Retrieves the index of a given <see cref="IPricePeriod"/> within an <see cref="IPriceSeries"/>.
        /// </summary>
        /// <param name="period">The <see cref="IPricePeriod"/> to index.</param>
        /// <returns>The index of the given <see cref="IPricePeriod"/>.</returns>
        private int GetPeriodIndex(IPricePeriod period)
        {
            return PriceSeries.PricePeriods.IndexOf(period);
        }

        /// <summary>
        /// Retrieves the PricePeriod corresponding to <paramref name="dateTime"/>
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> of the PricePeriod to retrieve.</param>
        /// <returns>The <see cref="PricePeriod"/> for the given DateTime.</returns>
        private IPricePeriod GetCorrespondingPeriod(DateTime dateTime)
        {
            var periods = PriceSeries.GetPricePeriods().Where(p => p.Head <= dateTime && p.Tail >= dateTime);
            if (periods.Count() < 1)
                throw new ArgumentOutOfRangeException(String.Format("The underlying PriceSeries does not have a value for DateTime: {0}.", dateTime));
            if (periods.Count() > 1)
                throw new InvalidDataException(String.Format("The PricePeriod data contains more than one period for the same DateTime: {0}", dateTime));
            return periods.First();
        }

        #endregion
    }
}
