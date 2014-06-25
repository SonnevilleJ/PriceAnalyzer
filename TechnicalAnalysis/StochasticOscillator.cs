using System;
using System.Linq;

namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// A momentum indicator that shows the location of the close relative to the high-low range over a set number of periods.
    /// </summary>
    public class StochasticOscillator : TimeSeriesIndicator
    {
        private readonly SimpleMovingAverage _signalLine;

        public StochasticOscillator(ITimeSeries<ITimePeriod> timeSeries, int lookback)
            : base(timeSeries, lookback)
        {
            _signalLine = new SimpleMovingAverage(this, 3);
        }

        public override void CalculateAll()
        {
            base.CalculateAll();
            _signalLine.CalculateAll();
        }

        protected override decimal Calculate(DateTime index)
        {
            var runDate = index.AddTicks(1);
            decimal highestHigh;
            decimal lowestLow;

            var measuredPriceSeries = MeasuredTimeSeries as IPriceSeries;
            if (measuredPriceSeries != null)
            {
                highestHigh = TimeSeriesUtility.GetPreviousPricePeriods(measuredPriceSeries, Lookback, runDate).Max(p => p.High);
                lowestLow = TimeSeriesUtility.GetPreviousPricePeriods(measuredPriceSeries, Lookback, runDate).Min(p => p.Low);
            }
            else
            {
                highestHigh = TimeSeriesUtility.GetPreviousTimePeriods(MeasuredTimeSeries, Lookback, runDate).Max(p => p.Value());
                lowestLow = TimeSeriesUtility.GetPreviousTimePeriods(MeasuredTimeSeries, Lookback, runDate).Min(p => p.Value());
            }
            var currentClose = MeasuredTimeSeries[index];

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
    }
}
