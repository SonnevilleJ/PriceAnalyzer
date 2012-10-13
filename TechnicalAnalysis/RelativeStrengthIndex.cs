//using System;

//namespace Sonneville.PriceTools.TechnicalAnalysis
//{
//    /// <summary>
//    /// A momentum oscillator which measures the speed of changes in price movements.
//    /// </summary>
//    public class RelativeStrengthIndex : Indicator
//    {
//        //
//        // The algorithms in the RelativeStrengthIndicator class are based on an Excel calculator from the following article:
//        // http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:relative_strength_index_rsi
//        //

//        private readonly Indicator _averageGains;
//        private readonly Indicator _averageLosses;

//        #region Constructors

//        /// <summary>
//        /// Constructs a new RSI <see cref="IIndicator"/>.
//        /// </summary>
//        /// <param name="timeSeries">The <see cref="IPriceSeries"/> to measure.</param>
//        /// <param name="lookback">The lookback of this Indicator which specifies how many periods are required for the first indicator value.</param>
//        public RelativeStrengthIndex(ITimeSeries timeSeries, int lookback = 14)
//            : base(timeSeries, lookback)
//        {
//            timeSeries.NewDataAvailable += (sender, e) => ClearCachedValues();
//            _averageGains = new RsiAverageGainsIndicator(timeSeries, Lookback);
//            _averageLosses = new RsiAverageLossesIndicator(timeSeries, Lookback);
//        }

//        #endregion

//        #region Overrides of Indicator

//        /// <summary>
//        /// Calculates a single value of this Indicator.
//        /// </summary>
//        /// <param name="index">The index of the value to calculate.</param>
//        protected override decimal Calculate(DateTime index)
//        {
//            var previousGain = _averageGains[index];
//            var previousLoss = _averageLosses[index];

//            if (previousLoss == 0m) return 100m;
//            var rs = previousGain/previousLoss;
//            return 100 - (100/(1 + rs));
//        }

//        #endregion
//    }
//}