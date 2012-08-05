namespace Sonneville.PriceTools
{
    /// <summary>
    /// A class which holds extension methods for <see cref="IHolding"/>.
    /// </summary>
    public static class HoldingExtensions
    {
        /// <summary>
        /// Calculates the profit of a holding, before commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public static decimal GrossProfit(this IHolding holding)
        {
            return (holding.ClosePrice - holding.OpenPrice) * holding.Shares;
        }

        /// <summary>
        /// Calculates the profit of a holding, after commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        public static decimal NetProfit(this IHolding holding)
        {
            return holding.GrossProfit() - holding.OpenCommission - holding.CloseCommission;
        }
    }
}