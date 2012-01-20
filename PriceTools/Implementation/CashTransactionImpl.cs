using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    /// Represents a transaction for an <see cref="CashAccount"/>.
    /// </summary>
    [Serializable]
    internal abstract class CashTransactionImpl : CashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction.
        /// </summary>
        protected internal CashTransactionImpl()
        {
        }

        #endregion

        #region Implementation of Transaction

        /// <summary>
        ///   Gets the DateTime that the Transaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType { get; protected set; }

        #endregion

        #region Implementation of CashTransaction

        private decimal _amount;

        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                // ensure Amount is negative for Withdrawal
                var amount = Math.Abs(value);
                switch (OrderType)
                {
                    case OrderType.Withdrawal:
                        _amount = -amount;
                        break;
                    default:
                        _amount = amount;
                        break;
                }
            }
        }

        #endregion
    }
}
