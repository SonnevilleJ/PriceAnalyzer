using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected override void ValidateTimeSeries()
        {
            if (!(TimeSeries is PriceSeries))
            {
                throw new InvalidOperationException("TimeSeries must be assigned a PriceSeries object.");
            }
        }

        protected PriceSeries PriceSeries
        {
            get { return (PriceSeries) TimeSeries; }
        }
    }
}
