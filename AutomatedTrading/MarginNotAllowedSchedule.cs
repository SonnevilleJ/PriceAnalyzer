namespace Sonneville.PriceTools.AutomatedTrading
{
    public class MarginNotAllowedSchedule : IMarginSchedule
    {
        public bool IsMarginAllowed
        {
            get { return false; }
        }

        public double GetLeverageRequirement(string ticker)
        {
            return 1.0;
        }
    }
}