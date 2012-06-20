using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction for an <see cref="CashAccount"/>.
    /// </summary>
    [Serializable]
    public abstract class CashTransaction : Transaction
    {
        #region Constructors
        
        /// <summary>
        /// Constructs a CashTransaction with a given SettlemendDate and Amount.
        /// </summary>
        /// <param name="settlementDate"></param>
        /// <param name="amount"></param>
        protected internal CashTransaction(DateTime settlementDate, decimal amount)
        {
            SettlementDate = settlementDate;
            Amount = amount;
        }

        #endregion

        #region Implementation of CashTransaction

        /// <summary>
        ///   Gets the amount of cash in this CashTransaction.
        /// </summary>
        public decimal Amount { get; private set; }

        #endregion
    }
}
