using System;

namespace Sonneville.PriceTools.Implementation
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    [Serializable]
    public abstract class ShareTransactionImpl : IShareTransaction
    {
        #region Private Members

        private decimal _price;
        private double _shares;
        private decimal _commission;

        #endregion
        
        #region Accessors

        /// <summary>
        ///   Gets the DateTime that the ITransaction occurred.
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        ///   Gets the <see cref="OrderType"/> of this ShareTransaction.
        /// </summary>
        public OrderType OrderType { get; protected set; }

        /// <summary>
        ///   Gets the ticker symbol of the security traded in this IShareTransaction.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        ///   Gets the amount of securities traded in this IShareTransaction.
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
        ///   Gets the value of all securities traded in this IShareTransaction.
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
        ///   Gets the commission charged for this IShareTransaction.
        /// </summary>
        public decimal Commission
        {
            get { return _commission; }
            set
            {
                switch (OrderType)
                {
                    case OrderType.DividendReceipt:
                    case OrderType.DividendReinvestment:
                    case OrderType.Deposit:
                    case OrderType.Withdrawal:
                        if (value != 0)
                            throw new ArgumentOutOfRangeException("value", value, String.Format(Strings.ShareTransaction_Commission_Commission_for__0__transactions_must_be_0_, OrderType));
                        break;
                    default:
                        if (value < 0)
                            throw new ArgumentOutOfRangeException("value", value, Strings.ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_);
                        break;
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
                switch (OrderType)
                {
                    case OrderType.Buy:
                    case OrderType.SellShort:
                    case OrderType.DividendReceipt:
                    case OrderType.DividendReinvestment:
                    case OrderType.Deposit:
                    case OrderType.Withdrawal:
                        return 1;
                    case OrderType.BuyToCover:
                    case OrderType.Sell:
                        return -1;
                    default:
                        throw new NotSupportedException(String.Format("OrderType {0} is unknown.", OrderType));
                }
            }
        }

        #endregion
    }
}