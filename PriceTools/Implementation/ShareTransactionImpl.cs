using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    [Serializable]
    public abstract class ShareTransactionImpl : ShareTransaction
    {
        #region Private Members

        private decimal _price;
        private double _shares;
        private decimal _commission;

        #endregion
        
        #region Accessors

        /// <summary>
        ///   Gets the DateTime that the Transaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this ShareTransaction.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        ///   Gets the amount of securities traded in this ShareTransaction.
        /// </summary>
        public double Shares
        {
            get { return _shares; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, Strings.ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_);
                _shares = value;
            }
        }

        /// <summary>
        ///   Gets the value of all securities traded in this ShareTransaction.
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set
            {
                var price = Math.Abs(value);
                switch (PriceDirection)
                {
                    case 1:
                        _price = price;
                        break;
                    case -1:
                        _price = -price;
                        break;
                }
            }
        }

        /// <summary>
        ///   Gets the commission charged for this ShareTransaction.
        /// </summary>
        public decimal Commission
        {
            get { return _commission; }
            set
            {
                if (this is DividendReinvestment)
                {
                    if (value != 0)
                        throw new ArgumentOutOfRangeException("value", value, Strings.ShareTransactionImpl_Commission_Commission_for_dividend_receipts_must_be_0_);
                }
                else
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("value", value, Strings.ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_);
                }
                _commission = value;
            }
        }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        public virtual decimal TotalValue
        {
            get { return Math.Round(Price * (decimal)Shares, 2) + Commission; }
        }

        #endregion

        #region Private Methods

        private int PriceDirection
        {
            get
            {
                if (this is Buy || this is SellShort || this is DividendReinvestment)
                {
                    return 1;
                }
                if (this is BuyToCover || this is Sell)
                {
                    return -1;
                }
                throw new NotSupportedException("Unknown transaction type.");
            }
        }

        #endregion
    }
}