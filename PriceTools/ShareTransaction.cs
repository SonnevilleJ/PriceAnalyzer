using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    /// <summary>
    ///   Represents a transaction (or order) for a financial security.
    /// </summary>
    public abstract partial class ShareTransaction : IShareTransaction
    {
        #region Constructors

        /// <summary>
        ///   Constructs a ShareTransaction.
        /// </summary>
        protected internal ShareTransaction()
        {
        }

        #endregion

        #region Accessors

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
                        return 0; // unknown
                }
            }
        }

        /// <summary>
        ///   Gets the <see cref="OrderType"/> of this ShareTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return (OrderType) EFTransactionType; }
            protected set { EFTransactionType = (Int32) value; }
        }

        /// <summary>
        ///   Gets the total value of this ShareTransaction, including commissions.
        /// </summary>
        public virtual decimal TotalValue
        {
            get { return (Price * (decimal)Shares) + Commission; }
        }

        partial void OnCommissionChanging(decimal value)
        {
            switch (OrderType)
            {
                case OrderType.DividendReceipt:
                case OrderType.DividendReinvestment:
                case OrderType.Deposit:
                case OrderType.Withdrawal:
                    if (value != 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value,
                                                              String.Format(CultureInfo.CurrentCulture,
                                                                            "Commission for {0} must be 0.", OrderType));
                    }
                    break;
                default:
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value,
                                                              "Commission must be greater than or equal to 0.");
                    }
                    break;
            }
        }

        partial void OnSharesChanging(double value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", value, "Shares must be greater than or equal to 0.");
            }
        }

        partial void OnPriceChanged()
        {
            if ((PriceDirection > 0 && Price < 0) || (PriceDirection < 0 && Price > 0))
            {
                Price = -Price;
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