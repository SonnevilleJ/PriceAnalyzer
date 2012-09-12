using Sonneville.PriceTools;

namespace TestUtilities.Sonneville.PriceTools
{
    public class HourlyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Hours; } }
    }
}