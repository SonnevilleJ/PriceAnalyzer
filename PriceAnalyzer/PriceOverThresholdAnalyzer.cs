namespace Sonneville.PriceTools.Analysis
{
    public class PriceOverThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(IPricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
