using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///   Represents a portfolio of investments.
    /// </summary>
    public interface IPortfolio : ISecurityBasket
    {
        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this Portfolio.
        /// </summary>
        IEnumerable<IPosition> Positions { get; }

        /// <summary>
        ///   Retrieves the <see cref="IPosition"/> with Ticker <paramref name="ticker"/>.
        /// </summary>
        /// <param name="ticker">The Ticker symbol of the position to retrieve.</param>
        /// <returns>The <see cref="IPosition"/> with the requested Ticker. Returns null if no <see cref="IPosition"/> is found with the requested Ticker.</returns>
        IPosition GetPosition(string ticker);

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal GetAvailableCash(DateTime settlementDate);

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this Portfolio.
        /// </summary>
        string CashTicker { get; }
    }
}
