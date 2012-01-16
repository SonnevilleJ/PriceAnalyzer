namespace Sonneville.PriceTools.AutomatedTrading
{
    public abstract class PriceThresholdAnalyzer : SinglePeriodAnalyzer
    {
        public decimal Threshold { get; set; }
    }
}