namespace Sonneville.PriceAnalyzer
{
    public abstract class PriceThresholdWatcher : SinglePeriodWatcher
    {
        public decimal Threshold { get; set; }
    }
}