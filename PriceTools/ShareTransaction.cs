using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    public abstract class ShareTransaction : IShareTransaction
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

        #region Equality Checks

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator ==(ShareTransaction left, ShareTransaction right)
        {
            if (ReferenceEquals(null, left)) return false;
            if (ReferenceEquals(null, right)) return false;

            return (left.OrderType == right.OrderType &&
                    left.Commission == right.Commission &&
                    left.SettlementDate == right.SettlementDate &&
                    left.Price == right.Price &&
                    left.Shares == right.Shares &&
                    left.Ticker == right.Ticker);
        }

        /// <summary>
        /// </summary>
        /// <param name = "left"></param>
        /// <param name = "right"></param>
        /// <returns></returns>
        public static bool operator !=(ShareTransaction left, ShareTransaction right)
        {
            return !(left == right);
        }

        #endregion

        #region Implementation of IEquatable<ITransaction>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ITransaction other)
        {
            return Equals((object) other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return this == obj as ShareTransaction;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}