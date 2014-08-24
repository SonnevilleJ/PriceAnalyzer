using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class PriceSeriesFactory : IPriceSeriesFactory
    {
        public IPriceSeries ConstructPriceSeries(string ticker)
        {
            return ConstructPriceSeries(ticker, Resolution.Days);
        }

        public IPriceSeries ConstructPriceSeries(string ticker, Resolution resolution)
        {
            return new PriceSeries(resolution) {Ticker = ticker};
        }

        public IPriceSeries ConstructConstantPriceSeries(string ticker)
        {
            return new ConstantPriceSeries(ticker);
        }
    }
}