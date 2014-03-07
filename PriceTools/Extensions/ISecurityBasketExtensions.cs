using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ISecurityBasketExtensions
    {
        /// <summary>
        /// Gets an <see cref="IList{Holding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{Holding}"/> of the transactions in the Position.</returns>
        IList<Holding> CalculateHoldings(ISecurityBasket basket, DateTime settlementDate);

        /// <summary>
        /// Gets an <see cref="IList{Holding}"/> from the transactions in the Position.
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{Holding}"/> of the transactions in the Position.</returns>
        IList<Holding> CalculateHoldings(IEnumerable<Transaction> transactions, DateTime settlementDate);
    }
}