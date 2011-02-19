using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    public interface ICashAccount
    {
        /// <summary>
        /// Deposits cash into the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the ICashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the ICashAccount.</param>
        void Deposit(DateTime dateTime, decimal amount);

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the ICashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the ICashAccount.</param>
        void Withdraw(DateTime dateTime, decimal amount);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ICashTransaction"/>s in this ICashAccount.
        /// </summary>
        EntityCollection<CashTransaction> Transactions { get; }

        /// <summary>
        ///   Gets the balance of cash in this ICashAccount.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        decimal GetCashBalance(DateTime asOfDate);
    }
}
