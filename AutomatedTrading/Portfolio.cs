using System;
using System.Collections.Generic;

namespace Sonneville.PriceTools.AutomatedTrading
{
    /// <summary>
    ///   Represents a portfolio of investments.
    /// </summary>
    public interface Portfolio : SecurityBasket
    {
        /// <summary>
        ///   Gets an <see cref = "IList{T}" /> of positions held in this Portfolio.
        /// </summary>
        IEnumerable<Position> Positions { get; }

        /// <summary>
        ///   Retrieves the <see cref="Position"/> with Ticker <paramref name="ticker"/>.
        /// </summary>
        /// <param name="ticker">The Ticker symbol of the position to retrieve.</param>
        /// <returns>The <see cref="Position"/> with the requested Ticker. Returns null if no <see cref="Position"/> is found with the requested Ticker.</returns>
        Position GetPosition(string ticker);

        /// <summary>
        ///   Gets the amount of uninvested cash in this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> to use.</param>
        decimal GetAvailableCash(DateTime settlementDate);

        /// <summary>
        /// Gets or sets the ticker to use for the holding of cash in this Portfolio.
        /// </summary>
        string CashTicker { get; }

        /// <summary>
        ///   Adds an <see cref="Transaction"/> to this Portfolio.
        /// </summary>
        void AddTransaction(Transaction transaction);

        /// <summary>
        /// Deposits cash to this Portfolio.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the deposit.</param>
        /// <param name="cashAmount">The amount of cash deposited.</param>
        void Deposit(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Deposits cash to this Portfolio.
        /// </summary>
        void Deposit(Deposit deposit);

        /// <summary>
        /// Withdraws cash from this Portfolio. AvailableCash must be greater than or equal to the withdrawn amount.
        /// </summary>
        /// <param name="settlementDate">The <see cref="DateTime"/> of the withdrawal.</param>
        /// <param name="cashAmount">The amount of cash withdrawn.</param>
        void Withdraw(DateTime settlementDate, decimal cashAmount);

        /// <summary>
        /// Withdraws cash from this Portfolio. Available cash must be greater than or equal to the withdrawn amount.
        /// </summary>
        void Withdraw(Withdrawal withdrawal);

        /// <summary>
        /// Adds historical transactions to the Portfolio.
        /// </summary>
        /// <param name="transactions">The historical transactions to add.</param>
        void AddTransactions(IEnumerable<Transaction> transactions);

        /// <summary>
        /// Validates an <see cref="Transaction"/> without adding it to the Portfolio.
        /// </summary>
        /// <param name="transaction">The <see cref="Transaction"/> to validate.</param>
        /// <returns></returns>
        bool TransactionIsValid(Transaction transaction);
    }
}
