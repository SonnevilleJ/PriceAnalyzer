using System;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected override void ValidateTimeSeries()
        {
            if (!(TimeSeries is IPriceSeries))
            {
                throw new InvalidOperationException("TimeSeries must be assigned a PriceSeries object.");
            }
        }

        protected IPriceSeries PriceSeries
        {
            get { return (IPriceSeries) TimeSeries; }
        }
    }
}
