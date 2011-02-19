namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="ICashAccount"/>.
    /// </summary>
    public abstract partial class CashTransaction : ICashTransaction
    {
        #region Private Members

        private OrderType _type;

        #endregion

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
        ///   Gets the <see cref = "PriceTools.OrderType" /> of this CashTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return _type; }
            protected set { _type = value; }
        }

        #endregion

        /// <summary>
        /// Ensure Amount is positive for Deposit and negative for Withdrawal.
        /// </summary>
        partial void OnAmountChanged()
        {
            if ((OrderType == OrderType.Deposit && Amount < 0) || (OrderType == OrderType.Withdrawal && Amount > 0))
            {
                Amount = -Amount;
            }
        }
    }
}
