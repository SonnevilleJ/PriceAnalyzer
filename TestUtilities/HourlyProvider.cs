namespace Sonneville.PriceTools.Test.Utilities
{
    public class HourlyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Hours; } }
    }
}