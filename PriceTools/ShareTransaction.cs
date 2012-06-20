using System;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    [Serializable]
    public abstract class ShareTransaction : Transaction
    {
        #region Constructors

        protected internal ShareTransaction(string ticker, DateTime settlementDate, decimal shares, decimal price, decimal commission)
        {
            Ticker = ticker;
            SettlementDate = settlementDate;

            if (shares < 0)
                throw new ArgumentOutOfRangeException("shares", shares, Strings.ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_);
            Shares = shares;

            Price = price;

            if (commission < 0)
                throw new ArgumentOutOfRangeException("commission", commission, Strings.ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_);
            Commission = commission;
        }


        #endregion
        
        #region Accessors

        /// <summary>
        ///   Gets the DateTime that the Transaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; private set; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ShareTransaction.
        /// </summary>
        public string Ticker { get; private set; }

        /// <summary>
        ///   Gets the amount of securities traded in this ShareTransaction.
        /// </summary>
        public decimal Shares { get; private set; }

        /// <summary>
        ///   Gets the value of all securities traded in this ShareTransaction.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        ///   Gets the commission charged for this ShareTransaction.
        /// </summary>
        public decimal Commission { get; private set; }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        public virtual decimal TotalValue
        {
            get { return Math.Round(Price * Shares, 2) + Commission; }
        }

        #endregion
    }
}