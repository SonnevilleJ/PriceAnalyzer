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
        IEnumerable<IPosition> OpenPositions { get; }

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        decimal AvailableCash { get; }

        /// <summary>
        ///   Gets the current total value of this Portfolio.
        /// </summary>
        decimal GetValue();

        /// <summary>
        ///   Adds an <see cref="ITransaction"/> to this portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> to add to this portfolio.</param>
        void AddTransaction(ITransaction transaction);
    }
}