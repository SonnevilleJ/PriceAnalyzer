namespace Sonneville.PriceTools.SecurityAnalysis
{
    public class PriceUnderThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}