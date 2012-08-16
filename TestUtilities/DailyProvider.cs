namespace Sonneville.PriceTools.Test.Utilities
{
	public class DailyProvider : MockProvider
	{
	    public override Resolution BestResolution { get { return Resolution.Days; } }
	}
}