using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Extensions;

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
        private ITimeSeries Results { get; set; }

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

            MeasuredTimeSeries = timeSeries;
            Lookback = lookback;
            Results = TimeSeriesFactory.ConstructMutable();
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the first DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Head
        {
            get { return MeasuredTimeSeries.Head.SeekPeriods(Lookback - 1); }
        }

        /// <summary>
        /// Gets the last DateTime in the Indicator.
        /// </summary>
        public virtual DateTime Tail
        {
            get { return MeasuredTimeSeries.Tail; }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected decimal? Calculate(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IIndicator

        /// <summary>
        /// Gets the value stored at a given index of this Indicator.
        /// </summary>
        /// <param name="dateTime">The DateTime of the desired value.</param>
        /// <returns>The value of the ITimePeriod as of the given DateTime.</returns>
        public decimal this[DateTime dateTime]
        {
            get { return Results[dateTime]; }
        }

        /// <summary>
        /// Gets a collection of the <see cref="IPricePeriod"/>s in this MeasuredTimeSeries.
        /// </summary>
        public IEnumerable<ITimePeriod> TimePeriods
        {
            get { return Results.TimePeriods; }
        }

        /// <summary>
        /// Gets the lookback of this Indicator which specifies how many periods are required for the first indicator value.
        /// </summary>
        /// <example>A 50-period MovingAverage has a Lookback of 50.</example>
        public int Lookback { get; private set; }

        /// <summary>
        /// The underlying data which is to be analyzed by this Indicator.
        /// </summary>
        public ITimeSeries MeasuredTimeSeries { get; private set; }

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
        }

        #endregion

        #region Private Methods

        private void CalculateAndCache(DateTime index)
        {
            if (!HasValueInRange(index)) throw new ArgumentOutOfRangeException("index", index, Strings.IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator);

            throw new NotImplementedException();
        }

        #endregion
    }
}
