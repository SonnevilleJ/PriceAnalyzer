using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public class ProfitCalculator : IProfitCalculator
    {
        public decimal GrossProfit(Holding holding)
        {
            return (holding.ClosePrice - holding.OpenPrice)*holding.Shares;
        }

        public decimal NetProfit(Holding holding)
        {
            return GrossProfit(holding) - holding.OpenCommission - holding.CloseCommission;
        }
    }
}