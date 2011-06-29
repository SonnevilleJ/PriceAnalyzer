using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceUnderThresholdWatcher : SinglePeriodWatcher
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}