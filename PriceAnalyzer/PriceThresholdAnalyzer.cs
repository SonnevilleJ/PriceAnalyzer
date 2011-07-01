namespace Sonneville.PriceAnalyzer
{
    public abstract class PriceThresholdAnalyzer : SinglePeriodAnalyzer
    {
        public decimal Threshold { get; set; }
    }
}