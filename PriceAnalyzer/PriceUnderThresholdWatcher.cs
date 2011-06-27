using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceUnderThresholdWatcher : Watcher
    {
        protected override bool Evaluate(PricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}