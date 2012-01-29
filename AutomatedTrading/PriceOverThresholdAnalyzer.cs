namespace Sonneville.PriceTools.AutomatedTrading
{
    public class PriceOverThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.High >= Threshold;
        }
    }
}
