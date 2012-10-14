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

        private ITimeSeries _cachedValues = TimeSeriesFactory.ConstructMutable();
        private readonly ITimeSeries _measuredTimeSeries;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an Indicator for a given <see cref="MeasuredTimeSeries"/>.
        /// </summary>
        /// <param name="timeSeries">The <see cref="ITimeSeries"/> to transform.</param>
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

        #region Protected / Abstract Members

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate.</param>
        protected abstract decimal Calculate(DateTime index);

        /// <summary>
        /// Gets a value indicating whether or not an indicator value is calculable for a given <see cref="DateTime"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual bool CanCalculate(DateTime index)
        {
            var sufficientDates = index >= MeasuredTimeSeries.TimePeriods.ElementAt(Lookback - 1).Tail;
            var haveAllData = MeasuredTimeSeries.TimePeriods.Count(p => p.Tail <= index) >= Lookback;
            return sufficientDates && haveAllData;
        }

        /// <summary>
        /// Clears any cached data values so they will be recalculated.
        /// </summary>
        /// <remarks>Overriding implementations should call the base method.</remarks>
        protected virtual void ClearCachedValues()
        {
            _cachedValues = TimeSeriesFactory.ConstructMutable();
            InvokeNewDataAvailable(new NewDataAvailableEventArgs {Head = Head, Tail = Tail});
        }

        #endregion

        #region Implementation of IIndicator

        /// <summary>
        /// Gets the first DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Head
        {
            get { return MeasuredTimeSeries.TimePeriods.ElementAt(Lookback - 1).Head; }
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
                var dateTime = index.CurrentPeriodClose(Resolution);
                return CachedValues.HasValueInRange(dateTime) ? CachedValues[dateTime] : CalculateAndCache(dateTime);
            }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this MeasuredTimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod> TimePeriods
        {
            get
            {
                EnsureAllCalculated();
                return CachedValues.TimePeriods.ToList().AsReadOnly();
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
        ///   Event which is invoked when new data is available for the Indicator.
        /// </summary>
        public event EventHandler<NewDataAvailableEventArgs> NewDataAvailable;

        /// <summary>
        /// Pre-caches all values for this Indicator.
        /// </summary>
        public virtual void CalculateAll()
        {
            foreach(var period in MeasuredTimeSeries.TimePeriods.Where(p => p.Head >= Head))
            {
                CalculateAndCache(period.Tail);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures values have been calculated for all possible periods.
        /// </summary>
        private void EnsureAllCalculated()
        {
            var cachedPeriods = CachedValues.TimePeriods.ToArray();
            foreach (var period in MeasuredTimeSeries.TimePeriods.Where(p => p.Head >= Head))
            {
                if (cachedPeriods.All(p => p.Head != period.Head))
                    CalculateAndCache(period.Tail);
            }
        }

        /// <summary>
        /// Stores the calculated values for each period.
        /// </summary>
        private ITimeSeries CachedValues
        {
            get { return _cachedValues; }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if a value cannot be calculated for the period ending on or before <see cref="index"/>.
        /// </summary>
        /// <param name="index"></param>
        private void ThrowIfCannotCalculate(DateTime index)
        {
            if (!CanCalculate(index))
                throw new ArgumentOutOfRangeException("index", index, String.Format("Unable to calculate value for DateTime: {0}", index.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Invokes the NewDataAvailable event.
        /// </summary>
        /// <param name="e">The NewPriceDataEventArgs to pass.</param>
        private void InvokeNewDataAvailable(NewDataAvailableEventArgs e)
        {
            var eventHandler = NewDataAvailable;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

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
