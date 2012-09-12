using Sonneville.PriceTools;

namespace TestUtilities.Sonneville.PriceTools
{
    public class SecondsProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Seconds; } }
    }
}