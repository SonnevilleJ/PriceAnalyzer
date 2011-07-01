using System;
using System.Reflection;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public abstract class PriceSeriesAnalyzer : Analyzer
    {
        protected PriceSeriesAnalyzer()
        {
            PriceSeriesProperty = OHLC.Close;
        }

        protected PriceSeries PriceSeries
        {
            get { return (PriceSeries) TimeSeries; }
        }

        protected OHLC PriceSeriesProperty { get; set; }

        protected decimal GetValue(PricePeriod pricePeriod)
        {
            var propertyName = Enum.GetName(typeof (OHLC), PriceSeriesProperty);
            var propertyInfo = typeof(PricePeriod).GetProperty(propertyName);
            return (decimal) propertyInfo.GetValue(pricePeriod, null);
        }
    }
}
