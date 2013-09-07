using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="Holding"/>.
    /// </summary>
    public static class HoldingExtensions
    {
        /// <summary>
        /// Calculates the profit of a holding, before commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public static decimal GrossProfit(this Holding holding)
        {
            return (holding.ClosePrice - holding.OpenPrice)*holding.Shares;
        }

        /// <summary>
        /// Calculates the profit of a holding, after commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public static decimal NetProfit(this Holding holding)
        {
            return holding.GrossProfit() - holding.OpenCommission - holding.CloseCommission;
        }
    }
}