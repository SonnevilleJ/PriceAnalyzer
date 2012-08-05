using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public class SecondsProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Seconds; } }
    }
}