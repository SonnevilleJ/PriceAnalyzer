namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface ISignalProcessor
    {
        void Signal(IPriceSeries priceSeries, double direction, double magnitude);
    }
}