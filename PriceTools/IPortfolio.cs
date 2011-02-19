using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using Sonneville.PriceTools.Data;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a portfolio of investments.
    /// </summary>
    public interface IPortfolio : ITimeSeries
    {
        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this IPortfolio.
        /// </summary>
        EntityCollection<Position> Positions { get; }

        /// <summary>
        ///   Gets the <see cref="ICashAccount"/> used by this IPortfolio.
        /// </summary>
        CashAccount CashAccount { get; }

        /// <summary>
        ///   Gets the amount of uninvested cash in this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal GetAvailableCash(DateTime settlementDate);

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this IPortfolio.
        /// </summary>
        string CashTicker { get; }

        /// <summary>
        ///   Gets the current total value of this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal GetValue(DateTime settlementDate);

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        /// <param name="commission">The commission charge for the transaction.</param>
        void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price, double shares, decimal commission);

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        /// <param name="shares">The number of shares.</param>
        void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price, double shares);

        /// <summary>
        ///   Adds an <see cref="IShareTransaction"/> to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the transaction.</param>
        /// <param name="type">The <see cref="OrderType"/> of the transaction.</param>
        /// <param name="ticker">The ticker symbol to use for the transaction.</param>
        /// <param name="price">The per-share price of the ticker symbol.</param>
        void AddTransaction(DateTime settlementDate, OrderType type, string ticker, decimal price);

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        void Deposit(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Withdraws cash from this IPortfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        void Withdraw(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Adds transaction history from a CSV file to the IPortfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        void AddTransactionHistory(TransactionHistoryCsvFile csvFile);
    }
}