using System.Reflection;
using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceUnderThresholdWatcher : Watcher
    {
        public override PropertyInfo Property
        {
            get
            {
                return (typeof (PricePeriod)).GetProperty("Low");
            }
        }

        protected override bool Evaluate(PricePeriod pricePeriod)
        {
            return (decimal)Property.GetValue(pricePeriod, null) <= Threshold;
        }
    }
}