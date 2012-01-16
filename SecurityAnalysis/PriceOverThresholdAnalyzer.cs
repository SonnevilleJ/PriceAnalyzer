namespace Sonneville.PriceTools.SecurityAnalysis
{
    public class PriceOverThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
