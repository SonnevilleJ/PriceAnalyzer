using System;
using System.Globalization;

namespace Sonneville.PriceTools
{
    /// <summary>
    /// Represents a transaction (or order) for a financial security.
    /// </summary>
    public abstract partial class ShareTransaction : IShareTransaction
    {
        #region Private Members
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a ShareTransaction.
        /// </summary>
        protected internal ShareTransaction()
        {
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets the TransactionType of this ShareTransaction.
        /// </summary>
        public OrderType OrderType
        {
            get { return (OrderType) TransactionType; }
            protected set { TransactionType = (Int32) value; }
        }

        partial void OnCommissionChanging(decimal value)
        {
            switch (OrderType)
            {
                case OrderType.DividendReceipt:
                case OrderType.DividendReinvestment:
                case OrderType.Deposit:
                case OrderType.Withdrawal:
                    if(value != 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value, String.Format(CultureInfo.CurrentCulture, "Commission for {0} must be 0.", OrderType));
                    }
                    break;
                default:
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("value", value, "Commission must be greater than or equal to 0.");
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
                if((PriceDirection > 0 && Price < 0) || (PriceDirection < 0 && Price > 0))
                {
                    Price = -Price;
                }
        }

        private int PriceDirection
        {
            get
            {
                switch(OrderType)
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

        #endregion

        #region Equality Checks

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator==(ShareTransaction left, ShareTransaction right)
        {
            return (left.OrderType == right.OrderType &&
                    left.Commission == right.Commission &&
                    left.SettlementDate == right.SettlementDate &&
                    left.Price == right.Price &&
                    left.Shares == right.Shares &&
                    left.Ticker == right.Ticker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ShareTransaction left, ShareTransaction right)
        {
            return !(left == right);
        }

        #endregion
    }
}
