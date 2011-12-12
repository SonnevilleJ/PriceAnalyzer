using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract class CashTransaction : ICashTransaction
    {
        #region Constructors

        /// <summary>
        /// Constructs a CashTransaction.
        /// </summary>
        protected internal CashTransaction()
        {
        }

        #endregion

        #region Implementation of ITransaction

        /// <summary>
        ///   Gets the DateTime that the ITransaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType { get; protected set; }

        #endregion

        #region Implementation of ICashTransaction

        private decimal _amount;

        /// <summary>
        ///   Gets the amount of cash in this ICashTransaction.
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
