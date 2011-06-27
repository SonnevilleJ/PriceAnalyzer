using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceOverThresholdWatcher : Watcher
    {
        protected override bool Evaluate(PricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
