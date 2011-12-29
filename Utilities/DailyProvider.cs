using Sonneville.PriceTools;

namespace Sonneville.Utilities
{
	public class DailyProvider : MockProvider
	{
	    public override Resolution BestResolution { get { return Resolution.Days; } }
	}
}