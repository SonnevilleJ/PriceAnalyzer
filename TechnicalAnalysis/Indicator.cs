using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sonneville.PriceTools.Extensions;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A generic indicator used to transform <see cref="PriceTools.ITimeSeries"/> data in order to identify a trend, correlation, reversal, or other meaningful information about the underlying data series.
    /// </summary>
    public abstract class Indicator : IIndicator
    {
        #region Private Members

        private readonly ITimeSeries _cachedValues = TimeSeriesFactory.ConstructMutable();
        private readonly ITimeSeries _measuredTimeSeries;

        /// <summary>
        /// Stores the calculated values for each period.
        /// </summary>
        private ITimeSeries CachedValues
        {
            get { return _cachedValues; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="MeasuredTimeSeries"/>.
        /// </summary>
        /// <param name="timeSeries"> </param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        protected Indicator(ITimeSeries timeSeries, int lookback)
        {
            if (timeSeries == null)
            {
                throw new ArgumentNullException("timeSeries");
            }
            if(timeSeries.TimeSpan() < new TimeSpan(lookback * (long)timeSeries.Resolution))
            {
                // not enough data to calculate at least one indicator value
                //throw new InvalidOperationException("The TimeSpan of timeSeries is too narrow for the given lookback duration.");
            }

            _measuredTimeSeries = timeSeries;
            Lookback = lookback;
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected abstract decimal Calculate(DateTime index);

        /// <summary>
        /// Gets a value indicating whether or not an indicator value is calculable for a given <see cref="DateTime"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual bool CanCalculate(DateTime index)
        {
            var sufficientDates = index >= Head.SeekPeriods(Lookback - 1, Resolution);
            var haveAllData = MeasuredTimeSeries.TimePeriods.Count(p => p.Tail <= index) >= Lookback;
            return sufficientDates && haveAllData;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if a value cannot be calculated for the period ending on or before <see cref="index"/>.
        /// </summary>
        /// <param name="index"></param>
        protected void ThrowIfCannotCalculate(DateTime index)
        {
            if (!CanCalculate(index))
                throw new InvalidOperationException(String.Format("Unable to calculate value for DateTime: {0}", index.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Gets the most recent <paramref name="count"/> <see cref="ITimePeriod"/>s starting from <paramref name="origin"/>.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        protected IEnumerable<ITimePeriod> GetPreviousPeriods(int count, DateTime origin)
        {
            var periods = MeasuredTimeSeries.TimePeriods.Where(p => p.Head <= origin).ToArray();
            if(periods.Count() < count) throw new InvalidOperationException("Not enough ITimePeriods to select.");

            var results = new List<ITimePeriod>();
            for (var i = periods.Count(); i > periods.Count() - count; i--)
            {
                results.Add(periods[i - 1]);
            }
            return results;
        }

        #endregion

        #region Implementation of IIndicator

        /// <summary>
        /// Gets the first DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Head
        {
            get { return MeasuredTimeSeries.Head; }
        }

        /// <summary>
        /// Gets the last DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return MeasuredTimeSeries.Tail; }
        }

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="index">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime index]
        {
            get
            {
                return CachedValues.HasValueInRange(index) ? CachedValues[index] : CalculateAndCache(index);
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this MeasuredTimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod> TimePeriods
        {
            get { return CachedValues.TimePeriods.ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Gets the lookback of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        public int Lookback { get; private set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        public ITimeSeries MeasuredTimeSeries
        {
            get { return _measuredTimeSeries; }
        }

        /// <summary>
        /// The Resolution of this Indicator.
        /// </summary>
        public Resolution Resolution { get { return MeasuredTimeSeries.Resolution; } }

        /// <summary>
        /// Determines if the Indicator has a valid value for a given date.
        /// </summary>
        /// <remarks>Assumes the Indicator has a valid value for every date of the underlying MeasuredTimeSeries.</remarks>
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
            var firstPeriodHeadOfIndicator = MeasuredTimeSeries.TimePeriods.ElementAt(Lookback - 1).Head;
            var timePeriods = MeasuredTimeSeries.TimePeriods.Where(p => p.Head >= firstPeriodHeadOfIndicator);
            foreach(var period in timePeriods)
            {
                CalculateAndCache(period.Head);
            }
        }

        #endregion

        #region Private Methods

        private decimal CalculateAndCache(DateTime index)
        {
            ThrowIfCannotCalculate(index);
            
            var result = Calculate(index);
            AddOrReplaceResult(index, result);
            return result;
        }

        /// <summary>
        /// Records values, storing them with the Tail of the period they represent.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="calculate"></param>
        private void AddOrReplaceResult(DateTime index, decimal calculate)
        {
            var list = CachedValues.TimePeriods as List<ITimePeriod>;
            if (list != null)
            {
                var head = index.CurrentPeriodOpen(Resolution);
                var tail = index.CurrentPeriodClose(Resolution);
                if(list.Any(p => p.Head == head && p.Tail == tail))
                {
                    list.RemoveAll(p => p.Head == head && p.Tail == tail);
                }
                list.Add(TimePeriodFactory.ConstructTimePeriod(head, tail, calculate));
            }
        }

        #endregion
    }
}
