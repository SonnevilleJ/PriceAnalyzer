namespace Sonneville.PriceTools.AutomatedTrading
{
    public class PriceUnderThresholdAnalyzer : PriceThresholdAnalyzer
    {
        protected override bool EvaluatePricePeriod(PricePeriod pricePeriod)
        {
            return pricePeriod.Low <= Threshold;
        }
    }
}