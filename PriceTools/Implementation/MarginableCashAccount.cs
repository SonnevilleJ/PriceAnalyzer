using System;

namespace Sonneville.PriceTools.Implementation
{
    [Serializable]
    internal class MarginableCashAccount : CashAccount, IMarginableCashAccount
    {
        private decimal _maximumMargin = 2000.00m;

        public override bool TransactionIsValid(CashTransaction cashTransaction)
        {
            if (cashTransaction is Withdrawal)
            {
                var balance = GetCashBalance(cashTransaction.SettlementDate);
                var required = Math.Abs(cashTransaction.Amount);
                return balance - required >= -MaximumMargin;
            }
            return base.TransactionIsValid(cashTransaction);
        }

        public decimal MaximumMargin
        {
            get
            {
                return _maximumMargin;
            }
            set 
            {
                _maximumMargin = value < 0 ? Math.Abs(value) : value;
            }
        }
    }
}