using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a portfolio of investments.
    /// </summary>
    public interface IPortfolio : ITimeSeries
    {
        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of open positions held in this Portfolio.
        /// </summary>
        IList<IPosition> OpenPositions { get; }

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        decimal AvailableCash { get; }

        /// <summary>
        ///   Gets the current total value of this Portfolio.
        /// </summary>
        decimal GetValue();

        /// <summary>
        ///   Gets the total value of this Portfolio as of a given <see cref = "DateTime" />.
        /// </summary>
        /// <param name = "asOfDate">The <see cref = "DateTime" /> of which the value should be retrieved.</param>
        /// <returns>The total value of this Portfolio as of the given <see cref = "DateTime" />.</returns>
        decimal GetValue(DateTime asOfDate);

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> to add to this portfolio.</param>
        void AddTransaction(ITransaction transaction);
    }
}