using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IProfitCalculator
    {
        decimal GrossProfit(Holding holding);

        decimal NetProfit(Holding holding);
    }
}