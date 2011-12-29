using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
    public class WeeklyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Weeks; } }
    }
}