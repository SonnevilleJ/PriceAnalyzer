using Sonneville.PriceTools;

namespace Sonneville.PriceAnalyzer
{
    public class PriceUnderThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}