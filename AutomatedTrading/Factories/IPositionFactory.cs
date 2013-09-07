using System.Collections.Generic;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    public interface IPositionFactory
    {
        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">An optional list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        Position ConstructPosition(string ticker, params ShareTransaction[] transactions);

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">A list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        Position ConstructPosition(string ticker, IEnumerable<ShareTransaction> transactions);
    }
}