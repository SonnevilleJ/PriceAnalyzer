using System.Collections.Generic;
using System.Linq;
using Sonneville.PriceTools.AutomatedTrading.Implementation;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    /// Constructs <see cref="IPosition"/> objects.
    /// </summary>
    public class PositionFactory : IPositionFactory
    {
        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">An optional list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        public IPosition ConstructPosition(string ticker, params ShareTransaction[] transactions)
        {
            return ConstructPosition(ticker, transactions.AsEnumerable());
        }

        /// <summary>
        ///   Constructs a new Position that will handle transactions for a given ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker of the security held in this Position.</param>
        /// <param name="transactions">A list of <see cref="ShareTransaction"/>s previously in the Position.</param>
        public IPosition ConstructPosition(string ticker, IEnumerable<ShareTransaction> transactions)
        {
            var position = new PositionImpl(ticker);
            foreach (var transaction in transactions)
            {
                position.AddTransaction(transaction);
            }
            return position;
        }
    }
}
