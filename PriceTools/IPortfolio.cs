using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Data;

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
        IList<IPosition> Positions { get; }

        /// <summary>
        ///   Retrieves the <see cref="IPosition"/> with Ticker <paramref name="ticker"/>.
        /// </summary>
        /// <param name="ticker">The Ticker symbol of the position to retrieve.</param>
        /// <returns>The <see cref="IPosition"/> with the requested Ticker. Returns null if no <see cref="IPosition"/> is found with the requested Ticker.</returns>
        IPosition GetPosition(string ticker);

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
        /// Adds historical transactions to the Portfolio.
        /// </summary>
        /// <param name="transactionHistory">The historical transactions to add.</param>
        void AddTransactionHistory(ITransactionHistory transactionHistory);

        /// <summary>
        /// Gets an <see cref="IList{IHolding}"/> from the transactions in the IPortfolio.
        /// </summary>
        /// <param name="settlementDate">The latest date used to include a transaction in the calculation.</param>
        /// <returns>An <see cref="IList{IHolding}"/> of the transactions in the IPortfolio.</returns>
        IList<IHolding> CalculateHoldings(DateTime settlementDate);

        /// <summary>
        /// Validates an <see cref="ITransaction"/> without adding it to the IPortfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="ITransaction"/> to validate.</param>
        /// <returns></returns>
        bool TransactionIsValid(ITransaction transaction);
    }
}
