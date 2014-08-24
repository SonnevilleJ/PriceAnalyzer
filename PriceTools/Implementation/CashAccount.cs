using System;
using System.Collections.Generic;
using System.Linq;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    internal class CashAccount : ICashAccount
    {
        private readonly IList<CashTransaction> _transactions = new List<CashTransaction>();
        private readonly ITransactionFactory _transactionFactory;

        public CashAccount()
            : this(new TransactionFactory())
        {
        }

        public CashAccount(ITransactionFactory transactionFactory)
        {
            _transactionFactory = transactionFactory;
        }

        public void Deposit(DateTime dateTime, decimal amount)
        {
            Deposit(_transactionFactory.ConstructDeposit(dateTime, amount));
        }

        public void Deposit(Deposit deposit)
        {
            _transactions.Add(deposit);
        }

        public void Deposit(DividendReceipt dividendReceipt)
        {
            _transactions.Add(dividendReceipt);
        }

        public void Withdraw(DateTime dateTime, decimal amount)
        {
            Withdraw(_transactionFactory.ConstructWithdrawal(dateTime, amount));
        }

        public void Withdraw(Withdrawal withdrawal)
        {
            VerifySufficientFunds(withdrawal);
            _transactions.Add(withdrawal);
        }

        public ICollection<CashTransaction> Transactions
        {
            get { return new List<CashTransaction>(_transactions); }
        }

        public decimal GetCashBalance(DateTime asOfDate)
        {
            return _transactions.AsParallel()
                .Where(transaction => transaction.SettlementDate <= asOfDate)
                .Sum(transaction => transaction.Amount);
        }

        public virtual bool TransactionIsValid(CashTransaction cashTransaction)
        {
            if (cashTransaction is Deposit)
            {
                return true;
            }
            if (cashTransaction is Withdrawal)
            {
                return GetCashBalance(cashTransaction.SettlementDate) >= Math.Abs(cashTransaction.Amount);
            }
            return false;
        }

        private void VerifySufficientFunds(Withdrawal withdrawal)
        {
            if (!TransactionIsValid(withdrawal))
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
        }
    }
}
