using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A generic indicator used to transform <see cref="ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying ITimeSeries data.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        #region Private Members

        private readonly object _padlock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="IPriceSeries"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to measure.</param>
        /// <param name="range">The range of this Indicator which specifies how many periods are required for the first indicator value.</param>
        protected Indicator(IPriceSeries priceSeries, int range)
        {
            if (priceSeries == null)
            {
                throw new ArgumentNullException("priceSeries");
            }
            PriceSeries = priceSeries;
            Resolution = priceSeries.Resolution;
            if(priceSeries.TimeSpan < new TimeSpan(range * (long)Resolution))
            {
                throw new InvalidOperationException("The TimeSpan of priceSeries is too narrow for the given PriceSeriesResolution.");
            }
            Dictionary = new Dictionary<DateTime, decimal?>(priceSeries.PricePeriods.Count - range);
            Range = range;
        }

        #endregion

        private IDictionary<DateTime, decimal?> Dictionary { get; set; }

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the ITimeSeries.
        /// </summary>
        public virtual DateTime Head
        {
            get { return PriceSeries.Head.Add(new TimeSpan((long) Resolution * (Range - 1))); }
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
                    this[date] = Calculate(date);
                }
            }
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
                return value ?? Calculate(index);
            }
            protected set { Dictionary[index] = value; }
        }

        /// <summary>
        /// Gets the number of periods for which this Indicator has a value.
        /// </summary>
        public virtual int Span
        {
            get
            {
                CalculateAll();
                return Dictionary.Count;
            }
        }

        /// <summary>
        /// Gets the range of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Range of 50.</example>
        public int Range { get; private set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        public ITimeSeries PriceSeries { get; private set; }

        /// <summary>
        /// An object to lock when performing thread unsafe tasks.
        /// </summary>
        protected object Padlock
        {
            get { return _padlock; }
        }

        /// <summary>
        /// The Resolution of this Indicator. Used when splitting the PriceSeries into periods.
        /// </summary>
        public PriceSeriesResolution Resolution { get; private set; }

        /// <summary>
        /// Determines if the Indicator has a valid value for a given date.
        /// </summary>
        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying IPriceSeries.</remarks>
        /// <param name="settlementDate">The date to check.</param>
        /// <returns>A value indicating if the Indicator has a valid value for the given date.</returns>
        public virtual bool HasValueInRange(DateTime settlementDate)
        {
            return (settlementDate >= Head && settlementDate <= Tail);
        }

        #endregion
    }
}
