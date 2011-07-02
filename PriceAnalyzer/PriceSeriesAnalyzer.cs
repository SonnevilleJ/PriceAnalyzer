using System;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected PriceSeriesAnalyzer()
        {
            PriceSeriesProperty = OHLC.Close;
        }

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

        protected OHLC PriceSeriesProperty { get; set; }

        protected decimal GetValue(DateTime index)
        {
            var propertyName = Enum.GetName(typeof (OHLC), PriceSeriesProperty);
            var propertyInfo = typeof (PricePeriod).GetProperty(propertyName);
            return (decimal) propertyInfo.GetValue(PriceSeries[index], null);
        }
    }
}
