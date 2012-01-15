namespace Sonneville.PriceTools.Analysis
{
    public abstract class PriceThresholdAnalyzer : SinglePeriodAnalyzer
    {
        public decimal Threshold { get; set; }
    }
}