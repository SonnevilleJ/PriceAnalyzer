using System;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    public partial class CashAccount : ICashAccount
    {
        #region Private Members

        #endregion

        #region Constructors

        #endregion

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the CashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the CashAccount.</param>
        public void Deposit(DateTime dateTime, decimal amount)
        {
            Deposit(new Deposit
                        {
                            SettlementDate = dateTime,
                            Amount = amount
                        });
        }

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        public void Deposit(Deposit deposit)
        {
            Transactions.Add(deposit);
        }

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the CashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the CashAccount.</param>
        public void Withdraw(DateTime dateTime, decimal amount)
        {
            Withdraw(new Withdrawal
                         {
                             SettlementDate = dateTime,
                             Amount = amount
                         });
        }

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        public void Withdraw(Withdrawal withdrawal)
        {
            VerifySufficientFunds(withdrawal);
            Transactions.Add(withdrawal);
        }

        /// <summary>
        ///   Gets the balance of cash in this CashAccount.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetCashBalance(DateTime asOfDate)
        {
            return Transactions
                .Where(transaction => transaction.SettlementDate <= asOfDate)
                .Sum(transaction => transaction.Amount);
        }

        private void VerifySufficientFunds(Withdrawal withdrawal)
        {
            if (GetCashBalance(withdrawal.SettlementDate) < Math.Abs(withdrawal.Amount))
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
        }

        #region Equality Checks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CashAccount left, CashAccount right)
        {
            bool cashMatches = false;
            if (left.Transactions.Count == right.Transactions.Count)
            {
                cashMatches = left.Transactions.All(transaction => right.Transactions.Contains(transaction));
            }

            return cashMatches;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CashAccount left, CashAccount right)
        {
            return !(left == right);
        }

        #endregion
    }
}
