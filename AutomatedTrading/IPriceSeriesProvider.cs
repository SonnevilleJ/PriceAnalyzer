namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPriceSeriesProvider
    {
        IPriceSeries GetPriceSeries(string ticker);
    }
}