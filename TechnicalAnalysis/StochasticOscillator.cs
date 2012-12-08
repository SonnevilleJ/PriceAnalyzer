using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A momentum indicator that shows the location of the close relative to the high-low range over a set number of periods.
    /// </summary>
    public class StochasticOscillator : PriceSeriesIndicator
    {
        private SimpleMovingAverage _signalLine;

        public StochasticOscillator(IPriceSeries timeSeries, int lookback)
            : base(timeSeries, lookback)
        {
            _signalLine = new SimpleMovingAverage(this, 3);
        }

        protected override decimal Calculate(DateTime index)
        {
            var runDate = index.AddTicks(1);
            var highestHigh = MeasuredPriceSeries.GetPreviousPricePeriods(Lookback, runDate).Max(p => p.High);
            var lowestLow = MeasuredPriceSeries.GetPreviousPricePeriods(Lookback, runDate).Min(p => p.Low);
            var currentClose = MeasuredPriceSeries[index];

            if (highestHigh == lowestLow) return 50.0m;
            return (currentClose - lowestLow)/(highestHigh - lowestLow)*100m;
        }

        public ITimeSeriesIndicator K
        {
            get { return this; }
        }

        public ITimeSeriesIndicator D
        {
            get { return _signalLine; }
        }

        /// <summary>
        /// Gets or sets the lookback period for the signal line - a <see cref="SimpleMovingAverage"/> of the <see cref="StochasticOscillator"/>.
        /// </summary>
        public int SignalLineLookback
        {
            get { return _signalLine.Lookback; }
            set
            {
                ClearCachedValues();
                _signalLine = new SimpleMovingAverage(this, value);
            }
        }
    }
}
