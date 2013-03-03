using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a single account used to hold cash.
    /// </summary>
    [Serializable]
    internal class CashAccountImpl : ICashAccount
    {
        #region Private Members

        private readonly IList<ICashTransaction> _transactions = new List<ICashTransaction>();
        private readonly object _padlock = new object();

        #endregion

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is deposited into the CashAccount.</param>
        /// <param name="amount">The amount of cash deposited into the CashAccount.</param>
        public void Deposit(DateTime dateTime, decimal amount)
        {
            Deposit(TransactionFactory.ConstructDeposit(dateTime, amount));
        }

        /// <summary>
        /// Deposits cash into the CashAccount.
        /// </summary>
        public void Deposit(IDeposit deposit)
        {
            lock(_padlock)
            {
                _transactions.Add(deposit);
            }
        }

        /// <summary>
        /// Deposits a cash dividend into the CashAccount.
        /// </summary>
        /// <param name="dividendReceipt"></param>
        public void Deposit(IDividendReceipt dividendReceipt)
        {
            lock(_padlock)
            {
                _transactions.Add(dividendReceipt);
            }
        }

        /// <summary>
        /// Withdraws cash from the CashAccount.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> cash is withdrawn from the CashAccount.</param>
        /// <param name="amount">The amount of cash withdrawn from the CashAccount.</param>
        public void Withdraw(DateTime dateTime, decimal amount)
        {
            Withdraw(TransactionFactory.ConstructWithdrawal(dateTime, amount));
        }

        /// <summary>
        /// Withdraws cash from the CashAccount.
        /// </summary>
        public void Withdraw(IWithdrawal withdrawal)
        {
            lock (_padlock)
            {
                VerifySufficientFunds(withdrawal);
                _transactions.Add(withdrawal);
            }
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="CashTransactionImpl"/>s in this CashAccount.
        /// </summary>
        public ICollection<ICashTransaction> Transactions
        {
            get
            {
                lock(_padlock)
                {
                    return new List<ICashTransaction>(_transactions);
                }
            }
        }

        /// <summary>
        ///   Gets the balance of cash in this CashAccount.
        /// </summary>
        /// <param name="asOfDate">The <see cref="DateTime"/> to use.</param>
        public decimal GetCashBalance(DateTime asOfDate)
        {
            lock (_padlock)
            {
                return _transactions.AsParallel()
                    .Where(transaction => transaction.SettlementDate <= asOfDate)
                    .Sum(transaction => transaction.Amount);
            }
        }

        /// <summary>
        /// Validates a <see cref="CashTransactionImpl"/> without adding it to the CashAccount.
        /// </summary>
        /// <param name="cashTransaction">The <see cref="ICashAccount"/> to validate.</param>
        /// <returns></returns>
        public virtual bool TransactionIsValid(ICashTransaction cashTransaction)
        {
            if (cashTransaction is DepositImpl)
            {
                return true;
            }
            if (cashTransaction is WithdrawalImpl)
            {
                return GetCashBalance(cashTransaction.SettlementDate) >= Math.Abs(cashTransaction.Amount);
            }
            return false;
        }

        private void VerifySufficientFunds(IWithdrawal withdrawal)
        {
            if (!TransactionIsValid(withdrawal))
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
        }
    }
}
