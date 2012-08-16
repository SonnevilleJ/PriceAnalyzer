namespace Sonneville.PriceTools.Test.Utilities
{
    public class SecondsProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Seconds; } }
    }
}