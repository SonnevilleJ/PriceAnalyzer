namespace Sonneville.PriceTools.SecurityAnalysis
{
    public abstract class PriceThresholdAnalyzer : SinglePeriodAnalyzer
    {
        public decimal Threshold { get; set; }
    }
}