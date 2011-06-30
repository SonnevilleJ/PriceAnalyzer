using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceUnderThresholdWatcher : PriceThresholdWatcher
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}