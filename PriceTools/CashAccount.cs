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
        /// Deposits a cash dividend into the CashAccount.
        /// </summary>
        /// <param name="dividendReceipt"></param>
        public void Deposit(DividendReceipt dividendReceipt)
        {
            Transactions.Add(dividendReceipt);
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
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return left.Transactions.Count == right.Transactions.Count && left.Transactions.All(transaction => right.Transactions.Where(t => t.OrderType == transaction.OrderType && t.Amount == transaction.Amount && t.SettlementDate == transaction.SettlementDate).Count() != 0);
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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(CashAccount other)
        {
            return other == this;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ICashAccount other)
        {
            return other as CashAccount == this;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return Equals(obj as CashAccount);
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
            return base.GetHashCode();
        }

        #endregion
    }
}
