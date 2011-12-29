using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public class HourlyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Hours; } }
    }
}