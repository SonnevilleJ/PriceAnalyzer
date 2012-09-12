using Sonneville.PriceTools;

namespace TestUtilities.Sonneville.PriceTools
{
    public class WeeklyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Weeks; } }
    }
}