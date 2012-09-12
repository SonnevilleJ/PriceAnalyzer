using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected override void ValidateTimeSeries()
        {
            if (!(TimePeriod is PriceSeries))
            {
                throw new InvalidOperationException("TimePeriod must be assigned a PriceSeries object.");
            }
        }

        protected PriceSeries PriceSeries
        {
            get { return (PriceSeries) TimePeriod; }
        }
    }
}
