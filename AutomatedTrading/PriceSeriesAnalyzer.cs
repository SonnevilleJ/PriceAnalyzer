using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected override void ValidateTimeSeries()
        {
            if (!(TimePeriod is IPriceSeries))
            {
                throw new InvalidOperationException("ITimePeriod must be assigned a PriceSeries object.");
            }
        }

        protected IPriceSeries PriceSeries
        {
            get { return (IPriceSeries) TimePeriod; }
        }
    }
}
