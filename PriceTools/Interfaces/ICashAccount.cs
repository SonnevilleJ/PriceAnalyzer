using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    public interface ICashAccount
    {
        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the CashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the CashAccount.</param>
        void Deposit(DateTime dateTime, decimal amount);

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        void Deposit(Deposit deposit);

        /// <summary>
        /// Deposits a cash dividend into the CashAccount.
        /// </summary>
        /// <param name="dividendReceipt"></param>
        void Deposit(DividendReceipt dividendReceipt);

        /// <summary>
        /// Withdraws cash from the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the CashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the CashAccount.</param>
        void Withdraw(DateTime dateTime, decimal amount);

        /// <summary>
        /// Withdraws cash from the CashAccount.
        /// </summary>
        void Withdraw(Withdrawal withdrawal);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ICashTransaction"/>s in this CashAccount.
        /// </summary>
        ICollection<ICashTransaction> Transactions { get; }

        /// <summary>
        ///   Gets the balance of cash in this CashAccount.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        decimal GetCashBalance(DateTime asOfDate);

        /// <summary>
        /// Validates an <see cref="ICashTransaction"/> without adding it to the CashAccount.
        /// </summary>
        /// <param name="cashTransaction">The <see cref="ICashAccount"/> to validate.</param>
        /// <returns></returns>
        bool TransactionIsValid(ICashTransaction cashTransaction);
    }
}
