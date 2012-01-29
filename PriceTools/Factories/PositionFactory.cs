using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Constructs <see cref="IPosition"/> objects.
    /// </summary>
    public static class PositionFactory
    {
        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker"></param>
        public static IPosition CreatePosition(string ticker)
        {
            return new PositionImpl(ticker);
        }
    }
}
