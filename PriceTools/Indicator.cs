using System;
using System.Collections;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform <see cref="ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        #region Private Members

        private IDictionary<DateTime, decimal?> Dictionary { get; set; }
        private readonly object _padlock = new object();

        #endregion

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
            Dictionary = new Dictionary<DateTime, decimal?>(priceSeries.PricePeriods.Count - lookback);
            Lookback = lookback;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Head
        {
            get { return PriceSeries.GetPricePeriods(Resolution)[Lookback - 1].Head; }
        }

        /// <summary>
        /// Gets the last DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return PriceSeries.Tail; }
        }

        #endregion

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        /// <returns>The value of this Indicator for the given period.</returns>
        protected abstract decimal Calculate(DateTime index);

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        public virtual void CalculateAll()
        {
            for (var date = Head; date <= Tail; date = IncrementDate(date))
            {
                if (HasValueInRange(date))
                {
                    this[date] = CalculatePeriod(date);
                }
            }
        }

        private decimal CalculatePeriod(DateTime index)
        {
            if (!HasValueInRange(index))
            {
                throw new ArgumentOutOfRangeException("index", index, Strings.IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator);
            }
            return Calculate(index);
        }

        /// <summary>
        /// Increments a date by 1 day.
        /// </summary>
        /// <param name="date">The date to increment.</param>
        /// <returns>A <see cref="DateTime"/> 1 day after <paramref name="date"/>.</returns>
        protected static DateTime IncrementDate(DateTime date)
        {
            return date.AddDays(1);
        }

        #region Implementation of ITimeSeries

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>THe value of the ITimeSeries as of the given DateTime.</returns>
        public virtual decimal this[DateTime index]
        {
            get
            {
                decimal? value;
                Dictionary.TryGetValue(index, out value);
                return value ?? CalculatePeriod(index);
            }
            protected set
            {
                lock (Padlock)
                {
                    Dictionary[index] = value;
                }
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
        /// An object to lock when performing thread unsafe tasks.
        /// </summary>
        private object Padlock
        {
            get { return _padlock; }
        }

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
    }
}
