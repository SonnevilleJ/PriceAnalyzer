using Sonneville.PriceTools;

namespace TestUtilities.Sonneville.PriceTools
{
	public class DailyProvider : MockProvider
	{
	    public override Resolution BestResolution { get { return Resolution.Days; } }
	}
}