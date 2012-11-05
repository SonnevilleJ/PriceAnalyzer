using Sonneville.PriceTools.AutomatedTrading.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs <see cref="Position"/> objects.
    /// </summary>
    public static class PositionFactory
    {
        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker"></param>
        public static Position CreatePosition(string ticker)
        {
            return new PositionImpl(ticker);
        }
    }
}
