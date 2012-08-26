namespace Sonneville.PriceTools.Test.Utilities
{
    public class WeeklyProvider : MockProvider
    {
        public override Resolution BestResolution { get { return Resolution.Weeks; } }
    }
}