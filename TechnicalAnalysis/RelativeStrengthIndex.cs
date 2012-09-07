using System;
using System.Linq;
using System.Collections.Generic;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A momentum oscillator which measures the speed of changes in price movements.
    /// </summary>
    public class RelativeStrengthIndex : Indicator
    {
        //
        // The algorithms in the RelativeStrengthIndicator class are based on an Excel calculator from the following article:
        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
        //

        #region Private Members

        private readonly List<KeyValuePair<int, decimal>> _gains = new List<KeyValuePair<int, decimal>>();
        private readonly List<KeyValuePair<int, decimal>> _losses = new List<KeyValuePair<int, decimal>>();
        private readonly IDictionary<int, bool> _preCalculatedPeriods = new Dictionary<int, bool>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new RSI <see cref="IIndicator"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="PriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        public RelativeStrengthIndex(PriceSeries priceSeries, int lookback = 14)
            : base(priceSeries, lookback)
        {
        }

        #endregion

        #region Overrides of Indicator

        /// <summary>
        /// Gets the first DateTime in the Indicator.
        /// </summary>
        public override DateTime Head
        {
            get { return ConvertIndexToDateTime(0); }
        }

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected override decimal? Calculate(int index)
        {
            // cannot do anything for the first period; we need to have a delta to even begin
            if (index > 0)
            {
                // if any data is missing, calculate it
                for (var i = index - (Lookback - 1); i > 0 && i <= index; i++)
                {
                    if (!_preCalculatedPeriods.ContainsKey(i))
                    {
                        Precalculate(i);
                    }
                }
                
                // if we have enough data to calculate an RSI value
                if (index >= Lookback)
                {
                    var averageGain = GetAverageGain(index);
                    var averageLoss = GetAverageLoss(index);
                    if (averageLoss != 0)
                    {
                        var relativeStrength = averageGain / -averageLoss;
                        var result = 100.0m - (100.0m/(1.0m + relativeStrength));
                        return result;
                    }
                    return 100.0m;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculates gain and loss data for use in later RSI calculations.
        /// </summary>
        /// <param name="index">The period to calulate.</param>
        private void Precalculate(int index)
        {
            if (index > 0)
            {
                //var sufficientAmount = PreCalculatedPeriods.Count >= index + 1;
                //if (!sufficientAmount || !PreCalculatedPeriods[index])
                //{
                    var change = IndexedPriceSeriesValues[index] - IndexedPriceSeriesValues[index - 1];
                    if (change > 0) _gains.Add(new KeyValuePair<int, decimal>(index, change));
                    if (change < 0) _losses.Add(new KeyValuePair<int, decimal>(index, change));
                    _preCalculatedPeriods[index] = true;
                //}

            }
        }

        #endregion

        #region Private Methods

        private decimal GetAverageGain(int index)
        {
            if (index == Lookback)
            {
                return _gains.Where(kvp => kvp.Key <= index && kvp.Key >= index - Lookback).Sum(kvp => kvp.Value)/Lookback;
            }
            var today = _gains.Where(kvp => kvp.Key == index).ToList();
            var value = today.Count() == 1 ? today.First().Value : 0;

            return ((GetAverageGain(index - 1) * (Lookback - 1)) + value) / Lookback;
        }

        private decimal GetAverageLoss(int index)
        {
            if (index == Lookback)
            {
                return _losses.Where(kvp => kvp.Key <= index && kvp.Key >= index - Lookback).Sum(kvp => kvp.Value) / Lookback;
            }
            var today = _losses.Where(kvp => kvp.Key == index).ToList();
            var value = today.Count() == 1 ? today.First().Value : 0;

            return ((GetAverageLoss(index - 1) * (Lookback - 1)) + value) / Lookback;
        }

        #endregion
    }
}