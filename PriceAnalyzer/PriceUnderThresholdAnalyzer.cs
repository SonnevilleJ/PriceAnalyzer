namespace Sonneville.PriceTools.Analysis
{
    public class PriceUnderThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}