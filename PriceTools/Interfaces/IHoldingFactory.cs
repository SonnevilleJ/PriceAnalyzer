using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface IHoldingFactory
    {
        /// <summary>
        /// Constructs a new <see cref="Holding"/> object.
        /// </summary>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="openCommission"></param>
        /// <param name="closePrice"></param>
        /// <param name="closeCommission"></param>
        /// <returns></returns>
        Holding ConstructHolding(decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);

        /// <summary>
        /// Constructs a new <see cref="Holding"/> object.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="closePrice"></param>
        /// <returns></returns>
        Holding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal closePrice);

        /// <summary>
        /// Constructs a new <see cref="Holding"/> object.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="head"></param>
        /// <param name="tail"></param>
        /// <param name="shares"></param>
        /// <param name="openPrice"></param>
        /// <param name="openCommission"></param>
        /// <param name="closePrice"></param>
        /// <param name="closeCommission"></param>
        /// <returns></returns>
        Holding ConstructHolding(string ticker, DateTime head, DateTime tail, decimal shares, decimal openPrice, decimal openCommission, decimal closePrice, decimal closeCommission);
        
        /// <summary>
        /// Gets an <see cref="IList{T}"/> from the transactions in the Position.
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