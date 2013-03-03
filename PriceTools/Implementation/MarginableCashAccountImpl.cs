using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a single account used to hold cash. The account is marginable and allows a negative balance.
    /// </summary>
    [Serializable]
    internal class MarginableCashAccountImpl : CashAccountImpl, MarginableCashAccount
    {
        private decimal _maximumMargin = 2000.00m;

        /// <summary>
        /// Validates a <see cref="CashTransaction"/> without adding it to the CashAccount.
        /// </summary>
        /// <param name="cashTransaction">The <see cref="CashAccount"/> to validate.</param>
        /// <returns></returns>
        public override bool TransactionIsValid(ICashTransaction cashTransaction)
        {
            if (cashTransaction is Withdrawal)
            {
                var balance = GetCashBalance(cashTransaction.SettlementDate);
                var required = Math.Abs(cashTransaction.Amount);
                return balance - required >= -MaximumMargin;
            }
            return base.TransactionIsValid(cashTransaction);
        }

        /// <summary>
        /// The maximum amount of margin allowed on the account.
        /// </summary>
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