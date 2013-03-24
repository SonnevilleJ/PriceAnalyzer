using System.Collections.Generic;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPositionFactory
    {
        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">An optional list of <see cref="IShareTransaction"/>s previously in the Position.</param>
        IPosition ConstructPosition(string ticker, params IShareTransaction[] transactions);

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">A list of <see cref="IShareTransaction"/>s previously in the Position.</param>
        IPosition ConstructPosition(string ticker, IEnumerable<IShareTransaction> transactions);
    }
}