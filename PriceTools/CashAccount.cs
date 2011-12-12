using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    public class CashAccount : ICashAccount
    {
        #region Private Members

        private readonly ICollection<ICashTransaction> _transactions = new List<ICashTransaction>();
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
            _transactions.Add(deposit);
        }

        /// <summary>
        /// Deposits a cash dividend into the CashAccount.
        /// </summary>
        /// <param name="dividendReceipt"></param>
        public void Deposit(DividendReceipt dividendReceipt)
        {
            _transactions.Add(dividendReceipt);
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
            _transactions.Add(withdrawal);
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ICashTransaction"/>s in this ICashAccount.
        /// </summary>
        public ICollection<ICashTransaction> Transactions
        {
            get { return new List<ICashTransaction>(_transactions); }
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
    }
}
