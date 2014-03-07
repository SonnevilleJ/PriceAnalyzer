using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IProfitCalculator
    {
        /// <summary>
        /// Calculates the profit of a holding, before commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        decimal GrossProfit(Holding holding);

        /// <summary>
        /// Calculates the profit of a holding, after commissions.
        /// </summary>
        /// <param name="holding"></param>
        /// <returns></returns>
        decimal NetProfit(Holding holding);
    }
}