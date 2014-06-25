﻿namespace Sonneville.PriceTools.TechnicalAnalysis
{
    /// <summary>
    /// An oscillator which measures the level of <see cref="RelativeStrengthIndex"/> relative to its high/low range.
    /// </summary>
    public class StochRSI : StochasticOscillator
    {
        public StochRSI(ITimeSeries<ITimePeriod<decimal>, decimal> timeSeries, int lookback)
            : base(new RelativeStrengthIndex(timeSeries, lookback), lookback)
        {
        }

        public override void CalculateAll()
        {
            ((TimeSeriesIndicator<decimal>)MeasuredTimeSeries).CalculateAll();
            base.CalculateAll();
        }
    }
}
