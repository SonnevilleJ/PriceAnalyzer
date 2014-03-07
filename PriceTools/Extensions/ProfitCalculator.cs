using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="Holding"/>.
    /// </summary>
    public class ProfitCalculator : IProfitCalculator
    {
        /// <summary>
        /// Calculates the profit of a holding, before commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public decimal GrossProfit(Holding holding)
        {
            return (holding.ClosePrice - holding.OpenPrice)*holding.Shares;
        }

        /// <summary>
        /// Calculates the profit of a holding, after commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public decimal NetProfit(Holding holding)
        {
            return GrossProfit(holding) - holding.OpenCommission - holding.CloseCommission;
        }
    }
}