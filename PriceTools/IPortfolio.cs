using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using Sonneville.PriceTools.Services;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a portfolio of investments.
    /// </summary>
    public interface IPortfolio : IMeasurableSecurityBasket
    {
        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this IPortfolio.
        /// </summary>
        EntityCollection<Position> Positions { get; }

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
        ///   Adds an <see cref="ITransaction"/> to this IPortfolio.
        /// </summary>
        void AddTransaction(ITransaction transaction);

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        void Deposit(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Deposits cash to this IPortfolio.
        /// </summary>
        void Deposit(Deposit deposit);

        /// <summary>
        /// Withdraws cash from this IPortfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        void Withdraw(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Withdraws cash from this IPortfolio. Available cash must be greater than or equal to the withdrawn amount.
        /// </summary>
        void Withdraw(Withdrawal withdrawal);

        /// <summary>
        /// Adds transaction history from a CSV file to the IPortfolio.
        /// </summary>
        /// <param name="csvFile">The CSV file containing the transactions to add.</param>
        void AddTransactionHistory(TransactionHistoryCsvFile csvFile);
    }
}
