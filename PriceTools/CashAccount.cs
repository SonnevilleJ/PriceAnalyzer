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
            Transactions.Add(new Deposit
                                      {
                                          SettlementDate = dateTime,
                                          Amount = amount
                                      });
        }

        /// <summary>
        /// Withdraws cash from the ICashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the CashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the CashAccount.</param>
        public void Withdraw(DateTime dateTime, decimal amount)
        {
            if (GetCashBalance(dateTime) < amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
            Transactions.Add(new Withdrawal
                                      {
                                          SettlementDate = dateTime,
                                          Amount = amount
                                      });
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

        #region Equality Checks

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(CashAccount)) return false;
            return this == (CashAccount)obj;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 397 * Transactions.GetHashCode();
                return result;
            }
        }

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
