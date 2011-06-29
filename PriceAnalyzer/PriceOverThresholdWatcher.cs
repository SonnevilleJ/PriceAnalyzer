using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceOverThresholdWatcher : SinglePeriodWatcher
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
