using System;
using System.Collections.Generic;
using Sonneville.PriceTools.Implementation;

namespace Sonneville.PriceTools
{
    public interface ICashAccount
    {
        void Deposit(DateTime dateTime, decimal amount);

        void Deposit(Deposit deposit);

        void Deposit(DividendReceipt dividendReceipt);

        void Withdraw(DateTime dateTime, decimal amount);

        void Withdraw(Withdrawal withdrawal);

        ICollection<CashTransaction> Transactions { get; }

        decimal GetCashBalance(DateTime asOfDate);

        bool TransactionIsValid(CashTransaction cashTransaction);
    }
}
