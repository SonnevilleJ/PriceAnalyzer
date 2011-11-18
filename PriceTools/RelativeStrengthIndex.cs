using System.Linq;
using System.Collections.Generic;

namespace Sonneville.PriceTools
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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new RSI <see cref="IIndicator"/>.
        /// </summary>
        /// <param name="priceSeries">The <see cref="IPriceSeries"/> to measure.</param>
        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
        public RelativeStrengthIndex(IPriceSeries priceSeries, int lookback = 14)
            : base(priceSeries, lookback)
        {
        }

        #endregion

        #region Overrides of Indicator

        /// <summary>
        /// Calculates a single value of this Indicator.
        /// </summary>
        /// <param name="index">The index of the value to calculate. The index of the current period is 0.</param>
        protected override void Calculate(int index)
        {
            // if we can calculate any data for future use
            if (index > 0)
            {
                var change = PricePeriods[index].Close - PricePeriods[index - 1].Close;
                if (change > 0) _gains.Add(new KeyValuePair<int, decimal>(index, change));
                if (change < 0) _losses.Add(new KeyValuePair<int, decimal>(index, change));
            }

            // if we have enough data to calculate an RSI value
            var uncalculatedPeriods = PreCalculatedPeriods.Where(pcp => pcp.Key >= index - Lookback && pcp.Key <= index && pcp.Value != true);
            if (uncalculatedPeriods.Count() != 0)
            {
                foreach (var uncalculatedPeriod in uncalculatedPeriods)
                {
                    Calculate(uncalculatedPeriod.Key);
                }
            }
            if (index >= Lookback + 1)
            {
                var averageGain = GetAverageGain(index);
                var averageLoss = GetAverageLoss(index);
                if (averageLoss != 0)
                {
                    var relativeStrength = averageGain/averageLoss;
                    var result = 100.0m - (100.0m/(1.0m + relativeStrength));
                    Results[index] = result;
                }
                Results[index] = 100.0m;
            }
        }

        #endregion

        #region Private Methods

        private decimal GetAverageGain(int index)
        {
            return _gains.Where(kvp => kvp.Key <= index && kvp.Key >= index - Lookback).Sum(kvp => kvp.Value);
        }

        private decimal GetAverageLoss(int index)
        {
            return _losses.Where(kvp => kvp.Key <= index && kvp.Key >= index - Lookback).Sum(kvp => kvp.Value);
        }

        #endregion
    }
}