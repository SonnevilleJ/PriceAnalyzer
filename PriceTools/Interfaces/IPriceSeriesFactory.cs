namespace Sonneville.PriceTools
{
    public interface IPriceSeriesFactory
    {
        IPriceSeries ConstructPriceSeries(string ticker);

        IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution);

        IPriceSeries ConstructConstantPriceSeries(string ticker);
    }
}